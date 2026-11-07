#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using cinema_project.DAL.DTO;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.DAL
{
    public sealed class VeDaDatDAL
    {
        private readonly SupabaseService _supabase = new SupabaseService();
        private readonly ILogger<VeDaDatDAL> _logger = AppLogging.CreateLogger<VeDaDatDAL>();

        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };
        private static readonly JsonSerializerOptions _json = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<VeDaDatItem>> LayTatCaGheDaDatAsync()
        {
            AuthLoginResult? phien = PhienDangNhap.NguoiDung;
            if (phien == null || string.IsNullOrWhiteSpace(phien.UserId) ||
                string.IsNullOrWhiteSpace(phien.AccessToken))
            {
                _logger.LogWarning("Yêu cầu xem vé đã đặt bị từ chối: Người dùng chưa đăng nhập hoặc thiếu AccessToken.");
                throw new UnauthorizedAccessException("Vui lòng đăng nhập để xem vé của bạn.");
            }

            _logger.LogInformation("Đang tải danh sách vé đã đặt từ Supabase cho UserId={UserId}...", phien.UserId);

            try
            {
                string userId = Uri.EscapeDataString(phien.UserId);
                string path =
                    "ghe_suat_chieu?select=id,user_id,ma_suat_chieu,ma_ghe,gia_ghe,trang_thai,ngay_dat" +
                    $"&user_id=eq.{userId}&trang_thai=eq.da_dat&order=ngay_dat.desc";

                using HttpRequestMessage request = _supabase.CreateRequest(HttpMethod.Get, path, phien.AccessToken);
                using HttpResponseMessage response = await _http.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi khi tải vé đã đặt từ Supabase. HTTP Status: {StatusCode}. Chi tiết: {Content}",
                        (int)response.StatusCode, content);
                    throw new InvalidOperationException(
                        $"Không tải được vé của tài khoản (HTTP {(int)response.StatusCode}).\n{content}");
                }

                List<VeDaDatItem>? result = JsonSerializer.Deserialize<List<VeDaDatItem>>(content, _json);
                List<VeDaDatItem> danhSachVe = result?.Where(x => x.TrangThai == "da_dat" &&
                                                          string.Equals(x.UserId, phien.UserId, StringComparison.OrdinalIgnoreCase))
                       .ToList() ?? new List<VeDaDatItem>();

                _logger.LogInformation("Tải danh sách vé đã đặt thành công cho UserId={UserId}. Tổng số vé: {Count}", phien.UserId, danhSachVe.Count);
                return danhSachVe;
            }
            catch (Exception ex) when (ex is not UnauthorizedAccessException)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi ngoài dự kiến trong hàm LayTatCaGheDaDatAsync.");
                throw;
            }
        }
    }
}