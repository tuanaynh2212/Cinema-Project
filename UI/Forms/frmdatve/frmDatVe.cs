#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using cinema_project.DAL;
using cinema_project.DAL.DTO;

namespace cinema_project.UI.Forms.frmDatVe
{
    public partial class frmDatVe : Form
    {
        private readonly Phim _phim;
        private readonly SupabaseService _supabase = new();
        private readonly GheSuatChieuDAL _gheDAL = new();
        private readonly List<SuatChieu> _cacSuat = new();
        private SuatChieu? _suatDangChon;
        private int _lanTaiGhe;

        public IReadOnlyList<string> GheDaChon => SapXepMaGhe(_gheDangChon);
        public SuatChieu? SuatChieuDaChon => _suatDangChon;
        public int SoNguoiDat => _gheDangChon.Count;
        public decimal GiaTamTinh => _gheDangChon.Sum(LayGiaGhe);

        private decimal GiaVeCoBan => (_suatDangChon?.GiaVe ?? 0) > 0
            ? _suatDangChon!.GiaVe : _phim.GiaVe;

        public frmDatVe(Phim phim)
        {
            _phim = phim ?? throw new ArgumentNullException(nameof(phim));
            InitializeComponent();
            HienThiThongTinPhim();
            TaoSoDoGhe();
            CapNhatTongTien();
        }

        private void HienThiThongTinPhim()
        {
            lblTenPhim.Text = _phim.Ten;
            lblThongTinPhim.Text = $"Độ tuổi: {_phim.DoTuoi}   •   Thời lượng: {_phim.ThoiLuong}   •   Giá vé cơ bản: {_phim.GiaVe:N0} VNĐ";
            Text = $"Đặt vé - {_phim.Ten}";
        }

        private async void frmDatVe_Load(object? sender, EventArgs e) => await TaiSuatChieuAsync();

