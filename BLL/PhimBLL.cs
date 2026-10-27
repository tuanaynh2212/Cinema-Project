using System.Collections.Generic;
using System.Threading.Tasks;
using cinema_project.DAL;
using cinema_project.BLL.Services;

namespace cinema_project.BLL
{
    /// <summary>
    /// Lớp xử lý nghiệp vụ Phim (BLL) - Quản lý dữ liệu từ TMDB trong bộ nhớ (không cần SQL)
    /// </summary>
    public class PhimBLL
    {
        private readonly PhimDAL _phimDAL;
        private readonly TmdbService _tmdbService;

        public PhimBLL()
        {
            _phimDAL = new PhimDAL();
            _tmdbService = new TmdbService();
        }

        /// <summary>
        /// Khởi tạo và nạp dữ liệu phim trực tiếp từ TMDB
        /// </summary>
        public async Task<int> NapPhimTuTmdbAsync()
        {
            var dsPhim = await _tmdbService.LayDanhSachPhimTuTmdbAsync();
            _phimDAL.CapNhatDanhSach(dsPhim);
            return dsPhim.Count;
        }

        public List<Phim> LayTatCaPhim()
        {
            return _phimDAL.LayTatCa();
        }

        public List<Phim> LayPhimTheoLoai(string loaiPhim)
        {
            return _phimDAL.LayTheoLoai(loaiPhim);
        }

        public List<Phim> TimKiemPhim(string tuKhoa)
        {
            return _phimDAL.TimKiem(tuKhoa);
        }
    }
}
