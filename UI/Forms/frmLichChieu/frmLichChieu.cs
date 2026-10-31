#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using cinema_project.BLL;
using cinema_project.DAL;

namespace cinema_project.UI.Forms.frmLichChieu
{
    public partial class frmLichChieu : Form
    {
        private readonly SuatChieuBLL _suatChieuBLL = new();
        private readonly PhimBLL _phimBLL = new();
        private readonly SupabaseService supabaseService = new();
        private List<Phim> _danhSachPhim = new();
        private List<SuatChieu> _danhSachHienThi = new();
        private bool _isBinding = false;
        private List<PhongChieu> _danhSachPhong = [];


        public async Task LoadDanhSachPhongAsync()
        {
            try
            {
                _danhSachPhong = await supabaseService.LayTatCaPhongChieuAsync();
            }
            catch
            {
                _danhSachPhong = new List<PhongChieu>();
            }

            if (_danhSachPhong == null || _danhSachPhong.Count == 0)
            {
                _danhSachPhong = new List<PhongChieu>
                {
                    new() { MaPhong = 1, TenPhong = "Phòng 01 (2D Standard)" },
                    new() { MaPhong = 2, TenPhong = "Phòng 02 (3D Dolby)" },
                    new() { MaPhong = 3, TenPhong = "Phòng 03 (IMAX Laser)" },
                    new() { MaPhong = 4, TenPhong = "Phòng 04 (VIP Gold Class)" },
                    new() { MaPhong = 5, TenPhong = "Phòng 05 (4DX Motion)" }
                };
            }
        }
        private readonly string[] _danhSachDinhDang =
        {
            "2D Phụ đề",
            "2D Lồng tiếng",
            "3D Phụ đề",
            "3D Lồng tiếng",
            "IMAX 2D",
            "4DX"
        };

        private readonly string[] _danhSachTrangThai =
        {
            "Sắp chiếu",
            "Đang chiếu",
            "Hoàn tất",
            "Đã hủy"
        };

        public frmLichChieu()
        {
            InitializeComponent();
            BậtChếĐộDoubleBuffered(dgvSuatChieu);
        }

        private static void BậtChếĐộDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        private async void FrmLichChieu_Load(object? sender, EventArgs e)
        {
            _isBinding = true;

            await LoadDanhSachPhongAsync();

            cmbLocPhong.Items.Clear();
            cmbLocPhong.Items.Add("Tất cả phòng");
            foreach (var p in _danhSachPhong)
            {
                cmbLocPhong.Items.Add(p.TenPhong);
            }
            if (cmbLocPhong.Items.Count > 0) cmbLocPhong.SelectedIndex = 0;

            cmbPhong.Items.Clear();
            foreach (var p in _danhSachPhong)
            {
                cmbPhong.Items.Add(p);
            }
            if (cmbPhong.Items.Count > 0) cmbPhong.SelectedIndex = 0;

            cmbDinhDang.Items.Clear();
            cmbDinhDang.Items.AddRange(_danhSachDinhDang);
            if (cmbDinhDang.Items.Count > 0) cmbDinhDang.SelectedIndex = 0;

            cmbTrangThai.Items.Clear();
            cmbTrangThai.Items.AddRange(_danhSachTrangThai);
            if (cmbTrangThai.Items.Count > 0) cmbTrangThai.SelectedIndex = 0;

            try
            {
                _danhSachPhim = await _phimBLL.DongBoVaLayPhimAsync();
            }
            catch
            {
                _danhSachPhim = _phimBLL.LayTatCaPhim();
            }

            if (_danhSachPhim.Count == 0)
            {
                _danhSachPhim = new List<Phim>
                {
                    new() { Ten = "Mai", ThoiLuong = "131 phút", GiaVe = 90000 },
                    new() { Ten = "Dune: Hành Tinh Cát 2", ThoiLuong = "166 phút", GiaVe = 150000 },
                    new() { Ten = "Kung Fu Panda 4", ThoiLuong = "94 phút", GiaVe = 90000 },
                    new() { Ten = "Lật Mặt 7: Một Điều Ước", ThoiLuong = "138 phút", GiaVe = 95000 },
                    new() { Ten = "Godzilla x Kong: Đế Chế Mới", ThoiLuong = "115 phút", GiaVe = 110000 }
                };
            }

            cmbLocPhim.Items.Clear();
            cmbLocPhim.Items.Add("Tất cả phim");
            foreach (var p in _danhSachPhim)
            {
                if (!string.IsNullOrWhiteSpace(p.Ten) && !cmbLocPhim.Items.Contains(p.Ten))
                {
                    cmbLocPhim.Items.Add(p.Ten);
                }
            }
            if (cmbLocPhim.Items.Count > 0) cmbLocPhim.SelectedIndex = 0;

            cmbPhim.Items.Clear();
            foreach (var p in _danhSachPhim)
            {
                if (!string.IsNullOrWhiteSpace(p.Ten) && !cmbPhim.Items.Contains(p.Ten))
                {
                    cmbPhim.Items.Add(p.Ten);
                }
            }
            if (cmbPhim.Items.Count > 0) cmbPhim.SelectedIndex = 0;

            dtpLocNgay.Value = DateTime.Today;
            chkTatCaNgay.Checked = true;

            _isBinding = false;

            TaiDanhSachSuatChieu();
            LamMoiForm();
        }

