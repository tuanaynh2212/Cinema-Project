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

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(15) };
        private static readonly JsonSerializerOptions _jsonOpt = new() { PropertyNameCaseInsensitive = true };

        private HttpRequestMessage CreateRequest(HttpMethod method, string path)
        {
            string baseUrl = SupabaseUrl.TrimEnd('/');
            if (baseUrl.EndsWith("/rest/v1", StringComparison.OrdinalIgnoreCase))
                baseUrl = baseUrl[..^8].TrimEnd('/');

            var req = new HttpRequestMessage(method, $"{baseUrl}/rest/v1/{path.TrimStart('/')}");
            req.Headers.Add("apikey", SupabaseKey);
            req.Headers.Add("Authorization", $"Bearer {SupabaseKey}");
            return req;
        }

        public async Task<bool> ThemHoacCapNhatDanhSachPhimAsync(List<Phim> dsPhim)
        {
            if (dsPhim == null || dsPhim.Count == 0 || string.IsNullOrWhiteSpace(SupabaseUrl)) return false;

            try
            {
                var dsHopLe = dsPhim.Where(p => p.TmdbId > 0).GroupBy(p => p.TmdbId!.Value).Select(g => g.First()).ToList();
                if (dsHopLe.Count == 0) return false;

                foreach (var p in dsHopLe)
                    if (string.IsNullOrWhiteSpace(p.NgayChieu)) p.NgayChieu = null;

                var req = CreateRequest(HttpMethod.Post, $"{TableName}?on_conflict=tmdb_id");
                req.Headers.Add("Prefer", "resolution=merge-duplicates");
                req.Content = new StringContent(JsonSerializer.Serialize(dsHopLe), Encoding.UTF8, "application/json");

                var res = await _http.SendAsync(req);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Supabase Upsert Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Phim>> LayTatCaPhimAsync()
        {
            var result = new List<Phim>();
            if (string.IsNullOrWhiteSpace(SupabaseUrl)) return result;

            try
            {
                var req = CreateRequest(HttpMethod.Get, $"{TableName}?select=*&order=tmdb_id.desc");
                var res = await _http.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {
                    string json = await res.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<Phim>>(json, _jsonOpt);
                    if (data != null)
                    {
                        for (int i = 0; i < data.Count; i++) data[i].ChuanHoaThongTin(i);
                        result = data;
                    }
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
