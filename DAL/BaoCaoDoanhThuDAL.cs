using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL.DTO;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.DAL
{
    public class BaoCaoDoanhThuDAL
    {
        private readonly SupabaseService _supabase = new SupabaseService();
        private readonly ILogger<BaoCaoDoanhThuDAL> logger = AppLogging.CreateLogger<BaoCaoDoanhThuDAL>();

        private static readonly JsonSerializerOptions _json =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        public async Task<List<BaoCaoDoanhThu>> LayDoanhThuDaThanhToanAsync()
        {
            AuthLoginResult? phien = PhienDangNhap.NguoiDung;

            // 1. Log cảnh báo khi người dùng chưa đăng nhập
            if (phien == null || string.IsNullOrWhiteSpace(phien.UserId) ||
                string.IsNullOrWhiteSpace(phien.AccessToken))
            {
                logger.LogWarning("Người dùng chưa đăng nhập hoặc không có quyền truy cập báo cáo doanh thu.");
                throw new UnauthorizedAccessException(
                    "Vui lòng đăng nhập trước khi xem báo cáo doanh thu.");
            }

            // 2. Log cảnh báo khi người dùng không phải Admin
            if (!phien.LaAdmin)
            {
                logger.LogWarning("Yêu cầu bị từ chối: Tài khoản UserId={UserId} không có quyền Admin.", phien.UserId);
                throw new UnauthorizedAccessException(
                    "Chỉ tài khoản quản trị viên mới được xem thống kê doanh thu.");
            }

            logger.LogInformation("Tài khoản Admin UserId={UserId} đang yêu cầu lấy báo cáo doanh thu từ Supabase...", phien.UserId);

            try
            {
                string path =
                    "ticket_sales?select=id,user_id,movie_id,movie_title,quantity,unit_price,total_amount,payment_status,purchased_at" +
                    "&payment_status=eq.paid" +
                    "&order=purchased_at.desc";

                using HttpRequestMessage request =
                    _supabase.CreateRequest(HttpMethod.Get, path, phien.AccessToken);
                using HttpResponseMessage response = await _http.SendAsync(request);

                string noiDung = await response.Content.ReadAsStringAsync();

                // 3. Log lỗi khi API Supabase trả về lỗi HTTP
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Lỗi khi truy vấn dữ liệu từ Supabase. HTTP Status: {StatusCode}. Chi tiết: {NoiDung}",
                        (int)response.StatusCode, noiDung);
                    throw new InvalidOperationException(
                        "Không lấy được dữ liệu doanh thu từ Supabase. HTTP " +
                        (int)response.StatusCode + ": " + noiDung);
                }

                List<BaoCaoDoanhThu> danhSach = JsonSerializer.Deserialize<List<BaoCaoDoanhThu>>(noiDung, _json)
                                                ?? new List<BaoCaoDoanhThu>();

                // 4. Log thông tin thành công và số lượng bản ghi tải về
                logger.LogInformation("Lấy báo cáo doanh thu thành công. Tổng số bản ghi: {Count}", danhSach.Count);

                return danhSach;
            }
            catch (Exception ex) when (ex is not UnauthorizedAccessException)
            {
                // 5. Log ngoại lệ hệ thống (mạng, Deserialize lỗi, v.v.)
                logger.LogError(ex, "Đã xảy ra lỗi ngoài dự kiến trong hàm LayDoanhThuDaThanhToanAsync.");
                throw;
            }
        }
    }
}