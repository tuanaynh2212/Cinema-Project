using System;
using System.Collections.Generic;
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
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        private HttpRequestMessage CreateRequest(HttpMethod method, string path)
        {
            var req = new HttpRequestMessage(method, $"{SupabaseUrl.TrimEnd('/')}/rest/v1/{path}");
            req.Headers.Add("apikey", SupabaseKey);
            req.Headers.Add("Authorization", $"Bearer {SupabaseKey}");
            return req;
        }

        public async Task<bool> ThemHoacCapNhatDanhSachPhimAsync(List<Phim> dsPhim)
        {
            var ds = dsPhim?.Where(p => p.TmdbId > 0)
                            .GroupBy(p => p.TmdbId)
                            .Select(g => g.First())
                            .ToList();
            if (ds == null || ds.Count == 0) return false;

            foreach (var p in ds)
                if (string.IsNullOrWhiteSpace(p.NgayChieu)) p.NgayChieu = null;

            try
            {
                var req = CreateRequest(HttpMethod.Post, $"{TableName}?on_conflict=tmdb_id");
                req.Headers.Add("Prefer", "resolution=merge-duplicates");
                req.Content = new StringContent(JsonSerializer.Serialize(ds), Encoding.UTF8, "application/json");
                var res = await _http.SendAsync(req);
                return res.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Phim>> LayTatCaPhimAsync()
        {
            try
            {
                var req = CreateRequest(HttpMethod.Get, $"{TableName}?select=*&order=tmdb_id.desc");
                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode) return new();

                var data = JsonSerializer.Deserialize<List<Phim>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
                for (int i = 0; i < data.Count; i++) data[i].ChuanHoaThongTin(i);
                return data;
            }
            catch
            {
                return new();
            }
        }
    }
}