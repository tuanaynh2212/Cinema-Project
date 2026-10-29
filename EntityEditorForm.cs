using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace CinemaManagement;

public sealed class FieldSpec
{
    public string Column { get; init; } = "";
    public string Label { get; init; } = "";
    public string Kind { get; init; } = "text";
    public bool Required { get; init; }
}

public sealed class EntityDefinition
{
    public string Title { get; init; } = "";
    public string Table { get; init; } = "";
    public string IdColumn { get; init; } = "";
    public string SelectSql { get; init; } = "";
    public List<FieldSpec> Fields { get; init; } = new();
}

public sealed class EntityEditorForm : Form
{
    private readonly EntityDefinition _definition;
    private readonly int? _id;
    private readonly Dictionary<FieldSpec, Control> _controls = new();

    public EntityEditorForm(EntityDefinition definition, int? id = null)
    {
        _definition = definition;
        _id = id;
        Text = (id.HasValue ? "Chỉnh sửa: " : "Thêm mới: ") + definition.Title;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Ui.Background;
        Font = new Font("Segoe UI", 9.5F);
        MinimumSize = new Size(540, 500);
        Size = new Size(620, Math.Min(850, 200 + definition.Fields.Count * 70));
        BuildLayout();
        if (_id.HasValue) LoadRecord(_id.Value);
    }

