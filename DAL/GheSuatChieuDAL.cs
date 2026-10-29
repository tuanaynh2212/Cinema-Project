#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public sealed class GheSuatChieuDat
    {
        [JsonPropertyName("ma_suat_chieu")]
        public int MaSuatChieu { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = "";

        [JsonPropertyName("ma_ghe")]
        public string MaGhe { get; set; } = "";

        [JsonPropertyName("gia_ghe")]
        public decimal GiaGhe { get; set; }

        [JsonPropertyName("trang_thai")]
        public string TrangThai { get; set; } = "da_dat";
    }

    public sealed class GheSuatChieuDAL
    {
        private readonly SupabaseService _supabase = new SupabaseService();
        private static readonly HttpClient _http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
        private static readonly JsonSerializerOptions _json = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private static AuthLoginResult LayPhienDangNhap()
        {
            AuthLoginResult? phien = PhienDangNhap.NguoiDung;
            if (phien == null || string.IsNullOrWhiteSpace(phien.UserId) ||
                string.IsNullOrWhiteSpace(phien.AccessToken))
            {
                throw new UnauthorizedAccessException("Vui lòng đăng nhập trước khi đặt vé.");
            }

            return phien;
        }

        public async Task<List<string>> LayGheDaDatAsync(int maSuatChieu)
        {
            if (maSuatChieu <= 0)
                throw new ArgumentOutOfRangeException(nameof(maSuatChieu), "Mã suất chiếu không hợp lệ.");

            AuthLoginResult phien = LayPhienDangNhap();
            // Phải đọc trạng thái ghế của mọi người để tránh hai tài khoản cùng chọn một ghế.
            // RLS của bảng cần cho phép người dùng đã xác thực đọc các ghế đã đặt.
            using HttpRequestMessage request = _supabase.CreateRequest(
                HttpMethod.Get,
                $"ghe_suat_chieu?select=ma_ghe&ma_suat_chieu=eq.{maSuatChieu}&trang_thai=eq.da_dat",
                phien.AccessToken);

            using HttpResponseMessage response = await _http.SendAsync(request);
            string noiDung = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    $"Không tải được ghế đã đặt (HTTP {(int)response.StatusCode}). {noiDung}");

            List<GheSuatChieuDat>? danhSach = JsonSerializer.Deserialize<List<GheSuatChieuDat>>(noiDung, _json);
            return danhSach?.Where(x => !string.IsNullOrWhiteSpace(x.MaGhe))
                            .Select(x => x.MaGhe.Trim().ToUpperInvariant())
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList() ?? new List<string>();
        }

        // Lưu tất cả ghế đã chọn trong một request. Unique index ở CSDL chặn đặt trùng.
        public async Task DatGheAsync(IEnumerable<GheSuatChieuDat> danhSachGhe)
        {
            if (danhSachGhe == null)
                throw new ArgumentNullException(nameof(danhSachGhe));

            AuthLoginResult phien = LayPhienDangNhap();

            List<GheSuatChieuDat> ds = danhSachGhe
                .Where(x => x != null)
                .Select(x => new GheSuatChieuDat
                {
                    MaSuatChieu = x.MaSuatChieu,
                    // Không lấy user_id từ form: luôn gán theo phiên đăng nhập tin cậy.
                    UserId = phien.UserId,
                    MaGhe = (x.MaGhe ?? "").Trim().ToUpperInvariant(),
                    GiaGhe = x.GiaGhe,
                    TrangThai = "da_dat"
                })
                .ToList();

            if (ds.Count == 0)
                throw new InvalidOperationException("Không có ghế nào được chọn.");
            if (ds.Any(x => x.MaSuatChieu <= 0 || string.IsNullOrWhiteSpace(x.MaGhe) || x.GiaGhe < 0))
                throw new InvalidOperationException("Thông tin ghế đặt không hợp lệ.");

            using HttpRequestMessage request = _supabase.CreateRequest(
                HttpMethod.Post, "ghe_suat_chieu", phien.AccessToken);
            request.Headers.TryAddWithoutValidation("Prefer", "return=minimal");
            request.Content = new StringContent(JsonSerializer.Serialize(ds), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _http.SendAsync(request);
            string noiDung = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                string giaiThich = (int)response.StatusCode == 409
                    ? "Một hoặc nhiều ghế vừa được người khác đặt. Hãy tải lại sơ đồ ghế và chọn lại."
                    : $"Không thể lưu ghế (HTTP {(int)response.StatusCode}).";
                throw new InvalidOperationException($"{giaiThich}\n{noiDung}");
            }
        }
    }
}
