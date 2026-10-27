using System;
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
            // 1. Lấy dữ liệu từ Supabase trước
            var dsSupabase = await _phimDAL.LayPhimTuSupabaseAsync();

            // 2. Nếu Supabase chưa có dữ liệu (hoặc muốn đồng bộ thêm)
            if (dsSupabase.Count == 0)
            {
                List<Phim> dsNguon = new();
                try
                {
                    dsNguon = await _tmdbService.LayDanhSachPhimTuTmdbAsync();
                }
                catch
                {
                }

                if (dsNguon.Count > 0)
                {
                    // Đẩy danh sách phim lên Supabase
                    bool luuThanhCong = await _phimDAL.LuuPhimVaoSupabaseAsync(dsNguon);
                    if (luuThanhCong)
                    {
                        // Lấy lại danh sách chính thức từ Supabase
                        dsSupabase = await _phimDAL.LayPhimTuSupabaseAsync();
                    }
                    else
                    {
                        // Nếu mạng lỗi không đẩy được lên Supabase thì dùng tạm trong bộ nhớ
                        _phimDAL.CapNhatBoNhoTam(dsNguon);
                        return dsNguon;
                    }
                }
            }

            return dsSupabase;
        }

        public List<Phim> LayPhimTheoLoai(string loaiPhim)
        {
            return _phimDAL.LayTheoLoai(loaiPhim);
        }

        public List<Phim> LayTatCaPhim()
        {
            return _phimDAL.LayTatCa();
        }
    }
}
