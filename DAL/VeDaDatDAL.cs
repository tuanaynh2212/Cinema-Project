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
        private readonly ILogger<VeDaDatDAL> _logger;

        public VeDaDatDAL(ILogger<VeDaDatDAL>? logger = null)
        {
            _logger = logger ?? AppLogging.CreateLogger<VeDaDatDAL>();
        }
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
                _logger.LogWarning("Xem vé: người dùng chưa đăng nhập hoặc phiên không hợp lệ.");
                throw new UnauthorizedAccessException("Vui lòng đăng nhập để xem vé của bạn.");
            }

            string userId = Uri.EscapeDataString(phien.UserId);
            string path =
                "ghe_suat_chieu?select=id,user_id,ma_suat_chieu,ma_ghe,gia_ghe,trang_thai,ngay_dat" +
                $"&user_id=eq.{userId}&trang_thai=eq.da_dat&order=ngay_dat.desc";
            _logger.LogInformation("Bắt đầu tải danh sách vé của user {UserId}.", phien.UserId);
            

            using HttpRequestMessage request = _supabase.CreateRequest(HttpMethod.Get, path, phien.AccessToken);
            using HttpResponseMessage response = await _http.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Tải vé thất bại: HTTP {Status}",(int)response.StatusCode);
                throw new InvalidOperationException(
                    $"Không tải được vé của tài khoản (HTTP {(int)response.StatusCode}).\n{content}");
            }

            List<VeDaDatItem>? result = JsonSerializer.Deserialize<List<VeDaDatItem>>(content, _json);

            List<VeDaDatItem> danhSach = result?.Where(x =>x.TrangThai == "da_dat" &&
            string.Equals(x.UserId, phien.UserId,StringComparison.OrdinalIgnoreCase))
            .ToList() ?? new List<VeDaDatItem>();

            _logger.LogInformation("Tải thành công {Count} vé của user {UserId}.",danhSach.Count, phien.UserId);

            return danhSach;
        }
    }
}
