using System.Drawing;
using System.Windows.Forms;
using cinema_project.BLL;
using cinema_project.DAL;
using FormVeCuaBan = cinema_project.UI.Forms.frmVeCuaBan.frmVeCuaBan;

namespace cinema_project.UI.Forms.frmTaiKhoan
{
    public partial class frmTaiKhoan : Form
    {
        private readonly AuthBLL _authBLL;

        public frmTaiKhoan()
        {
            _authBLL = new AuthBLL(new SupabaseService());
            InitializeComponent();
            var nguoiDung = PhienDangNhap.NguoiDung;
            string email = nguoiDung?.Email ?? "";
            _lblEmailTaiKhoan.Text = email;
            _txtEmail.Text = email;
            _txtHoTen.Text = nguoiDung?.HoTen ?? "";
            _txtSoDienThoai.Text = nguoiDung?.SoDienThoai ?? "";
            if (nguoiDung?.NgaySinh is DateTime ngaySinh)
            {
                _ngaySinh.Value = ngaySinh;
                _ngaySinh.Checked = true;
            }
            _lblHoTenTaiKhoan.Text = string.IsNullOrWhiteSpace(nguoiDung?.HoTen)
                ? "Tài khoản"
                : nguoiDung.HoTen;
            _btnMoDanhSachVe.Click += (s, e) => new FormVeCuaBan().ShowDialog(this);
            _btnDangXuat.Click += BtnDangXuat_Click;
            _btnLuuThongTin.Click += BtnLuuThongTin_Click;
            ChuyenTrang(0);
        }

        private async void BtnLuuThongTin_Click(object? sender, System.EventArgs e)
        {
            var nguoiDung = PhienDangNhap.NguoiDung;
            if (nguoiDung == null || string.IsNullOrWhiteSpace(nguoiDung.AccessToken))
            {
                MessageBox.Show(
                    this,
                    "Vui lòng đăng nhập để cập nhật thông tin tài khoản.",
                    "Không thể cập nhật hồ sơ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string hoTen = _txtHoTen.Text.Trim();
            string soDienThoai = _txtSoDienThoai.Text.Trim();
            DateTime? ngaySinh = _ngaySinh.Checked ? _ngaySinh.Value.Date : null;

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show(
                    this,
                    "Vui lòng nhập họ và tên.",
                    "Thông tin chưa hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _txtHoTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                MessageBox.Show(
                    this,
                    "Vui lòng nhập số điện thoại.",
                    "Thông tin chưa hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _txtSoDienThoai.Focus();
                return;
            }

            _btnLuuThongTin.Enabled = false;
            try
            {
                await _authBLL.UpdateProfileAsync(
                    nguoiDung.AccessToken,
                    hoTen,
                    soDienThoai,
                    ngaySinh);
                nguoiDung.HoTen = hoTen;
                nguoiDung.SoDienThoai = soDienThoai;
                nguoiDung.NgaySinh = ngaySinh;
                _lblHoTenTaiKhoan.Text = hoTen;
                MessageBox.Show(
                    this,
                    "Thông tin tài khoản đã được cập nhật.",
                    "Cập nhật thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "Không thể cập nhật hồ sơ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                    _btnLuuThongTin.Enabled = true;
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

        private async void BtnCapNhatMatKhau_Click(object? sender, System.EventArgs e)
        {
            string? email = PhienDangNhap.NguoiDung?.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(
                    this,
                    "Vui lòng đăng nhập để đổi mật khẩu.",
                    "Không thể đổi mật khẩu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string matKhauHienTai = _txtMatKhauHienTai.Text;
            string matKhauMoi = _txtMatKhauMoi.Text;
            string xacNhanMatKhauMoi = _txtXacNhanMatKhauMoi.Text;

            if (string.IsNullOrEmpty(matKhauMoi))
            {
                MessageBox.Show(
                    this,
                    "Vui lòng nhập mật khẩu mới.",
                    "Thông tin chưa hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _txtMatKhauMoi.Focus();
                return;
            }

            if (!string.Equals(matKhauMoi, xacNhanMatKhauMoi, StringComparison.Ordinal))
            {
                MessageBox.Show(
                    this,
                    "Mật khẩu xác nhận không khớp.",
                    "Thông tin chưa hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                _txtXacNhanMatKhauMoi.Focus();
                return;
            }

            _btnCapNhatMatKhau.Enabled = false;
            try
            {
                await _authBLL.ChangePasswordAsync(email, matKhauHienTai, matKhauMoi);
                MessageBox.Show(
                    this,
                    "Mật khẩu đã được cập nhật thành công.",
                    "Đổi mật khẩu thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                _txtMatKhauHienTai.Clear();
                _txtMatKhauMoi.Clear();
                _txtXacNhanMatKhauMoi.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "Không thể đổi mật khẩu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                    _btnCapNhatMatKhau.Enabled = true;
            }
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