using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cinema_project.DAL;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.BLL
{
    public class SuatChieuBLL
    {
        private readonly SuatChieuDAL _dal = new();
        private readonly ILogger<SuatChieuBLL> _logger;

        public SuatChieuBLL(ILogger<SuatChieuBLL>? logger = null)
        {
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
        {
            _logger.LogDebug("Lọc suất chiếu: ngay={Ngay}, phim={Phim}, maPhong={MaPhong}, trangThai={TrangThai}, tuKhoa={TuKhoa}",
                ngay, tenPhim, maPhong, trangThai, tuKhoa);

            var ds = LayTatCa().AsEnumerable();

            if (ngay.HasValue)
            {
                ds = ds.Where(s => s.NgayChieu.Date == ngay.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(tenPhim) && tenPhim != "Tất cả phim")
            {
                ds = ds.Where(s => s.TenPhim.Equals(tenPhim, StringComparison.OrdinalIgnoreCase));
            }

            if (maPhong.HasValue && maPhong.Value > 0)
            {
                ds = ds.Where(s => s.MaPhong == maPhong.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "Tất cả trạng thái")
            {
                ds = ds.Where(s => s.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string kw = tuKhoa.Trim().ToLowerInvariant();
                ds = ds.Where(s =>
                    s.MaSuatChieu.ToString().Contains(kw) ||
                    s.TenPhim.ToLowerInvariant().Contains(kw) ||
                    s.TenPhong.ToLowerInvariant().Contains(kw) ||
                    s.MaPhong.ToString().Contains(kw) ||
                    s.DinhDang.ToLowerInvariant().Contains(kw));
            }

            var result = ds.OrderBy(s => s.NgayChieu)
                           .ThenBy(s => s.GioBatDau)
                           .ToList();
            _logger.LogDebug("Kết quả lọc: {Count} suất chiếu", result.Count);
            return result;
        }

        public List<SuatChieu> LocSuatChieu(DateTime? ngay, string? tenPhim, string? tenPhong, string? trangThai, string? tuKhoa)
        {
            _logger.LogDebug("Lọc suất chiếu: ngay={Ngay}, phim={Phim}, phong={Phong}, trangThai={TrangThai}, tuKhoa={TuKhoa}",
                ngay, tenPhim, tenPhong, trangThai, tuKhoa);

            var ds = LayTatCa().AsEnumerable();

            if (ngay.HasValue)
            {
                ds = ds.Where(s => s.NgayChieu.Date == ngay.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(tenPhim) && tenPhim != "Tất cả phim")
            {
                ds = ds.Where(s => s.TenPhim.Equals(tenPhim, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(tenPhong) && tenPhong != "Tất cả phòng")
            {
                ds = ds.Where(s => s.MaPhong.ToString().Equals(tenPhong, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(trangThai) && trangThai != "Tất cả trạng thái")
            {
                ds = ds.Where(s => s.TrangThai.Equals(trangThai, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string kw = tuKhoa.Trim().ToLowerInvariant();
                ds = ds.Where(s =>
                    s.MaSuatChieu.ToString().Contains(kw) ||
                    s.TenPhim.ToLowerInvariant().Contains(kw) ||
                    s.TenPhong.ToLowerInvariant().Contains(kw) ||
                    s.MaPhong.ToString().Contains(kw) ||
                    s.DinhDang.ToLowerInvariant().Contains(kw));
            }

            var result = ds.OrderBy(s => s.NgayChieu)
                           .ThenBy(s => s.GioBatDau)
                           .ToList();
            _logger.LogDebug("Kết quả lọc: {Count} suất chiếu", result.Count);
            return result;
        }

        public bool KiemTraTrungLich(SuatChieu sc, out string thongBaoLoi)
        {
            thongBaoLoi = "";
            var danhSach = LayTatCa();

            foreach (var existing in danhSach)
            {
                if (existing.MaSuatChieu == sc.MaSuatChieu) continue;

                bool cungPhong = (sc.MaPhong > 0 && existing.MaPhong > 0)
                    ? existing.MaPhong == sc.MaPhong
                    : existing.TenPhong.Equals(sc.TenPhong, StringComparison.OrdinalIgnoreCase);

                if (cungPhong && existing.NgayChieu.Date == sc.NgayChieu.Date)
                {
                    if (sc.GioBatDau < existing.GioKetThuc && sc.GioKetThuc > existing.GioBatDau)
                    {
                        string tenPhongHienThi = !string.IsNullOrWhiteSpace(sc.TenPhong) ? sc.TenPhong : $"Phòng {sc.MaPhong}";
                        thongBaoLoi = $"Trùng lịch chiếu tại '{tenPhongHienThi}'! Đã có phim '{existing.TenPhim}' chiếu khung giờ {existing.KhungGio}.";
                        _logger.LogWarning("Trùng lịch: suất {MaSuatChieu} với suất {MaTrung} tại {Phong} ngày {Ngay}",
                            sc.MaSuatChieu, existing.MaSuatChieu, tenPhongHienThi, sc.NgayChieu.Date);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ThemSuatChieu(SuatChieu sc, out string thongBaoLoi)
        {
            _logger.LogInformation("Thêm suất chiếu: phim={Phim}, phong={Phong}, ngay={Ngay}", sc.TenPhim, sc.TenPhong, sc.NgayChieu.Date);

            if (!KiemTraHopLe(sc, out thongBaoLoi)) return false;
            if (KiemTraTrungLich(sc, out thongBaoLoi)) return false;

            try
            {
                var ok = _dal.Them(sc).GetAwaiter().GetResult();
                if (ok) _logger.LogInformation("Thêm suất chiếu thành công: {Phim}", sc.TenPhim);
                else _logger.LogWarning("Thêm suất chiếu không thành công: {Phim}", sc.TenPhim);
                return ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Thêm suất chiếu thất bại: {Phim}", sc.TenPhim);
                throw;
            }
        }

        public bool CapNhatSuatChieu(SuatChieu sc, out string thongBaoLoi)
        {
            _logger.LogInformation("Cập nhật suất chiếu {MaSuatChieu}", sc.MaSuatChieu);

            if (!KiemTraHopLe(sc, out thongBaoLoi)) return false;
            if (KiemTraTrungLich(sc, out thongBaoLoi)) return false;

            try
            {
                var ok = _dal.CapNhat(sc).GetAwaiter().GetResult();
                if (ok) _logger.LogInformation("Cập nhật suất chiếu thành công: {MaSuatChieu}", sc.MaSuatChieu);
                else _logger.LogWarning("Cập nhật suất chiếu không thành công: {MaSuatChieu}", sc.MaSuatChieu);
                return ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cập nhật suất chiếu {MaSuatChieu} thất bại", sc.MaSuatChieu);
                throw;
            }
        }

        public bool XoaSuatChieu(int maSuatChieu)
        {
            _logger.LogInformation("Xóa suất chiếu {MaSuatChieu}", maSuatChieu);
            try
            {
                var ok = _dal.Xoa(maSuatChieu).GetAwaiter().GetResult();
                if (ok) _logger.LogInformation("Xóa suất chiếu thành công: {MaSuatChieu}", maSuatChieu);
                else _logger.LogWarning("Xóa suất chiếu không thành công: {MaSuatChieu}", maSuatChieu);
                return ok;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Xóa suất chiếu {MaSuatChieu} thất bại", maSuatChieu);
                throw;
            }
        }

        public TimeSpan TinhGioKetThuc(TimeSpan gioBatDau, string? thoiLuongStr)
        {
            int phut = 120;
            if (!string.IsNullOrWhiteSpace(thoiLuongStr))
            {
                var match = Regex.Match(thoiLuongStr, @"\d+");
                if (match.Success && int.TryParse(match.Value, out int p))
                {
                    phut = p;
                }
            }

            phut += 15;

            var ketThuc = gioBatDau.Add(TimeSpan.FromMinutes(phut));
            if (ketThuc.TotalHours >= 24)
            {
                ketThuc = new TimeSpan(23, 59, 0);
            }
            return ketThuc;
        }

        private bool KiemTraHopLe(SuatChieu sc, out string thongBaoLoi)
        {
            thongBaoLoi = "";
            if (string.IsNullOrWhiteSpace(sc.TenPhim))
            {
                thongBaoLoi = "Vui lòng chọn hoặc nhập tên phim.";
            }
            else if (string.IsNullOrWhiteSpace(sc.TenPhong))
            {
                thongBaoLoi = "Vui lòng chọn phòng chiếu.";
            }
            else if (sc.GioKetThuc <= sc.GioBatDau)
            {
                thongBaoLoi = "Giờ kết thúc phải lớn hơn giờ bắt đầu.";
            }
            else if (sc.GiaVe < 0)
            {
                thongBaoLoi = "Giá vé không được nhỏ hơn 0.";
            }

            if (thongBaoLoi.Length > 0)
            {
                _logger.LogWarning("Dữ liệu suất chiếu không hợp lệ: {Loi}", thongBaoLoi);
                return false;
            }

            return true;
        }
    }
}