        private void TaiDanhSachSuatChieu()
        {
            if (_isBinding) return;

            DateTime? ngay = chkTatCaNgay.Checked ? null : dtpLocNgay.Value.Date;
            string? phim = cmbLocPhim.SelectedItem?.ToString();

            int? maPhong = null;
            if (cmbLocPhong.SelectedItem is string tenPhong && tenPhong != "Tất cả phòng")
            {
                var phong = _danhSachPhong.FirstOrDefault(p => p.TenPhong.Equals(tenPhong, StringComparison.OrdinalIgnoreCase));
                if (phong != null)
                {
                    maPhong = phong.MaPhong;
                }
            }

            string tuKhoa = txtTimKiem.Text.Trim();

            _danhSachHienThi = _suatChieuBLL.LocSuatChieu(ngay, phim, maPhong, null, tuKhoa);

            foreach (var sc in _danhSachHienThi)
            {
                if (string.IsNullOrWhiteSpace(sc.TenPhim) && sc.MaPhim > 0)
                {
                    var p = _danhSachPhim.FirstOrDefault(x => x.MaPhim == sc.MaPhim || x.TmdbId == sc.MaPhim);
                    if (p != null) sc.TenPhim = p.Ten;
                }

                if (string.IsNullOrWhiteSpace(sc.TenPhong) && sc.MaPhong > 0)
                {
                    var r = _danhSachPhong.FirstOrDefault(x => x.MaPhong == sc.MaPhong);
                    if (r != null) sc.TenPhong = r.TenPhong;
                }
            }

            dgvSuatChieu.DataSource = null;
            dgvSuatChieu.DataSource = _danhSachHienThi;

            lblSoLuongSuat.Text = $"({_danhSachHienThi.Count} suất chiếu)";

            if (_danhSachHienThi.Count > 0)
            {
                dgvSuatChieu.ClearSelection();
            }
        }

        private void LocSuatChieu_Changed(object? sender, EventArgs e)
        {
            TaiDanhSachSuatChieu();
        }

        private void BtnDatLaiBoLoc_Click(object? sender, EventArgs e)
        {
            _isBinding = true;
            chkTatCaNgay.Checked = true;
            dtpLocNgay.Value = DateTime.Today;
            if (cmbLocPhim.Items.Count > 0) cmbLocPhim.SelectedIndex = 0;
            if (cmbLocPhong.Items.Count > 0) cmbLocPhong.SelectedIndex = 0;
            txtTimKiem.Text = "";
            _isBinding = false;

            TaiDanhSachSuatChieu();
        }

