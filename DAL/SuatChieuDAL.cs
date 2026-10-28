using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public class SuatChieuDAL
    {
        private readonly SupabaseService _supabase = new();

        public List<SuatChieu> LayTatCa() =>
            Task.Run(() => _supabase.LayTatCaSuatChieuAsync()).GetAwaiter().GetResult();

        public SuatChieu? LayTheoMa(int maSuatChieu) =>
            LayTatCa().FirstOrDefault(s => s.MaSuatChieu == maSuatChieu);

        public Task<bool> Them(SuatChieu sc) => _supabase.ThemSuatChieuAsync(sc);

        public Task<bool> CapNhat(SuatChieu sc) => _supabase.CapNhatSuatChieuAsync(sc);

        public Task<bool> Xoa(int maSuatChieu) => _supabase.XoaSuatChieuAsync(maSuatChieu);
    }
}