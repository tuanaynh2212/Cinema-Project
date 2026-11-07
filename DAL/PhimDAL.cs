using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.DAL
{
    public class PhimDAL
    {
        private readonly SupabaseService _supabase = new();
        private readonly ILogger<PhimDAL> _logger = AppLogging.CreateLogger<PhimDAL>();
        private readonly List<Phim> _danhSachPhim = new();

        public async Task<bool> LuuPhimVaoSupabaseAsync(List<Phim> ds)
        {
            _logger.LogInformation("Đang thực hiện lưu/cập nhật {Count} bộ phim lên Supabase...", ds?.Count ?? 0);
            try
            {
                bool ketQua = await _supabase.ThemHoacCapNhatDanhSachPhimAsync(ds);
                if (ketQua)
                {
                    _logger.LogInformation("Lưu/cập nhật danh sách phim lên Supabase thành công.");
                }
                else
                {
                    _logger.LogWarning("Lưu/cập nhật danh sách phim lên Supabase trả về kết quả false.");
                }
                return ketQua;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi ngoại lệ khi lưu danh sách phim lên Supabase.");
                throw;
            }
        }

        public async Task<List<Phim>> LayPhimTuSupabaseAsync()
        {
            _logger.LogInformation("Đang yêu cầu tải danh sách phim từ Supabase...");
            try
            {
                var ds = await _supabase.LayTatCaPhimAsync();
                if (ds != null && ds.Count > 0)
                {
                    CapNhatBoNhoTam(ds);
                    _logger.LogInformation("Tải thành công {Count} phim từ Supabase và cập nhật bộ nhớ tạm.", ds.Count);
                }
                else
                {
                    _logger.LogWarning("Không lấy được dữ liệu phim từ Supabase hoặc danh sách trả về rỗng.");
                }
                return new(_danhSachPhim);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi ngoại lệ khi tải danh sách phim từ Supabase.");
                throw;
            }
        }

        public void CapNhatBoNhoTam(List<Phim> ds)
        {
            _danhSachPhim.Clear();
            if (ds != null)
            {
                _danhSachPhim.AddRange(ds);
            }
            _logger.LogDebug("Bộ nhớ tạm đã được làm mới. Tổng số phim hiện tại: {Count}", _danhSachPhim.Count);
        }

        public List<Phim> LayTatCa()
        {
            _logger.LogDebug("Lấy tất cả phim từ bộ nhớ tạm. Tổng số lượng: {Count}", _danhSachPhim.Count);
            return new(_danhSachPhim);
        }

        public List<Phim> LayTheoLoai(string loai)
        {
            _logger.LogDebug("Thực hiện lọc danh sách phim theo loại: '{Loai}'", loai);

            if (string.IsNullOrEmpty(loai) || loai == "Tất cả")
                return LayTatCa();

            var ketQua = _danhSachPhim.Where(p => p.LoaiPhim.Equals(loai, StringComparison.OrdinalIgnoreCase)).ToList();

            if (ketQua.Count > 0)
            {
                _logger.LogDebug("Tìm thấy {Count} phim thuộc loại '{Loai}'.", ketQua.Count, loai);
                return ketQua;
            }
            else
            {
                _logger.LogWarning("Không tìm thấy phim nào thuộc loại '{Loai}', hệ thống trả về toàn bộ danh sách.", loai);
                return LayTatCa();
            }
        }
    }
}