        private async Task TaiSuatChieuAsync()
        {
            cmbSuatChieu.Enabled = btnXacNhan.Enabled = false;
            lblTrangThai.Text = "Đang tải suất chiếu...";

            try
            {
                DateTime now = DateTime.Now;
                List<SuatChieu> tatCa = await _supabase.LayTatCaSuatChieuAsync();
                _cacSuat.Clear();
                _cacSuat.AddRange(tatCa.Where(s => LaPhimCanDat(s) &&
                    !string.Equals(s.TrangThai, "Đã hủy", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(s.TrangThai, "Hoàn tất", StringComparison.OrdinalIgnoreCase) &&
                    (s.NgayChieu.Date > now.Date ||
                     (s.NgayChieu.Date == now.Date && s.GioBatDau >= now.TimeOfDay)))
                    .OrderBy(s => s.NgayChieu).ThenBy(s => s.GioBatDau));

                var hienThi = _cacSuat.Select(s => new SuatChieuLuaChon(s)).ToList();
                cmbSuatChieu.DisplayMember = nameof(SuatChieuLuaChon.NoiDung);
                cmbSuatChieu.ValueMember = nameof(SuatChieuLuaChon.MaSuatChieu);
                cmbSuatChieu.DataSource = hienThi;
                cmbSuatChieu.Enabled = hienThi.Count > 0;

                if (hienThi.Count > 0)
                {
                    if (cmbSuatChieu.SelectedIndex < 0) cmbSuatChieu.SelectedIndex = 0;
                    lblTrangThai.Text = "Chọn suất chiếu, rồi chọn một hoặc nhiều ghế. Màu ghế biểu thị loại giá.";
                    return;
                }

                _suatDangChon = null;
                _gheDangChon.Clear();
                TaoSoDoGhe();
                CapNhatTongTien();
                lblSuatDaChon.Text = "Chưa có suất chiếu";
                lblTrangThai.Text = "Chưa có suất chiếu phù hợp cho phim này. Kiểm tra mục Lịch chiếu.";
            }
            catch (Exception ex)
            {
                _suatDangChon = null;
                _gheDangChon.Clear();
                TaoSoDoGhe();
                CapNhatTongTien();
                lblTrangThai.Text = "Không tải được suất chiếu.";
                MessageBox.Show("Không tải được suất chiếu từ Supabase.\n\n" + ex.Message,
                    "Lỗi tải suất chiếu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool LaPhimCanDat(SuatChieu s)
        {
            bool trungMa = _phim.MaPhim > 0 &&
                (s.MaPhim == _phim.MaPhim || (s.PhimThongTin?.MaPhim ?? 0) == _phim.MaPhim);
            bool trungTen = !string.IsNullOrWhiteSpace(s.TenPhim) &&
                string.Equals(s.TenPhim.Trim(), _phim.Ten.Trim(), StringComparison.OrdinalIgnoreCase);
            return trungMa || trungTen;
        }

        private async void cmbSuatChieu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbSuatChieu.SelectedItem is not SuatChieuLuaChon item) return;

            _suatDangChon = item.Suat;
            _gheDangChon.Clear();
            _gheDaDat.Clear();
            _loiTaiGhe = false;
            _dangTaiGhe = true;
            int lanTai = ++_lanTaiGhe;

            lblSuatDaChon.Text = $"{_suatDangChon.NgayChieu:dd/MM/yyyy} • {_suatDangChon.GioBatDau:hh\\:mm}\n{_suatDangChon.TenPhong} • {_suatDangChon.DinhDang}";
            lblTrangThai.Text = "Đang tải ghế đã đặt của suất chiếu...";
            TaoSoDoGhe();
            CapNhatTongTien();
            await TaiGheDaDatAsync(_suatDangChon.MaSuatChieu, lanTai);
        }

        private async Task TaiGheDaDatAsync(int maSuatChieu, int lanTai)
        {
            try
            {
                List<string> danhSach = await _gheDAL.LayGheDaDatAsync(maSuatChieu);
                if (lanTai != _lanTaiGhe || _suatDangChon?.MaSuatChieu != maSuatChieu) return;

                _gheDaDat.Clear();
                foreach (string ghe in danhSach.Where(x => !string.IsNullOrWhiteSpace(x)))
                    _gheDaDat.Add(ghe.Trim().ToUpperInvariant());
                _loiTaiGhe = false;
                lblTrangThai.Text = $"Đã tải sơ đồ ghế. Có {_gheDaDat.Count} ghế đã đặt trong suất chiếu này.";
            }
            catch (Exception ex)
            {
                if (lanTai != _lanTaiGhe) return;
                _loiTaiGhe = true;
                _gheDangChon.Clear();
                lblTrangThai.Text = "Không tải được trạng thái ghế. Vui lòng kiểm tra kết nối rồi chọn lại suất chiếu.";
                MessageBox.Show("Không tải được ghế đã đặt từ Supabase. Để tránh đặt trùng ghế, chức năng xác nhận đang bị khóa.\n\n" + ex.Message,
                    "Lỗi tải sơ đồ ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (lanTai == _lanTaiGhe && !IsDisposed)
                {
                    _dangTaiGhe = false;
                    foreach (var ghe in _nutGhe) CapNhatMauNutGhe(ghe.Value, ghe.Key);
                    CapNhatTongTien();
                }
            }
        }

        private void CapNhatTongTien()
        {
            decimal gia = GiaVeCoBan;
            lblSoGhe.Text = $"Số ghế đã chọn: {_gheDangChon.Count}";
            lblDanhSachGhe.Text = _gheDangChon.Count == 0 ? "Chưa chọn ghế" : string.Join(", ", GheDaChon);
            lblGiaThuong.Text = $"Ghế thường: {gia * HeSoGheThuong:N0} VNĐ";
            lblGiaVIP.Text = $"Ghế VIP: {gia * HeSoGheVIP:N0} VNĐ";
            lblGiaDoi.Text = $"Ghế đôi (mỗi chỗ): {gia * HeSoGheDoi:N0} VNĐ";
            lblTongTien.Text = $"{GiaTamTinh:N0} VNĐ";
            lblHuongDanChonGhe.Text = "Chọn một hoặc nhiều ghế để tiếp tục.";
            btnXacNhan.Enabled = _suatDangChon != null && !_dangTaiGhe && !_loiTaiGhe && _gheDangChon.Count > 0;
        }

        private async void btnXacNhan_Click(object? sender, EventArgs e)
        {
            if (!PhienDangNhap.DaDangNhap ||
                string.IsNullOrWhiteSpace(PhienDangNhap.NguoiDung?.UserId) ||
                string.IsNullOrWhiteSpace(PhienDangNhap.NguoiDung?.AccessToken))
            {
                MessageBox.Show("Vui lòng đăng nhập trước khi đặt vé.", "Yêu cầu đăng nhập",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_suatDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn suất chiếu.", "Đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_dangTaiGhe || _loiTaiGhe)
            {
                MessageBox.Show("Chưa tải được trạng thái ghế. Vui lòng chọn lại suất chiếu hoặc kiểm tra kết nối Supabase.",
                    "Đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_gheDangChon.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một ghế.",
                    "Đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int maSuatChieu = _suatDangChon.MaSuatChieu;
            List<string> gheDaChon = GheDaChon.ToList();
            var danhSachLuu = gheDaChon.Select(ghe => new GheSuatChieuDat
            {
                MaSuatChieu = maSuatChieu,
                MaGhe = ghe,
                GiaGhe = LayGiaGhe(ghe),
                TrangThai = "da_dat"
            }).ToList();

            btnXacNhan.Enabled = false;
            lblTrangThai.Text = "Đang lưu ghế lên Supabase...";
            try
            {
                await _gheDAL.DatGheAsync(danhSachLuu);
                foreach (string ghe in gheDaChon) _gheDaDat.Add(ghe);
                lblTrangThai.Text = "Đã lưu ghế thành công.";
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _gheDangChon.Clear();
                int lanTai = ++_lanTaiGhe;
                _dangTaiGhe = true;
                _loiTaiGhe = false;
                TaoSoDoGhe();
                CapNhatTongTien();
                await TaiGheDaDatAsync(maSuatChieu, lanTai);
                MessageBox.Show("Không thể xác nhận ghế. Có thể ghế vừa được người khác đặt hoặc Supabase từ chối ghi dữ liệu.\n\n" + ex.Message,
                    "Không thể đặt ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (!IsDisposed) CapNhatTongTien();
            }
        }

        private void btnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private sealed class SuatChieuLuaChon
        {
            public SuatChieu Suat { get; }
            public int MaSuatChieu => Suat.MaSuatChieu;
            public string NoiDung { get; }

            public SuatChieuLuaChon(SuatChieu suat)
            {
                Suat = suat ?? throw new ArgumentNullException(nameof(suat));
                NoiDung = $"{Suat.NgayChieu:dd/MM/yyyy} • {Suat.GioBatDau:hh\\:mm} | " +
                          $"{Suat.TenPhong} | {Suat.DinhDang}";
            }
        }
    }
}
