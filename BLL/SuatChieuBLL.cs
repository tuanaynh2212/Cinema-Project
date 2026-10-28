using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cinema_project.DAL;

namespace cinema_project.BLL
{
    public class SuatChieuBLL
    {
        private readonly SuatChieuDAL _dal = new();

        public List<SuatChieu> LayTatCa() => _dal.LayTatCa();

        public SuatChieu? LayTheoMa(int maSuatChieu) => _dal.LayTheoMa(maSuatChieu);

        public List<SuatChieu> LocSuatChieu(DateTime? ngay, string? tenPhim, int? maPhong, string? trangThai, string? tuKhoa)
        {
            var ds = _dal.LayTatCa().AsEnumerable();

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

            return ds.OrderBy(s => s.NgayChieu)
                     .ThenBy(s => s.GioBatDau)
                     .ToList();
        }

        public List<SuatChieu> LocSuatChieu(DateTime? ngay, string? tenPhim, string? tenPhong, string? trangThai, string? tuKhoa)
        {
            var ds = _dal.LayTatCa().AsEnumerable();

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
                ds = ds.Where(s => s.TenPhong.Equals(tenPhong, StringComparison.OrdinalIgnoreCase));
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

            return ds.OrderBy(s => s.NgayChieu)
                     .ThenBy(s => s.GioBatDau)
                     .ToList();
        }

        public bool KiemTraTrungLich(SuatChieu sc, out string thongBaoLoi)
        {
            thongBaoLoi = "";
            var danhSach = _dal.LayTatCa();

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
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ThemSuatChieu(SuatChieu sc, out string thongBaoLoi)
        {
            thongBaoLoi = "";
            if (!KiemTraHopLe(sc, out thongBaoLoi)) return false;

            if (KiemTraTrungLich(sc, out thongBaoLoi)) return false;

            return _dal.    Them(sc).Result;
        }

        public bool CapNhatSuatChieu(SuatChieu sc, out string thongBaoLoi)
        {
            thongBaoLoi = "";
            if (!KiemTraHopLe(sc, out thongBaoLoi)) return false;

            if (KiemTraTrungLich(sc, out thongBaoLoi)) return false;

            return _dal.CapNhat(sc).Result;
        }

        public bool XoaSuatChieu(int maSuatChieu)
        {
            return _dal.Xoa(maSuatChieu).Result;
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
                return false;
            }

            if (string.IsNullOrWhiteSpace(sc.TenPhong))
            {
                thongBaoLoi = "Vui lòng chọn phòng chiếu.";
                return false;
            }

            if (sc.GioKetThuc <= sc.GioBatDau)
            {
                thongBaoLoi = "Giờ kết thúc phải lớn hơn giờ bắt đầu.";
                return false;
            }

            if (sc.GiaVe < 0)
            {
                thongBaoLoi = "Giá vé không được nhỏ hơn 0.";
                return false;
            }

            return true;
        }
    }
}
