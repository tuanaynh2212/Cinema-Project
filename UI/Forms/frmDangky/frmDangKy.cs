using System;
using System.Net.Mail;
using System.Windows.Forms;
using cinema_project.DAL;

namespace cinema_project.UI.Forms.frmAuth
{
    public partial class frmDangKy : Form
    {
        private readonly AuthDAL _authDAL;

        public string EmailDangKy { get; private set; } = "";

        public frmDangKy(AuthDAL authDAL)
        {
            _authDAL = authDAL ?? throw new ArgumentNullException(nameof(authDAL));
            InitializeComponent();
            AcceptButton = btnDangKy;
        }

        private async void btnDangKy_Click(object? sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string email = txtEmail.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string xacNhan = txtXacNhanMatKhau.Text;

            if (string.IsNullOrWhiteSpace(hoTen) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(matKhau) ||
                string.IsNullOrWhiteSpace(xacNhan))
            {
                HienThongBao("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            try { _ = new MailAddress(email); }
            catch
            {
                HienThongBao("Email không đúng định dạng.");
                return;
            }

            if (matKhau.Length < 8)
            {
                HienThongBao("Mật khẩu phải có ít nhất 8 ký tự.");
                return;
            }

            if (matKhau != xacNhan)
            {
                HienThongBao("Mật khẩu xác nhận không khớp.");
                return;
            }

            btnDangKy.Enabled = false;
            btnHuy.Enabled = false;
            lblThongBao.Text = "Đang tạo tài khoản...";

            try
            {
                await _authDAL.DangKyAsync(hoTen, email, matKhau);
                EmailDangKy = email;
                MessageBox.Show(
                    "Gửi yêu cầu đăng ký thành công.\n\nNếu Supabase bật xác minh email, hãy kiểm tra hộp thư và xác minh trước khi đăng nhập.",
                    "CGV Cinemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                HienThongBao(ex.Message);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnDangKy.Enabled = true;
                    btnHuy.Enabled = true;
                }
            }
        }

        private void chkHienMatKhau_CheckedChanged(object? sender, EventArgs e)
        {
            bool hien = chkHienMatKhau.Checked;
            txtMatKhau.UseSystemPasswordChar = !hien;
            txtXacNhanMatKhau.UseSystemPasswordChar = !hien;
        }

        private void HienThongBao(string noiDung)
        {
            lblThongBao.Text = noiDung;
        }
    }
}