    private void BuildLayout()
    {
        Panel main = new() { Dock = DockStyle.Fill, Padding = new Padding(24), BackColor = Ui.Background };
        Label title = Ui.MakeLabel(_id.HasValue ? "Chỉnh sửa thông tin" : "Nhập thông tin mới", 18, true);
        title.Dock = DockStyle.Top;
        FlowLayoutPanel fields = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Padding = new Padding(0, 12, 0, 8) };
        foreach (FieldSpec spec in _definition.Fields)
        {
            Panel row = new() { Width = 530, Height = spec.Kind == "multiline" ? 112 : 64, Margin = new Padding(0, 2, 0, 4) };
            Label label = Ui.MakeLabel(spec.Label + (spec.Required ? " *" : ""), 9, true); label.Location = new Point(2, 2);
            Control input = CreateInput(spec);
            input.Location = new Point(2, 25); input.Width = 495;
            if (input is ComboBox combo) combo.Height = 30;
            if (input is DateTimePicker date) date.Width = 240;
            if (input is TextBox text)
            {
                text.Height = spec.Kind == "multiline" ? 76 : 31;
                if (spec.Kind == "multiline") { text.Multiline = true; text.ScrollBars = ScrollBars.Vertical; }
                Ui.StyleTextBox(text);
            }
            row.Controls.Add(label); row.Controls.Add(input); fields.Controls.Add(row);
            _controls.Add(spec, input);
        }
        Panel bottom = new() { Dock = DockStyle.Bottom, Height = 56, BackColor = Ui.Background };
        Button save = Ui.MakeButton("LƯU THÔNG TIN", Ui.Accent); save.Size = new Size(150, 40); save.Anchor = AnchorStyles.Right | AnchorStyles.Top; save.Location = new Point(305, 6); save.Click += Save_Click;
        Button cancel = Ui.MakeButton("HỦY", Color.FromArgb(226, 229, 239), Ui.Text); cancel.Size = new Size(75, 40); cancel.Anchor = AnchorStyles.Right | AnchorStyles.Top; cancel.Location = new Point(465, 6); cancel.Click += (s, e) => Close();
        bottom.Controls.Add(save); bottom.Controls.Add(cancel);
        main.Controls.Add(fields); main.Controls.Add(title); main.Controls.Add(bottom);
        Controls.Add(main);
        AcceptButton = save;
    }

    private Control CreateInput(FieldSpec spec)
    {
        switch (spec.Kind)
        {
            case "bool": return new CheckBox { Text = "Đang hoạt động", Checked = true, AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            case "datetime": return new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", Width = 260, Font = new Font("Segoe UI", 10F), ShowUpDown = true };
            case "comboRole":
            {
                ComboBox cb = new() { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F) };
                cb.Items.AddRange(new object[] { "Admin", "Manager", "Customer" }); cb.SelectedIndex = 2; return cb;
            }
            case "comboMovie": return LoadCombo("SELECT MovieID AS Id, Title AS Display FROM dbo.Movies WHERE IsActive=1 ORDER BY Title");
            case "comboRoom": return LoadCombo("SELECT RoomID AS Id, RoomName AS Display FROM dbo.Rooms WHERE IsActive=1 ORDER BY RoomName");
            case "multiline": return new TextBox { Multiline = true, Font = new Font("Segoe UI", 10F) };
            case "password": return new TextBox { UseSystemPasswordChar = true, PlaceholderText = _id.HasValue ? "Để trống nếu không đổi mật khẩu" : "Ít nhất 6 ký tự", Font = new Font("Segoe UI", 10F) };
            case "date": return new TextBox { PlaceholderText = "yyyy-MM-dd (để trống nếu không có)", Font = new Font("Segoe UI", 10F) };
            case "int": return new TextBox { Font = new Font("Segoe UI", 10F), PlaceholderText = "Nhập số nguyên" };
            case "decimal": return new TextBox { Font = new Font("Segoe UI", 10F), PlaceholderText = "Ví dụ: 95000" };
            default: return new TextBox { Font = new Font("Segoe UI", 10F) };
        }
    }

    private static ComboBox LoadCombo(string query)
    {
        ComboBox cb = new() { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F), DisplayMember = "Display", ValueMember = "Id" };
        cb.DataSource = DatabaseHelper.ExecuteQuery(query);
        return cb;
    }

    private void LoadRecord(int id)
    {
        try
        {
            DataTable data = DatabaseHelper.ExecuteQuery($"SELECT * FROM dbo.[{_definition.Table}] WHERE [{_definition.IdColumn}]=@Id",
                new SqlParameter("@Id", SqlDbType.Int) { Value = id });
            if (data.Rows.Count == 0) { MessageBox.Show("Không tìm thấy bản ghi."); Close(); return; }
            DataRow row = data.Rows[0];
            foreach (FieldSpec field in _definition.Fields)
            {
                if (field.Kind == "password") continue;
                Control input = _controls[field];
                object value = row[field.Column];
                if (input is CheckBox check) check.Checked = value != DBNull.Value && Convert.ToBoolean(value);
                else if (input is ComboBox combo && value != DBNull.Value) combo.SelectedValue = Convert.ToInt32(value);
                else if (input is DateTimePicker date && value != DBNull.Value) date.Value = Convert.ToDateTime(value);
                else if (input is TextBox text) text.Text = value == DBNull.Value ? "" : Convert.ToString(value, CultureInfo.CurrentCulture) ?? "";
            }
        }
        catch (Exception ex) { MessageBox.Show("Không tải được dữ liệu để chỉnh sửa.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); Close(); }
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        List<string> columns = new();
        List<string> parameterNames = new();
        List<SqlParameter> parameters = new();
        foreach (FieldSpec field in _definition.Fields)
        {
            Control control = _controls[field];
            object? value;
            try { value = ReadValue(field, control); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (field.Kind == "password" && _id.HasValue && value == null) continue;
            if (field.Required && (value == null || value == DBNull.Value || (value is string s && s.Trim().Length == 0)))
            {
                MessageBox.Show("Vui lòng nhập trường: " + field.Label, "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (field.Kind == "password" && value is string password)
            {
                if (password.Length < 6) { MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Mật khẩu không hợp lệ"); return; }
                value = DatabaseHelper.HashPassword(password);
            }
            columns.Add(field.Column);
            string paramName = "@p" + parameters.Count;
            parameterNames.Add(paramName);
            parameters.Add(CreateParameter(paramName, field, value));
        }

        try
        {
            int affected;
            if (_id.HasValue)
            {
                List<string> sets = columns.Select((col, i) => $"[{col}]={parameterNames[i]}").ToList();
                parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = _id.Value });
                affected = DatabaseHelper.ExecuteNonQuery($"UPDATE dbo.[{_definition.Table}] SET {string.Join(",", sets)} WHERE [{_definition.IdColumn}]=@Id", parameters.ToArray());
            }
            else
            {
                string columnSql = string.Join(",", columns.Select(c => $"[{c}]"));
                affected = DatabaseHelper.ExecuteNonQuery($"INSERT INTO dbo.[{_definition.Table}] ({columnSql}) VALUES ({string.Join(",", parameterNames)})", parameters.ToArray());
            }
            if (affected > 0) { DialogResult = DialogResult.OK; Close(); }
            else MessageBox.Show("Không có dữ liệu nào được lưu.", "Thông báo");
        }
        catch (Exception ex) { MessageBox.Show("Không lưu được dữ liệu. Hãy kiểm tra giá trị nhập và các ràng buộc.\n\n" + ex.Message, "Lỗi lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private object? ReadValue(FieldSpec field, Control control)
    {
        switch (field.Kind)
        {
            case "bool": return ((CheckBox)control).Checked;
            case "comboRole": return ((ComboBox)control).SelectedItem?.ToString() ?? "Customer";
            case "comboMovie": case "comboRoom":
                if (((ComboBox)control).SelectedValue == null) throw new InvalidOperationException("Không có dữ liệu để chọn. Hãy tạo phim/phòng chiếu trước.");
                return Convert.ToInt32(((ComboBox)control).SelectedValue);
            case "datetime": return ((DateTimePicker)control).Value;
            case "int":
            {
                string text = ((TextBox)control).Text.Trim();
                if (text.Length == 0 && !field.Required) return DBNull.Value;
                if (!int.TryParse(text, out int number)) throw new InvalidOperationException($"Trường {field.Label} phải là số nguyên.");
                return number;
            }
            case "decimal":
            {
                string text = ((TextBox)control).Text.Trim().Replace(",", "");
                if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal number))
                    throw new InvalidOperationException($"Trường {field.Label} phải là số. Ví dụ: 95000");
                return number;
            }
            case "date":
            {
                string text = ((TextBox)control).Text.Trim();
                if (text.Length == 0) return DBNull.Value;
                if (!DateTime.TryParseExact(text, new[] { "yyyy-MM-dd", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    throw new InvalidOperationException($"Ngày ở trường {field.Label} phải theo dạng yyyy-MM-dd hoặc dd/MM/yyyy.");
                return date.Date;
            }
            case "password":
            {
                string text = ((TextBox)control).Text;
                if (text.Length == 0 && field.Required && !_id.HasValue) throw new InvalidOperationException("Vui lòng nhập mật khẩu cho tài khoản mới.");
                return text.Length == 0 ? null : text;
            }
            default:
            {
                string text = ((TextBox)control).Text.Trim();
                if (text.Length == 0 && !field.Required) return DBNull.Value;
                return text;
            }
        }
    }

    private static SqlParameter CreateParameter(string name, FieldSpec field, object? value)
    {
        SqlParameter parameter = field.Kind switch
        {
            "bool" => new SqlParameter(name, SqlDbType.Bit),
            "int" or "comboMovie" or "comboRoom" => new SqlParameter(name, SqlDbType.Int),
            "decimal" => new SqlParameter(name, SqlDbType.Decimal) { Precision = 12, Scale = 2 },
            "datetime" => new SqlParameter(name, SqlDbType.DateTime2),
            "date" => new SqlParameter(name, SqlDbType.Date),
            "password" => new SqlParameter(name, SqlDbType.Char, 64),
            "multiline" => new SqlParameter(name, SqlDbType.NVarChar, -1),
            _ => new SqlParameter(name, SqlDbType.NVarChar, field.Kind == "text" ? 300 : 500)
        };
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}
