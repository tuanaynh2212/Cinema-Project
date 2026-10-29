using System.Drawing;

namespace CinemaManagement;

public partial class Login : Form
{
    private readonly TextBox _txtUsername = new();
    private readonly TextBox _txtPassword = new();
    private readonly Label _lblStatus = new();

    public Login()
    {
        BuildLayout();
    }

    private void BuildLayout()
    {
        Text = "CinemaManagement | Đăng nhập";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1000, 650);
        MinimumSize = new Size(900, 600);
        BackColor = Ui.Background;
        Font = new Font("Segoe UI", 10F);

        Panel brandPanel = new() { Dock = DockStyle.Left, Width = 390, BackColor = Ui.Navy, Padding = new Padding(36) };
        Label logo = new() { Text = "CINEMA\nMANAGEMENT", AutoSize = true, ForeColor = Color.White, Font = new Font("Segoe UI", 27, FontStyle.Bold), Location = new Point(32, 105) };
        Label tagline = new() { Text = "Quản lý rạp chiếu phim\nđơn giản – rõ ràng – hiệu quả", AutoSize = true, ForeColor = Color.FromArgb(205, 211, 230), Font = new Font("Segoe UI", 13F), Location = new Point(36, 220) };
        Label brandFoot = new() { Text = "HỆ THỐNG QUẢN LÝ RẠP CHIẾU PHIM", AutoSize = true, ForeColor = Color.FromArgb(160, 169, 194), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), Location = new Point(36, 555) };
        brandPanel.Controls.AddRange(new Control[] { logo, tagline, brandFoot });

        Panel rightPanel = new() { Dock = DockStyle.Fill, BackColor = Ui.Background, Padding = new Padding(54) };
        Label title = Ui.MakeLabel("Chào mừng trở lại!", 24, true);
        title.Location = new Point(48, 85);
        Label sub = Ui.MakeLabel("Đăng nhập để vào không gian làm việc của bạn", 10, false);
        sub.ForeColor = Ui.Muted; sub.Location = new Point(51, 132);

        Label userLabel = Ui.MakeLabel("TÊN ĐĂNG NHẬP", 9, true); userLabel.Location = new Point(50, 195);
        _txtUsername.Location = new Point(50, 222); _txtUsername.Width = 380; _txtUsername.Height = 38;
        _txtUsername.Font = new Font("Segoe UI", 11F); _txtUsername.PlaceholderText = "Nhập tên đăng nhập";
        Ui.StyleTextBox(_txtUsername);

        Label passLabel = Ui.MakeLabel("MẬT KHẨU", 9, true); passLabel.Location = new Point(50, 280);
        _txtPassword.Location = new Point(50, 307); _txtPassword.Width = 380; _txtPassword.Height = 38;
        _txtPassword.Font = new Font("Segoe UI", 11F); _txtPassword.PlaceholderText = "Nhập mật khẩu"; _txtPassword.UseSystemPasswordChar = true;
        Ui.StyleTextBox(_txtPassword);

        Button login = Ui.MakeButton("ĐĂNG NHẬP", Ui.Accent); login.Location = new Point(50, 375); login.Size = new Size(380, 46);
        login.Click += Login_Click;
        Button register = Ui.MakeButton("TẠO TÀI KHOẢN KHÁCH HÀNG", Color.FromArgb(226, 229, 239), Ui.Text); register.Location = new Point(50, 433); register.Size = new Size(380, 42);
        register.Click += (s, e) => { using RegisterForm form = new(); form.ShowDialog(this); };

        _lblStatus.Text = ""; _lblStatus.AutoSize = false; _lblStatus.Size = new Size(390, 52); _lblStatus.Location = new Point(50, 490); _lblStatus.ForeColor = Color.Firebrick;
        rightPanel.Controls.AddRange(new Control[] { title, sub, userLabel, _txtUsername, passLabel, _txtPassword, login, register, _lblStatus });
        Controls.Add(rightPanel); Controls.Add(brandPanel);
        AcceptButton = login;
    }

    private void Login_Click(object? sender, EventArgs e)
    {
        string username = _txtUsername.Text.Trim();
        string password = _txtPassword.Text;
        if (username.Length == 0 || password.Length == 0)
        {
            _lblStatus.Text = "Vui lòng nhập tên đăng nhập và mật khẩu.";
            return;
        }

        try
        {
            UserSession? user = DatabaseHelper.Authenticate(username, password);
            if (user == null)
            {
                _lblStatus.Text = "Thông tin không chính xác hoặc tài khoản đã bị khóa.";
                return;
            }

            Form dashboard = user.Role switch
            {
                "Admin" => new AdminMainForm(user),
                "Manager" => new ManagerMainForm(user),
                _ => new CustomerMainForm(user)
            };
            Hide();
            using (dashboard) dashboard.ShowDialog(this);
            _txtPassword.Clear();
            _lblStatus.Text = "";
            Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không đăng nhập được. Hãy kiểm tra cấu hình SQL Server.\n\n" + ex.Message,
                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
