using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public class SupabaseService
    {
        public static string SupabaseUrl { get; set; } = "https://mndebpggpzheidwteppi.supabase.co";
        public static string SupabaseKey { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1uZGVicGdncHpoZWlkd3RlcHBpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3OTMwOTc5OTIsImV4cCI6MjEwODY3Mzk5Mn0.F6qYcj38GHTzLwyEYgR5wLk09IdEhOCZ1pDJKvOndY0";
        public static string TableName { get; set; } = "movies";

        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(15) };
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private HttpRequestMessage TaoRequest(HttpMethod method, string path)
        {
            string baseUrl = SupabaseUrl.TrimEnd('/');
            if (baseUrl.EndsWith("/rest/v1", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl = baseUrl[..^8].TrimEnd('/');
            }

            string cleanPath = path.TrimStart('/');
            string url = $"{baseUrl}/rest/v1/{cleanPath}";

            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("apikey", SupabaseKey);
            request.Headers.Add("Authorization", $"Bearer {SupabaseKey}");
            return request;
        }

        public async Task<bool> ThemHoacCapNhatDanhSachPhimAsync(List<Phim> dsPhim)
        {
            if (dsPhim == null || dsPhim.Count == 0) return false;
            if (string.IsNullOrWhiteSpace(SupabaseUrl)) return false;

            try
            {
                var danhSachHopLe = dsPhim
                    .Where(p => p.TmdbId.HasValue && p.TmdbId.Value > 0)
                    .GroupBy(p => p.TmdbId!.Value)
                    .Select(g => g.First())
                    .ToList();

                if (danhSachHopLe.Count == 0) return false;

                foreach (var phim in danhSachHopLe)
                {
                    if (string.IsNullOrWhiteSpace(phim.NgayChieu))
                    {
                        phim.NgayChieu = null;
                    }
                }

                var request = TaoRequest(HttpMethod.Post, $"{TableName}?on_conflict=tmdb_id");
                request.Headers.Add("Prefer", "resolution=merge-duplicates");

                string json = JsonSerializer.Serialize(danhSachHopLe);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Supabase: Đã đẩy thành công {danhSachHopLe.Count} phim vào bảng '{TableName}'.");
                    return true;
                }

                string errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Supabase Upsert Thất bại ({response.StatusCode}): {errorContent}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Supabase Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Phim>> LayTatCaPhimAsync()
        {
            var result = new List<Phim>();
            if (string.IsNullOrWhiteSpace(SupabaseUrl)) return result;

            try
            {
                var request = TaoRequest(HttpMethod.Get, $"{TableName}?select=*&order=tmdb_id.desc");
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<Phim>>(json, _jsonOptions);
                    if (data != null)
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            data[i].ChuanHoaThongTin(i);
                        }
                        result = data;
                        Debug.WriteLine($"Supabase: Lấy thành công {result.Count} phim từ bảng '{TableName}'.");
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Supabase LayTatCaPhimAsync Lỗi ({response.StatusCode}): {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Supabase LayTatCaPhimAsync Exception: {ex.Message}");
            }

            return result;
        }
    }
}

