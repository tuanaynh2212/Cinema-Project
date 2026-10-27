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

        public Task<bool> LuuPhimVaoSupabaseAsync(List<Phim> ds) => _supabase.ThemHoacCapNhatDanhSachPhimAsync(ds);

        public async Task<List<Phim>> LayPhimTuSupabaseAsync()
        {
            var ds = await _supabase.LayTatCaPhimAsync();
            if (ds.Count > 0) CapNhatBoNhoTam(ds);
            return new(_danhSachPhim);
        }

        public void CapNhatBoNhoTam(List<Phim> ds)
        {
            _danhSachPhim.Clear();
            _danhSachPhim.AddRange(ds);
        }

        public List<Phim> LayTatCa() => new(_danhSachPhim);

        public List<Phim> LayTheoLoai(string loai)
        {
            if (string.IsNullOrEmpty(loai) || loai == "Tất cả") return LayTatCa();
            var ketQua = _danhSachPhim.Where(p => p.LoaiPhim.Equals(loai, StringComparison.OrdinalIgnoreCase)).ToList();
            return ketQua.Count > 0 ? ketQua : LayTatCa();
        }
    }
}
