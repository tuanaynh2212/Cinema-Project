using System.Collections.Generic;
using System.Threading.Tasks;
using cinema_project.DAL;
using cinema_project.BLL.Services;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.BLL
{
    public class PhimBLL
    {
        private readonly PhimDAL _phimDAL = new();
        private readonly TmdbService _tmdbService = new();
        private readonly ILogger<PhimBLL> _logger;

        public PhimBLL(ILogger<PhimBLL>? logger = null)
        {
            _logger = logger ?? AppLogging.CreateLogger<PhimBLL>();
        }
        public async Task<List<Phim>> DongBoVaLayPhimAsync()
        {
            _logger.LogInformation("Bắt đầu tải danh sách phim.");
            var ds = await _phimDAL.LayPhimTuSupabaseAsync();
            if (ds.Count > 0)
            {
                _logger.LogInformation("Lấy được {Count} phim từ Supabase.", ds.Count);
                return ds;
            }

            _logger.LogInformation("Supabase chưa có phim, chuyển sang lấy phim từ TMDB.");

            try
            {
                var dsTmdb = await _tmdbService.LayDanhSachPhimTuTmdbAsync();
                if (dsTmdb.Count > 0)
                {
                    if (await _phimDAL.LuuPhimVaoSupabaseAsync(dsTmdb))
                    {
                        _logger.LogInformation("Đồng bộ thành công {Count} phim vào Supabase.",
                            dsTmdb.Count);

                        return await _phimDAL.LayPhimTuSupabaseAsync();
                    }
                    _logger.LogWarning("Không lưu được phim vào Supabase, sử dụng bộ nhớ tạm.");
                    _phimDAL.CapNhatBoNhoTam(dsTmdb);
                    return dsTmdb;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex, "Đồng bộ phim từ TMDB thất bại.");
            }

            return ds;
        }

        public List<Phim> LayPhimTheoLoai(string loaiPhim) => _phimDAL.LayTheoLoai(loaiPhim);
        public List<Phim> LayTatCaPhim() => _phimDAL.LayTatCa();

        public Task<string?> LayTrailerKeyAsync(int tmdbId, string? tenPhim = null) =>
            _tmdbService.LayTrailerKeyAsync(tmdbId, tenPhim);

        public Task<(string? moTa, string? thoiLuong, string? trailerKey)> LayChiTietPhimBoSungAsync(int tmdbId, string? tenPhim = null) =>
            _tmdbService.LayChiTietPhimBoSungAsync(tmdbId, tenPhim);
    }
}
