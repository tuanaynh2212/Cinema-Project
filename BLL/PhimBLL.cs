using System.Collections.Generic;
using System.Threading.Tasks;
using cinema_project.DAL;
using cinema_project.BLL.Services;

namespace cinema_project.BLL
{
    public class PhimBLL
    {
        private readonly PhimDAL _phimDAL = new();
        private readonly TmdbService _tmdbService = new();

        public async Task<List<Phim>> DongBoVaLayPhimAsync()
        {
            var ds = await _phimDAL.LayPhimTuSupabaseAsync();
            if (ds.Count > 0) return ds;

            try
            {
                var dsTmdb = await _tmdbService.LayDanhSachPhimTuTmdbAsync();
                if (dsTmdb.Count > 0)
                {
                    if (await _phimDAL.LuuPhimVaoSupabaseAsync(dsTmdb))
                        return await _phimDAL.LayPhimTuSupabaseAsync();

                    _phimDAL.CapNhatBoNhoTam(dsTmdb);
                    return dsTmdb;
                }
            }
            catch { }

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
