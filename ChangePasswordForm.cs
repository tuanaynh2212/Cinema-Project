using System.Drawing;

namespace CinemaManagement;

public sealed class ChangePasswordForm : Form
{
    private readonly UserSession _user;
    private readonly TextBox _current = new();
    private readonly TextBox _new = new();
    private readonly TextBox _confirm = new();

    public ChangePasswordForm(UserSession user)
    {
        _user = user;
        Text = "Đổi mật khẩu";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(500, 470);
        BackColor = Ui.Background;
        Panel card = new() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(32) };
        Label title = Ui.MakeLabel("Đổi mật khẩu", 20, true); title.Location = new Point(32, 25);
        AddField(card, "MẬT KHẨU HIỆN TẠI", _current, 82);
        AddField(card, "MẬT KHẨU MỚI", _new, 160);
        AddField(card, "NHẬP LẠI MẬT KHẨU MỚI", _confirm, 238);
        Button save = Ui.MakeButton("CẬP NHẬT MẬT KHẨU", Ui.Accent); save.Location = new Point(32, 320); save.Size = new Size(390, 42); save.Click += Save_Click;
        card.Controls.Add(title); card.Controls.Add(save); Controls.Add(card); AcceptButton = save;
    }

    private static void AddField(Control parent, string labelText, TextBox box, int top)
    {
        Label label = Ui.MakeLabel(labelText, 8.5F, true); label.Location = new Point(32, top);
        box.Location = new Point(32, top + 24); box.Size = new Size(390, 32); box.UseSystemPasswordChar = true; box.Font = new Font("Segoe UI", 10F); Ui.StyleTextBox(box);
        parent.Controls.Add(label); parent.Controls.Add(box);
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (_current.Text.Length == 0 || _new.Text.Length < 6)
        {
            MessageBox.Show("Nhập đúng mật khẩu hiện tại và mật khẩu mới tối thiểu 6 ký tự.", "Kiểm tra thông tin"); return;
        }
        if (_new.Text != _confirm.Text) { MessageBox.Show("Mật khẩu mới nhập lại không khớp.", "Kiểm tra thông tin"); return; }
        try
        {
            if (!DatabaseHelper.ChangePassword(_user.UserId, _current.Text, _new.Text))
            { MessageBox.Show("Mật khẩu hiện tại chưa đúng hoặc tài khoản đã bị khóa.", "Không thể đổi mật khẩu"); return; }
            MessageBox.Show("Đổi mật khẩu thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) { MessageBox.Show("Không đổi được mật khẩu.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}
