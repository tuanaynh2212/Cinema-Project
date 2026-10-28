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

        public async Task<List<string>> LayGheDaDatAsync(int maSuatChieu)
        {
            if (maSuatChieu <= 0)
                throw new ArgumentOutOfRangeException(nameof(maSuatChieu), "Mã suất chiếu không hợp lệ.");

            using HttpRequestMessage request = _supabase.CreateRequest(
                HttpMethod.Get,
                $"ghe_suat_chieu?select=ma_ghe&ma_suat_chieu=eq.{maSuatChieu}&trang_thai=eq.da_dat");

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

            List<GheSuatChieuDat> ds = danhSachGhe
                .Where(x => x != null)
                .Select(x => new GheSuatChieuDat
                {
                    MaSuatChieu = x.MaSuatChieu,
                    MaGhe = (x.MaGhe ?? "").Trim().ToUpperInvariant(),
                    GiaGhe = x.GiaGhe,
                    TrangThai = "da_dat"
                })
                .ToList();

            if (ds.Count == 0)
                throw new InvalidOperationException("Không có ghế nào được chọn.");
            if (ds.Any(x => x.MaSuatChieu <= 0 || string.IsNullOrWhiteSpace(x.MaGhe) || x.GiaGhe < 0))
                throw new InvalidOperationException("Thông tin ghế đặt không hợp lệ.");

            using HttpRequestMessage request = _supabase.CreateRequest(HttpMethod.Post, "ghe_suat_chieu");
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
