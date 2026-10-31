using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmAuth
{
    partial class frmDangNhap
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlHeader;
        private Label lblLogo;
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblMatKhau;
        private TextBox txtMatKhau;
        private CheckBox chkHienMatKhau;
        private Button btnDangNhap;
        private Button btnMoDangKy;
        private Label lblThongBao;
        private Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblLogo = new Label();
            lblTieuDe = new Label();
            lblMoTa = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblMatKhau = new Label();
            txtMatKhau = new TextBox();
            chkHienMatKhau = new CheckBox();
            btnDangNhap = new Button();
            btnMoDangKy = new Button();
            lblThongBao = new Label();
            btnDong = new Button();
            pnlHeader.SuspendLayout();
            SuspendLayout();

            pnlHeader.BackColor = Color.FromArgb(26, 26, 34);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 125;
            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Controls.Add(lblMoTa);

            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(231, 26, 15);
            lblLogo.Location = new Point(30, 17);
            lblLogo.Text = "CGV CINEMAS";

            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(30, 47);
            lblTieuDe.Text = "Chào mừng trở lại";

            lblMoTa.AutoSize = true;
            lblMoTa.Font = new Font("Segoe UI", 9.5F);
            lblMoTa.ForeColor = Color.FromArgb(190, 190, 202);
            lblMoTa.Location = new Point(34, 89);
            lblMoTa.Text = "Đăng nhập để tiếp tục sử dụng dịch vụ.";

            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEmail.Location = new Point(36, 158);
            lblEmail.Text = "Email";

            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.Location = new Point(36, 184);
            txtEmail.Size = new Size(440, 32);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.PlaceholderText = "name@example.com";

            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMatKhau.Location = new Point(36, 237);
            lblMatKhau.Text = "Mật khẩu";

            txtMatKhau.Font = new Font("Segoe UI", 11F);
            txtMatKhau.Location = new Point(36, 263);
            txtMatKhau.Size = new Size(440, 32);
            txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhau.PlaceholderText = "Nhập mật khẩu";

            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.Font = new Font("Segoe UI", 9F);
            chkHienMatKhau.Location = new Point(36, 305);
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.CheckedChanged += ChkHienMatKhau_CheckedChanged;

            btnDangNhap.BackColor = Color.FromArgb(231, 26, 15);
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(36, 352);
            btnDangNhap.Size = new Size(440, 45);
            btnDangNhap.Text = "ĐĂNG NHẬP";
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.Click += BtnDangNhap_Click;
            btnDangNhap.Cursor = Cursors.Hand;

            lblThongBao.AutoSize = false;
            lblThongBao.ForeColor = Color.FromArgb(190, 30, 30);
            lblThongBao.Font = new Font("Segoe UI", 9F);
            lblThongBao.Location = new Point(36, 407);
            lblThongBao.Size = new Size(440, 50);
            lblThongBao.Text = "";
            lblThongBao.TextAlign = ContentAlignment.MiddleLeft;

            btnMoDangKy.BackColor = Color.White;
            btnMoDangKy.FlatStyle = FlatStyle.Flat;
            btnMoDangKy.FlatAppearance.BorderColor = Color.FromArgb(231, 26, 15);
            btnMoDangKy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMoDangKy.ForeColor = Color.FromArgb(231, 26, 15);
            btnMoDangKy.Location = new Point(36, 468);
            btnMoDangKy.Size = new Size(440, 40);
            btnMoDangKy.Text = "Chưa có tài khoản? Đăng ký";
            btnMoDangKy.Click += BtnMoDangKy_Click;
            btnMoDangKy.Cursor = Cursors.Hand;

            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.BackColor = Color.FromArgb(248, 248, 250);
            btnDong.ForeColor = Color.FromArgb(90, 90, 100);
            btnDong.Location = new Point(36, 520);
            btnDong.Size = new Size(440, 32);
            btnDong.Text = "Đóng";
            btnDong.Click += (s, e) => Close();

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 248, 250);
            ClientSize = new Size(512, 570);
            Controls.Add(btnDong);
            Controls.Add(btnMoDangKy);
            Controls.Add(lblThongBao);
            Controls.Add(btnDangNhap);
            Controls.Add(chkHienMatKhau);
            Controls.Add(txtMatKhau);
            Controls.Add(lblMatKhau);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDangNhap";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đăng nhập - CGV Cinemas";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
