using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using cinema_project.DAL;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.BLL
{
    public class SuatChieuBLL
    {
        private readonly SuatChieuDAL _dal;
        private readonly ILogger<SuatChieuBLL> _logger;

        public SuatChieuBLL(ILogger<SuatChieuBLL>? logger = null)
            : this(new SuatChieuDAL(), logger) { }

        public SuatChieuBLL(SuatChieuDAL dal, ILogger<SuatChieuBLL>? logger = null)
        {
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
            _logger = logger ?? AppLogging.CreateLogger<SuatChieuBLL>();
        }


        public List<SuatChieu> LayTatCa()
        {
            try
            {
                var ds = _dal.LayTatCa();
                _logger.LogDebug("Lấy tất cả suất chiếu: {Count}", ds.Count);
                return ds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lấy tất cả suất chiếu thất bại");
                throw;
            }
        }

        public SuatChieu? LayTheoMa(int maSuatChieu)
        {
            try
            {
                var sc = _dal.LayTheoMa(maSuatChieu);
                if (sc == null) _logger.LogWarning("Không tìm thấy suất chiếu {MaSuatChieu}", maSuatChieu);
                return sc;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lấy suất chiếu {MaSuatChieu} thất bại", maSuatChieu);
                throw;
            }
        }



        public List<SuatChieu> LocSuatChieu(DateTime? ngay, string? tenPhim, int? maPhong, string? trangThai, string? tuKhoa)
            => Loc(LayTatCa(), ngay, tenPhim,
                   maPhong is > 0 ? s => s.MaPhong == maPhong : null, trangThai, tuKhoa);

        public async Task<List<SuatChieu>> LocSuatChieuAsync(DateTime? ngay, string? tenPhim, int? maPhong, string? trangThai, string? tuKhoa)
            => Loc(await _dal.LayTatCaAsync(), ngay, tenPhim,
                   maPhong is > 0 ? s => s.MaPhong == maPhong : null, trangThai, tuKhoa);

        public List<SuatChieu> LocSuatChieu(DateTime? ngay, string? tenPhim, string? tenPhong, string? trangThai, string? tuKhoa)
            => Loc(LayTatCa(), ngay, tenPhim,
                   BoQua(tenPhong, "Tất cả phòng")
                       ? null
                       : s => s.MaPhong.ToString().Equals(tenPhong, StringComparison.OrdinalIgnoreCase),
                   trangThai, tuKhoa);

        private List<SuatChieu> Loc(IEnumerable<SuatChieu> danhSach, DateTime? ngay, string? tenPhim,
            Func<SuatChieu, bool>? locPhong, string? trangThai, string? tuKhoa)
        {
            string kw = tuKhoa?.Trim() ?? "";

            var result = danhSach
                .Where(s =>
                    (!ngay.HasValue || s.NgayChieu.Date == ngay.Value.Date) &&
                    (BoQua(tenPhim, "Tất cả phim") || s.TenPhim.Equals(tenPhim, StringComparison.OrdinalIgnoreCase)) &&
                    (locPhong == null || locPhong(s)) &&
                    (BoQua(trangThai, "Tất cả trạng thái") || s.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase)) &&
                    (kw.Length == 0 || KhopTuKhoa(s, kw)))
                .OrderBy(s => s.NgayChieu)
                .ThenBy(s => s.GioBatDau)
                .ToList();

            _logger.LogDebug("Kết quả lọc: {Count} suất chiếu", result.Count);
            return result;
        }

        private static bool BoQua(string? giaTri, string tatCa)
            => string.IsNullOrWhiteSpace(giaTri) || giaTri == tatCa;

        private static bool KhopTuKhoa(SuatChieu s, string kw)
            => new[] { s.MaSuatChieu.ToString(), s.TenPhim, s.TenPhong, s.TenRap, s.MaPhong.ToString(), s.DinhDang }
                .Any(v => v.Contains(kw, StringComparison.OrdinalIgnoreCase));

        public bool KiemTraTrungLich(SuatChieu sc, out string thongBaoLoi)
        {
            thongBaoLoi = TimLoiTrungLich(sc, LayTatCa()) ?? "";
            return thongBaoLoi.Length > 0;
        }

        private string? TimLoiTrungLich(SuatChieu sc, IEnumerable<SuatChieu> danhSach)
        {
            foreach (var e in danhSach)
            {
                if (e.MaSuatChieu == sc.MaSuatChieu) continue;

                bool cungRap = sc.MaRap > 0 && e.MaRap == sc.MaRap;
                bool cungPhong = sc.MaPhong > 0 && e.MaPhong > 0
                    ? e.MaPhong == sc.MaPhong
                    : e.TenPhong.Equals(sc.TenPhong, StringComparison.OrdinalIgnoreCase);

                if (!cungRap || !cungPhong || e.NgayChieu.Date != sc.NgayChieu.Date) continue;
                if (sc.GioBatDau >= e.GioKetThuc || sc.GioKetThuc <= e.GioBatDau) continue;

                string phong = !string.IsNullOrWhiteSpace(sc.TenPhong) ? sc.TenPhong : $"Phòng {sc.MaPhong}";
                _logger.LogWarning("Trùng lịch: suất {MaSuatChieu} với suất {MaTrung} tại {Phong} ngày {Ngay}",
                    sc.MaSuatChieu, e.MaSuatChieu, phong, sc.NgayChieu.Date);
                return $"Trùng lịch chiếu tại '{phong}'! Đã có phim '{e.TenPhim}' chiếu khung giờ {e.KhungGio}.";
            }
            return null;
        }



        public bool ThemSuatChieu(SuatChieu sc, out string thongBaoLoi)
            => LuuDongBo(sc, "Thêm", _dal.Them, out thongBaoLoi);

        public bool CapNhatSuatChieu(SuatChieu sc, out string thongBaoLoi)
            => LuuDongBo(sc, "Cập nhật", _dal.CapNhat, out thongBaoLoi);

        public Task<(bool ThanhCong, string Loi)> ThemSuatChieuAsync(SuatChieu sc)
            => LuuAsync(sc, "Thêm", _dal.Them);

        public Task<(bool ThanhCong, string Loi)> CapNhatSuatChieuAsync(SuatChieu sc)
            => LuuAsync(sc, "Cập nhật", _dal.CapNhat);

        public bool XoaSuatChieu(int maSuatChieu)
            => ThucThi("Xóa", maSuatChieu.ToString(), () => _dal.Xoa(maSuatChieu)).GetAwaiter().GetResult();

        public Task<bool> XoaSuatChieuAsync(int maSuatChieu)
            => ThucThi("Xóa", maSuatChieu.ToString(), () => _dal.Xoa(maSuatChieu));

        private bool LuuDongBo(SuatChieu sc, string hanhDong, Func<SuatChieu, Task<bool>> luu, out string loi)
        {
            _logger.LogInformation("{HanhDong} suất chiếu: phim={Phim}, phòng={Phong}, ngày={Ngay}",
                hanhDong, sc.TenPhim, sc.TenPhong, sc.NgayChieu.Date);

            if (!KiemTraHopLe(sc, out loi) || KiemTraTrungLich(sc, out loi)) return false;
            return ThucThi(hanhDong, sc.TenPhim, () => luu(sc)).GetAwaiter().GetResult();
        }

        private async Task<(bool ThanhCong, string Loi)> LuuAsync(SuatChieu sc, string hanhDong, Func<SuatChieu, Task<bool>> luu)
        {
            _logger.LogInformation("{HanhDong} suất chiếu: phim={Phim}, phòng={Phong}, ngày={Ngay}",
                hanhDong, sc.TenPhim, sc.TenPhong, sc.NgayChieu.Date);

            if (!KiemTraHopLe(sc, out string loi)) return (false, loi);

            string? loiTrung = TimLoiTrungLich(sc, await _dal.LayTatCaAsync());
            if (loiTrung is not null) return (false, loiTrung);

            return (await ThucThi(hanhDong, sc.TenPhim, () => luu(sc)), "");
        }

        private async Task<bool> ThucThi(string hanhDong, string doiTuong, Func<Task<bool>> thaoTac)
        {
            try
            {
                bool ok = await thaoTac();
                if (ok) _logger.LogInformation("{HanhDong} suất chiếu thành công: {DoiTuong}", hanhDong, doiTuong);
                else _logger.LogWarning("{HanhDong} suất chiếu không thành công: {DoiTuong}", hanhDong, doiTuong);
                return ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{HanhDong} suất chiếu thất bại: {DoiTuong}", hanhDong, doiTuong);
                throw;
            }
        }


        public TimeSpan TinhGioKetThuc(TimeSpan gioBatDau, string? thoiLuongStr)
        {
            int phut = 120;
            var match = Regex.Match(thoiLuongStr ?? "", @"\d+");
            if (match.Success && int.TryParse(match.Value, out int p)) phut = p;

            var ketThuc = gioBatDau.Add(TimeSpan.FromMinutes(phut + 15));
            return ketThuc.TotalHours >= 24 ? new TimeSpan(23, 59, 0) : ketThuc;
        }

        private bool KiemTraHopLe(SuatChieu sc, out string thongBaoLoi)
        {
            thongBaoLoi =
                string.IsNullOrWhiteSpace(sc.TenPhim) ? "Vui lòng chọn hoặc nhập tên phim." :
                sc.MaRap <= 0 ? "Vui lòng chọn rạp chiếu." :
                string.IsNullOrWhiteSpace(sc.TenPhong) ? "Vui lòng chọn phòng chiếu." :
                sc.GioKetThuc <= sc.GioBatDau ? "Giờ kết thúc phải lớn hơn giờ bắt đầu." :
                sc.GiaVe < 0 ? "Giá vé không được nhỏ hơn 0." : "";

            if (thongBaoLoi.Length == 0) return true;

            _logger.LogWarning("Dữ liệu suất chiếu không hợp lệ: {Loi}", thongBaoLoi);
            return false;
        }
    }
}