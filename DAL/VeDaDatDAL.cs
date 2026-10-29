#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public sealed class VeDaDatItem
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("ma_suat_chieu")]
        public int MaSuatChieu { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = "";

        [JsonPropertyName("ma_ghe")]
        public string MaGhe { get; set; } = "";

        [JsonPropertyName("gia_ghe")]
        public decimal GiaGhe { get; set; }

        [JsonPropertyName("trang_thai")]
        public string TrangThai { get; set; } = "";

        [JsonPropertyName("ngay_dat")]
        public DateTime NgayDat { get; set; }
    }

    public sealed class VeDaDatDAL
    {
        private readonly SupabaseService _supabase = new SupabaseService();
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };
        private static readonly JsonSerializerOptions _json = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Chỉ tải ghế/vé thuộc tài khoản đang đăng nhập.
        public async Task<List<VeDaDatItem>> LayTatCaGheDaDatAsync()
        {
            AuthLoginResult? phien = PhienDangNhap.NguoiDung;
            if (phien == null || string.IsNullOrWhiteSpace(phien.UserId) ||
                string.IsNullOrWhiteSpace(phien.AccessToken))
            {
                throw new UnauthorizedAccessException("Vui lòng đăng nhập để xem vé của bạn.");
            }

            string userId = Uri.EscapeDataString(phien.UserId);
            string path =
                "ghe_suat_chieu?select=id,user_id,ma_suat_chieu,ma_ghe,gia_ghe,trang_thai,ngay_dat" +
                $"&user_id=eq.{userId}&trang_thai=eq.da_dat&order=ngay_dat.desc";

            using HttpRequestMessage request = _supabase.CreateRequest(HttpMethod.Get, path, phien.AccessToken);
            using HttpResponseMessage response = await _http.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Không tải được vé của tài khoản (HTTP {(int)response.StatusCode}).\n{content}");
            }

            List<VeDaDatItem>? result = JsonSerializer.Deserialize<List<VeDaDatItem>>(content, _json);
            return result?.Where(x => x.TrangThai == "da_dat" &&
                                      string.Equals(x.UserId, phien.UserId, StringComparison.OrdinalIgnoreCase))
                   .ToList() ?? new List<VeDaDatItem>();
        }
    }
}
