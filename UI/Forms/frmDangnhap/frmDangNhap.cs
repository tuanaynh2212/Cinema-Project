using System;
using System.Windows.Forms;
using cinema_project.DAL;
using cinema_project.DAL.DTO;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.UI.Forms.frmAuth
{
    public partial class frmDangNhap : Form
    {
        private readonly AuthDAL _authDAL;
        private readonly ILogger<frmDangNhap> _logger;



        public string EmailDangNhap { get; private set; } = "";
        public AuthLoginResult? KetQuaDangNhap { get; private set; }

        public frmDangNhap(AuthDAL? authDAL = null)
        {
            _logger = AppLogging.CreateLogger<frmDangNhap>();
            _authDAL = authDAL ?? new AuthDAL(
                supabaseServices: new SupabaseService(),
                logger: AppLogging.CreateLogger<AuthDAL>());
            InitializeComponent();
            AcceptButton = btnDangNhap;
        }

        private async void BtnDangNhap_Click(object? sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(matKhau))
            {
                HienThongBao("Vui lòng nhập email và mật khẩu.");
                return;
            }

            btnDangNhap.Enabled = false;
            btnMoDangKy.Enabled = false;
            lblThongBao.Text = "Đang đăng nhập...";

            try
            {
                KetQuaDangNhap = await _authDAL.DangNhapAsync(email, matKhau);
                EmailDangNhap = KetQuaDangNhap.Email;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                HienThongBao(ex.Message);
                _logger.LogError(ex, "Đăng nhập thất bại cho {Email}", email);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnDangNhap.Enabled = true;
                    btnMoDangKy.Enabled = true;
                }
            }
        }

        private void BtnMoDangKy_Click(object? sender, EventArgs e)
        {
            using var form = new frmDangKy(authDAL: _authDAL);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                txtEmail.Text = form.EmailDangKy;
                txtMatKhau.Clear();
                HienThongBao(
                    "Đăng ký đã gửi thành công. Nếu Supabase yêu cầu xác minh email, hãy kiểm tra hộp thư trước khi đăng nhập.");
            }
        }

        private void ChkHienMatKhau_CheckedChanged(object? sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void HienThongBao(string noiDung)
        {
            lblThongBao.Text = noiDung;
        }
    }
}
