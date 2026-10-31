using System.Drawing;
using System.Windows.Forms;
using cinema_project.DAL;
using cinema_project.BLL;
using FormVeCuaBan = cinema_project.UI.Forms.frmVeCuaBan.frmVeCuaBan;

namespace cinema_project.UI.Forms.frmTaiKhoan
{
    public partial class frmTaiKhoan : Form
    {
        private readonly AuthBLL _authBLL;

        public frmTaiKhoan(AuthBLL? authBLL = null)
        {
            _authBLL = authBLL ?? new AuthBLL(new SupabaseService());
            InitializeComponent();
            string email = PhienDangNhap.NguoiDung?.Email ?? "minhanh@example.com";
            _lblEmailTaiKhoan.Text = email;
            _txtEmail.Text = email;
            _btnMoDanhSachVe.Click += (s, e) => new FormVeCuaBan().ShowDialog(this);
            _btnDangXuat.Click += BtnDangXuat_Click;
            _btnDoiMatKhau.Click += BtnDoiMatKhau_Click;
            ChuyenTrang(0);
        }

        private async void BtnDoiMatKhau_Click(object? sender, System.EventArgs e)
        {
            string matKhauHienTai = _txtMatKhauHienTai.Text;
            string matKhauMoi = _txtMatKhauMoi.Text;
            string xacNhanMatKhau = _txtXacNhanMatKhauMoi.Text;

            if (!PhienDangNhap.DaDangNhap)
            {
                MessageBox.Show(this, "Bạn cần đăng nhập để đổi mật khẩu.",
                    "Chưa đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(matKhauHienTai) ||
                string.IsNullOrWhiteSpace(matKhauMoi) ||
                string.IsNullOrWhiteSpace(xacNhanMatKhau))
            {
                MessageBox.Show(this, "Vui lòng nhập đầy đủ ba ô mật khẩu.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi.Length < 6)
            {
                MessageBox.Show(this, "Mật khẩu mới phải có ít nhất 6 ký tự.",
                    "Mật khẩu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtMatKhauMoi.Focus();
                return;
            }

            if (matKhauMoi != xacNhanMatKhau)
            {
                MessageBox.Show(this, "Mật khẩu mới và phần xác nhận không khớp.",
                    "Không khớp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtXacNhanMatKhauMoi.Focus();
                return;
            }

            if (matKhauHienTai == matKhauMoi)
            {
                MessageBox.Show(this, "Mật khẩu mới phải khác mật khẩu hiện tại.",
                    "Mật khẩu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtMatKhauMoi.Focus();
                return;
            }

            _btnDoiMatKhau.Enabled = false;
            _btnDoiMatKhau.Text = "Đang cập nhật...";
            Cursor = Cursors.WaitCursor;

            try
            {
                await _authBLL.DoiMatKhauAsync(matKhauHienTai, matKhauMoi);

                _txtMatKhauHienTai.Clear();
                _txtMatKhauMoi.Clear();
                _txtXacNhanMatKhauMoi.Clear();

                MessageBox.Show(this, "Đổi mật khẩu thành công.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message,
                    "Không thể đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    _btnDoiMatKhau.Enabled = true;
                    _btnDoiMatKhau.Text = "Cập nhật mật khẩu";
                    Cursor = Cursors.Default;
                }
            }
        }

        private void BtnDangXuat_Click(object? sender, System.EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                this,
                "Bạn có chắc muốn đăng xuất khỏi tài khoản này không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            PhienDangNhap.DangXuat();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ChuyenTrang(int index)
        {
            for (int i = 0; i < _cacTrang.Length; i++)
            {
                bool dangChon = i == index;
                _cacTrang[i].Visible = dangChon;
                _nutDieuHuong[i].BackColor = dangChon ? Color.FromArgb(255, 242, 242) : Color.White;
                _nutDieuHuong[i].ForeColor = dangChon ? MauDo : MauPhu;
                _nutDieuHuong[i].FlatAppearance.BorderColor = dangChon ? MauDo : Color.White;
            }
            _cacTrang[index].BringToFront();
        }
    }
}