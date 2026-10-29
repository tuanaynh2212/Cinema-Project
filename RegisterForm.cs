using System.Drawing;

namespace CinemaManagement;

public partial class RegisterForm : Form
{
    private readonly TextBox _txtName = new();
    private readonly TextBox _txtUsername = new();
    private readonly TextBox _txtPassword = new();
    private readonly TextBox _txtConfirm = new();

    public RegisterForm()
    {
        Text = "CinemaManagement | Đăng ký";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(520, 650);
        MinimumSize = new Size(500, 620);
        BackColor = Ui.Background;
        Font = new Font("Segoe UI", 10F);

        Panel card = new() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(40) };
        Label title = Ui.MakeLabel("Tạo tài khoản khách hàng", 21, true);
        title.Location = new Point(35, 28);
        Label subtitle = Ui.MakeLabel("Nhập thông tin bên dưới để đăng ký", 10, false);
        subtitle.ForeColor = Ui.Muted; subtitle.Location = new Point(38, 68);

        BuildField(card, "HỌ VÀ TÊN", _txtName, "Ví dụ: Nguyễn Văn An", 115, false);
        BuildField(card, "TÊN ĐĂNG NHẬP", _txtUsername, "Tên đăng nhập duy nhất", 205, false);
        BuildField(card, "MẬT KHẨU", _txtPassword, "Ít nhất 6 ký tự", 295, true);
        BuildField(card, "NHẬP LẠI MẬT KHẨU", _txtConfirm, "Nhập lại mật khẩu", 385, true);

        Button create = Ui.MakeButton("ĐĂNG KÝ", Ui.Accent); create.Location = new Point(38, 475); create.Size = new Size(390, 44); create.Click += Create_Click;
        Label note = Ui.MakeLabel("Tài khoản đăng ký mới mặc định là Khách hàng.", 9, false);
        note.ForeColor = Ui.Muted; note.Location = new Point(38, 530);
        card.Controls.AddRange(new Control[] { title, subtitle, create, note });
        Controls.Add(card);
        AcceptButton = create;
    }

    private static void BuildField(Control parent, string caption, TextBox box, string placeholder, int top, bool password)
    {
        Label label = Ui.MakeLabel(caption, 9, true); label.Location = new Point(38, top);
        box.Location = new Point(38, top + 27); box.Size = new Size(390, 36); box.Font = new Font("Segoe UI", 10.5F);
        box.PlaceholderText = placeholder; box.UseSystemPasswordChar = password; Ui.StyleTextBox(box);
        parent.Controls.Add(label); parent.Controls.Add(box);
    }

    private void Create_Click(object? sender, EventArgs e)
    {
        string fullName = _txtName.Text.Trim();
        string username = _txtUsername.Text.Trim();
        string password = _txtPassword.Text;
        if (fullName.Length == 0 || username.Length < 3 || password.Length < 6)
        {
            MessageBox.Show("Vui lòng nhập họ tên, tên đăng nhập từ 3 ký tự và mật khẩu từ 6 ký tự.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (password != _txtConfirm.Text)
        {
            MessageBox.Show("Mật khẩu nhập lại không khớp.", "Kiểm tra mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            DatabaseHelper.Register(username, fullName, password);
            MessageBox.Show("Đăng ký thành công. Bạn có thể đăng nhập ngay.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number is 2601 or 2627)
        {
            MessageBox.Show("Tên đăng nhập này đã tồn tại. Hãy chọn tên khác.", "Trùng tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Đăng ký không thành công. Hãy kiểm tra kết nối CSDL.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
