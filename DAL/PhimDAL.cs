using System;
using System.Collections.Generic;
using System.Linq;

namespace cinema_project.DAL
{
    /// <summary>
    /// Lớp quản lý dữ liệu Phim trong bộ nhớ (In-Memory Repository) - Không cần SQL
    /// </summary>
    public class PhimDAL
    {
        private readonly List<Phim> _danhSachPhim = new();

        /// <summary>
        /// Lấy toàn bộ danh sách phim
        /// </summary>
        public List<Phim> LayTatCa()
        {
            return new List<Phim>(_danhSachPhim);
        }

        /// <summary>
        /// Lấy danh sách phim theo phân loại (Đang chiếu, Đặc biệt, Sắp chiếu)
        /// </summary>
        public List<Phim> LayTheoLoai(string loaiPhim)
        {
            if (string.IsNullOrEmpty(loaiPhim) || loaiPhim == "Tất cả")
            {
                return LayTatCa();
            }

            return _danhSachPhim
                .Where(p => p.LoaiPhim.Equals(loaiPhim, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Tìm kiếm phim theo tên hoặc thể loại
        /// </summary>
        public List<Phim> TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return LayTatCa();
            }

            string kw = tuKhoa.Trim().ToLower();
            return _danhSachPhim
                .Where(p => p.Ten.ToLower().Contains(kw) || p.TheLoai.ToLower().Contains(kw))
                .ToList();
        }

        /// <summary>
        /// Cập nhật hoặc nạp mới toàn bộ danh sách phim (từ TMDB)
        /// </summary>
        public void CapNhatDanhSach(List<Phim> dsPhim)
        {
            _danhSachPhim.Clear();
            _danhSachPhim.AddRange(dsPhim);
        }

        /// <summary>
        /// Thêm hoặc cập nhật một phim đơn lẻ
        /// </summary>
        public void ThemHoacCapNhatPhim(Phim phim)
        {
            int index = _danhSachPhim.FindIndex(p => 
                (phim.TmdbId.HasValue && p.TmdbId == phim.TmdbId) || 
                p.Ten.Equals(phim.Ten, StringComparison.OrdinalIgnoreCase));

            if (index >= 0)
            {
                _danhSachPhim[index] = phim;
            }
            else
            {
                if (phim.MaPhim <= 0)
                {
                    phim.MaPhim = _danhSachPhim.Count > 0 ? _danhSachPhim.Max(p => p.MaPhim) + 1 : 1;
                }
                _danhSachPhim.Add(phim);
            }
        }
    }
}
