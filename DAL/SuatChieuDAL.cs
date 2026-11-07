using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public class SuatChieuDAL
    {
        private readonly SupabaseService _supabase = new();

        public virtual Task<List<SuatChieu>> LayTatCaAsync() =>
            _supabase.LayTatCaSuatChieuAsync();

        public virtual List<SuatChieu> LayTatCa() =>
            Task.Run(() => _supabase.LayTatCaSuatChieuAsync()).GetAwaiter().GetResult();

        public virtual SuatChieu? LayTheoMa(int maSuatChieu) =>
            LayTatCa().FirstOrDefault(s => s.MaSuatChieu == maSuatChieu);

        public virtual Task<bool> Them(SuatChieu sc) => _supabase.ThemSuatChieuAsync(sc);

        public virtual Task<bool> CapNhat(SuatChieu sc) => _supabase.CapNhatSuatChieuAsync(sc);

        public virtual Task<bool> Xoa(int maSuatChieu) => _supabase.XoaSuatChieuAsync(maSuatChieu);
    }
}