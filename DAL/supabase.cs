using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public class SupabaseService
    {
        public static string SupabaseUrl { get; set; } = "https://mndebpggpzheidwteppi.supabase.co/rest/v1/";
        public static string SupabaseKey { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1uZGVicGdncHpoZWlkd3RlcHBpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3OTMwOTc5OTIsImV4cCI6MjEwODY3Mzk5Mn0.F6qYcj38GHTzLwyEYgR5wLk09IdEhOCZ1pDJKvOndY0";
        public static string TableName { get; set; } = "phim";

        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private HttpRequestMessage TaoRequest(HttpMethod method, string path)
        {
            string url = $"{SupabaseUrl.TrimEnd('/')}/rest/v1/{path}";
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("apikey", SupabaseKey);
            request.Headers.Add("Authorization", $"Bearer {SupabaseKey}");
            return request;
        }

        public async Task<bool> ThemHoacCapNhatDanhSachPhimAsync(List<Phim> dsPhim)
        {
            if (dsPhim == null || dsPhim.Count == 0) return false;
            if (string.IsNullOrWhiteSpace(SupabaseUrl) || SupabaseUrl.Contains("your-project-id")) return false;

            try
            {
                var request = TaoRequest(HttpMethod.Post, $"{TableName}?on_conflict=tmdb_id");
                request.Headers.Add("Prefer", "resolution=merge-duplicates");

                string json = JsonSerializer.Serialize(dsPhim);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode) return true;

                var retryRequest = TaoRequest(HttpMethod.Post, TableName);
                retryRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var retryResponse = await _httpClient.SendAsync(retryRequest);

                return retryResponse.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Phim>> LayTatCaPhimAsync()
        {
            var result = new List<Phim>();
            if (string.IsNullOrWhiteSpace(SupabaseUrl) || SupabaseUrl.Contains("your-project-id")) return result;

            try
            {
                var request = TaoRequest(HttpMethod.Get, $"{TableName}?select=*");
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<Phim>>(json, _jsonOptions);
                    if (data != null)
                    {
                        result = data;
                    }
                }
            }
            catch
            {
            }

            return result;
        }
    }
}
