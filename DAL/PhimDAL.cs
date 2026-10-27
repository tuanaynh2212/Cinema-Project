using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public class PhimDAL
    {
        private readonly SupabaseService _supabase = new();
        private readonly List<Phim> _danhSachPhim = new();

        public async Task<bool> LuuPhimVaoSupabaseAsync(List<Phim> dsPhim)
        {
            return await _supabase.ThemHoacCapNhatDanhSachPhimAsync(dsPhim);
        }

        public async Task<List<Phim>> LayPhimTuSupabaseAsync()
        {
            var ds = await _supabase.LayTatCaPhimAsync();
            if (ds.Count > 0)
            {
                _danhSachPhim.Clear();
                _danhSachPhim.AddRange(ds);
            }
            return new List<Phim>(_danhSachPhim);
        }

        public void CapNhatBoNhoTam(List<Phim> dsPhim)
        {
            _danhSachPhim.Clear();
            _danhSachPhim.AddRange(dsPhim);
        }

        public List<Phim> LayTatCa() => new(_danhSachPhim);

        public List<Phim> LayTheoLoai(string loaiPhim)
        {
            if (string.IsNullOrEmpty(loaiPhim) || loaiPhim == "Tất cả")
            {
                return LayTatCa();
            }

            var ketQua = _danhSachPhim
                .Where(p => p.LoaiPhim.Equals(loaiPhim, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return ketQua.Count > 0 ? ketQua : LayTatCa();
        }
    }
}