        private void DgvSuatChieu_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvSuatChieu.CurrentRow?.DataBoundItem is SuatChieu sc)
            {
                HienThiChiTiet(sc);
            }
        }

        private void HienThiChiTiet(SuatChieu sc)
        {
            _isBinding = true;

            txtMaSC.Text = sc.MaSuatChieu.ToString();

            var phim = _danhSachPhim.FirstOrDefault(p => (!string.IsNullOrEmpty(sc.TenPhim) && p.Ten.Equals(sc.TenPhim, StringComparison.OrdinalIgnoreCase)) || (sc.MaPhim > 0 && (p.MaPhim == sc.MaPhim || p.TmdbId == sc.MaPhim)));
            if (phim != null && cmbPhim.Items.Contains(phim.Ten))
                cmbPhim.SelectedItem = phim.Ten;
            else if (cmbPhim.Items.Contains(sc.TenPhim))
                cmbPhim.SelectedItem = sc.TenPhim;
            else if (cmbPhim.Items.Count > 0)
                cmbPhim.SelectedIndex = 0;

            var phong = _danhSachPhong.FirstOrDefault(p => (sc.MaPhong > 0 && p.MaPhong == sc.MaPhong) || p.TenPhong.Equals(sc.TenPhong, StringComparison.OrdinalIgnoreCase));
            if (phong != null)
                cmbPhong.SelectedItem = phong;
            else if (cmbPhong.Items.Count > 0)
                cmbPhong.SelectedIndex = 0;

            dtpNgay.Value = sc.NgayChieu;
            dtpGioBD.Value = DateTime.Today.Add(sc.GioBatDau);
            dtpGioKT.Value = DateTime.Today.Add(sc.GioKetThuc);

            if (cmbDinhDang.Items.Contains(sc.DinhDang))
                cmbDinhDang.SelectedItem = sc.DinhDang;

            nudGiaVe.Value = Math.Clamp(sc.GiaVe, nudGiaVe.Minimum, nudGiaVe.Maximum);

            if (cmbTrangThai.Items.Contains(sc.TrangThai))
                cmbTrangThai.SelectedItem = sc.TrangThai;

            btnThem.Enabled = false;
            btnCapNhat.Enabled = true;
            btnXoa.Enabled = true;

            _isBinding = false;
        }

        private void LamMoiForm()
        {
            _isBinding = true;

            txtMaSC.Text = "(Tự động sinh mã)";
            if (cmbPhim.Items.Count > 0) cmbPhim.SelectedIndex = 0;
            if (cmbPhong.Items.Count > 0) cmbPhong.SelectedIndex = 0;
            dtpNgay.Value = DateTime.Today;

            var gioMacDinh = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 0, 0);
            dtpGioBD.Value = gioMacDinh;
            CapNhatGioKetThuc();

            if (cmbDinhDang.Items.Count > 0) cmbDinhDang.SelectedIndex = 0;
            nudGiaVe.Value = 90000;
            if (cmbTrangThai.Items.Count > 0) cmbTrangThai.SelectedIndex = 0;

            btnThem.Enabled = true;
            btnCapNhat.Enabled = false;
            btnXoa.Enabled = false;

            dgvSuatChieu.ClearSelection();

            _isBinding = false;
        }

        private void CapNhatGioKetThuc()
        {
            string? tenPhim = cmbPhim.SelectedItem?.ToString();
            var phim = _danhSachPhim.FirstOrDefault(p => p.Ten == tenPhim);
            string? thoiLuong = phim?.ThoiLuong;

            var gioBD = dtpGioBD.Value.TimeOfDay;
            var gioKT = _suatChieuBLL.TinhGioKetThuc(gioBD, thoiLuong);

            dtpGioKT.Value = DateTime.Today.Add(gioKT);
        }

        private void CmbPhim_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isBinding) return;

            string? tenPhim = cmbPhim.SelectedItem?.ToString();
            var phim = _danhSachPhim.FirstOrDefault(p => p.Ten == tenPhim);
            if (phim != null && phim.GiaVe > 0)
            {
                nudGiaVe.Value = Math.Clamp(phim.GiaVe, nudGiaVe.Minimum, nudGiaVe.Maximum);
            }

            CapNhatGioKetThuc();
        }

        private void DtpGioBD_ValueChanged(object? sender, EventArgs e)
        {
            if (_isBinding) return;
            CapNhatGioKetThuc();
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            var phongChon = cmbPhong.SelectedItem as PhongChieu;
            string tenPhimChon = cmbPhim.SelectedItem?.ToString() ?? cmbPhim.Text.Trim();
            var phim = _danhSachPhim.FirstOrDefault(p => p.Ten.Equals(tenPhimChon, StringComparison.OrdinalIgnoreCase));

            var sc = new SuatChieu
            {
                MaPhim = phim?.MaPhim ?? phim?.TmdbId ?? 0,
                MaPhong = phongChon?.MaPhong ?? 0,
                TenPhim = tenPhimChon,
                TenPhong = phongChon?.TenPhong ?? cmbPhong.Text,
                NgayChieu = dtpNgay.Value.Date,
                GioBatDau = dtpGioBD.Value.TimeOfDay,
                GioKetThuc = dtpGioKT.Value.TimeOfDay,
                DinhDang = cmbDinhDang.SelectedItem?.ToString() ?? "2D Phụ đề",
                GiaVe = nudGiaVe.Value,
                TrangThai = cmbTrangThai.SelectedItem?.ToString() ?? "Sắp chiếu"
            };

            if (!_suatChieuBLL.ThemSuatChieu(sc, out string loi))
            {
                MessageBox.Show(loi, "Cảnh báo lịch chiếu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Thêm suất chiếu mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TaiDanhSachSuatChieu();
            LamMoiForm();
        }

        private void BtnCapNhat_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaSC.Text, out int maSC))
            {
                MessageBox.Show("Vui lòng chọn suất chiếu cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var phongChon = cmbPhong.SelectedItem as PhongChieu;
            string tenPhimChon = cmbPhim.SelectedItem?.ToString() ?? cmbPhim.Text.Trim();
            var phim = _danhSachPhim.FirstOrDefault(p => p.Ten.Equals(tenPhimChon, StringComparison.OrdinalIgnoreCase));

            var sc = new SuatChieu
            {
                MaSuatChieu = maSC,
                MaPhim = phim?.MaPhim ?? phim?.TmdbId ?? 0,
                MaPhong = phongChon?.MaPhong ?? 0,
                TenPhim = tenPhimChon,
                TenPhong = phongChon?.TenPhong ?? cmbPhong.Text,
                NgayChieu = dtpNgay.Value.Date,
                GioBatDau = dtpGioBD.Value.TimeOfDay,
                GioKetThuc = dtpGioKT.Value.TimeOfDay,
                DinhDang = cmbDinhDang.SelectedItem?.ToString() ?? "2D Phụ đề",
                GiaVe = nudGiaVe.Value,
                TrangThai = cmbTrangThai.SelectedItem?.ToString() ?? "Sắp chiếu"
            };

            if (!_suatChieuBLL.CapNhatSuatChieu(sc, out string loi))
            {
                MessageBox.Show(loi, "Cảnh báo lịch chiếu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Cập nhật thông tin suất chiếu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TaiDanhSachSuatChieu();
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaSC.Text, out int maSC))
            {
                MessageBox.Show("Vui lòng chọn suất chiếu cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa suất chiếu #{maSC} ({cmbPhim.Text} - {cmbPhong.Text}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (xacNhan == DialogResult.Yes)
            {
                if (_suatChieuBLL.XoaSuatChieu(maSC))
                {
                    MessageBox.Show("Đã xóa suất chiếu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachSuatChieu();
                    LamMoiForm();
                }
                else
                {
                    MessageBox.Show("Không thể xóa suất chiếu này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            LamMoiForm();
        }

        private void DgvSuatChieu_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.CellStyle == null) return;

            string colName = dgvSuatChieu.Columns[e.ColumnIndex].DataPropertyName;
            if (colName == "TrangThai" && e.Value != null)
            {
                string tt = e.Value.ToString() ?? "";
                switch (tt)
                {
                    case "Đang chiếu":
                        e.CellStyle.ForeColor = Color.FromArgb(22, 163, 74);
                        e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                        break;
                    case "Sắp chiếu":
                        e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);
                        e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                        break;
                    case "Hoàn tất":
                        e.CellStyle.ForeColor = Color.FromArgb(107, 107, 118);
                        break;
                    case "Đã hủy":
                        e.CellStyle.ForeColor = Color.FromArgb(220, 38, 38);
                        e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                        break;
                }
            }
            else if (colName == "KhungGio")
            {
                e.CellStyle.ForeColor = Color.FromArgb(231, 26, 15);
            }
        }
    }
}
