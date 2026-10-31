using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmAuth
{
    partial class frmDangKy
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlHeader;
        private Label lblLogo;
        private Label lblTieuDe;
        private Label lblMoTa;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblMatKhau;
        private TextBox txtMatKhau;
        private Label lblXacNhan;
        private TextBox txtXacNhanMatKhau;
        private CheckBox chkHienMatKhau;
        private Button btnDangKy;
        private Button btnHuy;
        private Label lblThongBao;

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
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblMatKhau = new Label();
            txtMatKhau = new TextBox();
            lblXacNhan = new Label();
            txtXacNhanMatKhau = new TextBox();
            chkHienMatKhau = new CheckBox();
            btnDangKy = new Button();
            btnHuy = new Button();
            lblThongBao = new Label();
            pnlHeader.SuspendLayout();
            SuspendLayout();

            pnlHeader.BackColor = Color.FromArgb(26, 26, 34);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 115;
            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Controls.Add(lblMoTa);

            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(231, 26, 15);
            lblLogo.Location = new Point(30, 12);
            lblLogo.Text = "CGV CINEMAS";

            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(30, 42);
            lblTieuDe.Text = "Tạo tài khoản";

            lblMoTa.AutoSize = true;
            lblMoTa.Font = new Font("Segoe UI", 9.5F);
            lblMoTa.ForeColor = Color.FromArgb(190, 190, 202);
            lblMoTa.Location = new Point(34, 83);
            lblMoTa.Text = "Đăng ký tài khoản CGV bằng email.";

            lblHoTen.AutoSize = true;
            lblHoTen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblHoTen.Location = new Point(36, 133);
            lblHoTen.Text = "Họ và tên";

            txtHoTen.Font = new Font("Segoe UI", 10.5F);
            txtHoTen.Location = new Point(36, 158);
            txtHoTen.Size = new Size(440, 31);
            txtHoTen.BorderStyle = BorderStyle.FixedSingle;
            txtHoTen.PlaceholderText = "Nhập họ và tên";

            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEmail.Location = new Point(36, 201);
            lblEmail.Text = "Email";

            txtEmail.Font = new Font("Segoe UI", 10.5F);
            txtEmail.Location = new Point(36, 226);
            txtEmail.Size = new Size(440, 31);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.PlaceholderText = "name@example.com";

            lblMatKhau.AutoSize = true;
            lblMatKhau.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMatKhau.Location = new Point(36, 269);
            lblMatKhau.Text = "Mật khẩu (ít nhất 8 ký tự)";

            txtMatKhau.Font = new Font("Segoe UI", 10.5F);
            txtMatKhau.Location = new Point(36, 294);
            txtMatKhau.Size = new Size(440, 31);
            txtMatKhau.BorderStyle = BorderStyle.FixedSingle;
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhau.PlaceholderText = "Tạo mật khẩu";

            lblXacNhan.AutoSize = true;
            lblXacNhan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblXacNhan.Location = new Point(36, 337);
            lblXacNhan.Text = "Xác nhận mật khẩu";

            txtXacNhanMatKhau.Font = new Font("Segoe UI", 10.5F);
            txtXacNhanMatKhau.Location = new Point(36, 362);
            txtXacNhanMatKhau.Size = new Size(440, 31);
            txtXacNhanMatKhau.BorderStyle = BorderStyle.FixedSingle;
            txtXacNhanMatKhau.UseSystemPasswordChar = true;
            txtXacNhanMatKhau.PlaceholderText = "Nhập lại mật khẩu";

            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.Location = new Point(36, 402);
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;

            btnDangKy.BackColor = Color.FromArgb(231, 26, 15);
            btnDangKy.FlatStyle = FlatStyle.Flat;
            btnDangKy.FlatAppearance.BorderSize = 0;
            btnDangKy.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDangKy.ForeColor = Color.White;
            btnDangKy.Location = new Point(36, 439);
            btnDangKy.Size = new Size(440, 44);
            btnDangKy.Text = "TẠO TÀI KHOẢN";
            btnDangKy.UseVisualStyleBackColor = false;
            btnDangKy.Click += btnDangKy_Click;
            btnDangKy.Cursor = Cursors.Hand;

            lblThongBao.AutoSize = false;
            lblThongBao.ForeColor = Color.FromArgb(190, 30, 30);
            lblThongBao.Font = new Font("Segoe UI", 9F);
            lblThongBao.Location = new Point(36, 489);
            lblThongBao.Size = new Size(440, 50);
            lblThongBao.Text = "";
            lblThongBao.TextAlign = ContentAlignment.MiddleLeft;

            btnHuy.BackColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.FlatAppearance.BorderColor = Color.FromArgb(231, 26, 15);
            btnHuy.ForeColor = Color.FromArgb(231, 26, 15);
            btnHuy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnHuy.Location = new Point(36, 544);
            btnHuy.Size = new Size(440, 38);
            btnHuy.Text = "Hủy";
            btnHuy.Click += (s, e) => Close();

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 248, 250);
            ClientSize = new Size(512, 605);
            Controls.Add(btnHuy);
            Controls.Add(lblThongBao);
            Controls.Add(btnDangKy);
            Controls.Add(chkHienMatKhau);
            Controls.Add(txtXacNhanMatKhau);
            Controls.Add(lblXacNhan);
            Controls.Add(txtMatKhau);
            Controls.Add(lblMatKhau);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtHoTen);
            Controls.Add(lblHoTen);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDangKy";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đăng ký - CGV Cinemas";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
