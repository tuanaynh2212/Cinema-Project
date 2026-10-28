using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace cinema_project.DAL
{
    public class SupabaseService
    {
        public static string SupabaseUrl { get; set; } = "https://mndebpggpzheidwteppi.supabase.co";
        public static string SupabaseKey { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1uZGVicGdncHpoZWlkd3RlcHBpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3OTMwOTc5OTIsImV4cCI6MjEwODY3Mzk5Mn0.F6qYcj38GHTzLwyEYgR5wLk09IdEhOCZ1pDJKvOndY0";
        public static string TableName { get; set; } = "movies";

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(15) };
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        private static readonly ILoggerFactory _defaultLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        private readonly ILogger<SupabaseService> _logger;

        public SupabaseService(ILogger<SupabaseService>? logger = null)
        {
            _logger = logger ?? _defaultLoggerFactory.CreateLogger<SupabaseService>();
        }

        public HttpRequestMessage CreateRequest(HttpMethod method, string path)
        {
            if (method == null)
            {
                var ex = new ArgumentNullException(nameof(method), "HttpMethod không được null.");
                _logger.LogError(ex, "CreateRequest thất bại: Tham số 'method' không được rỗng (null).");
                throw ex;
            }

            if (path == null)
            {
                var ex = new ArgumentNullException(nameof(path), "Path không được null.");
                _logger.LogError(ex, "CreateRequest thất bại: Tham số 'path' không được rỗng (null).");
                throw ex;
            }

            if (string.IsNullOrWhiteSpace(SupabaseUrl))
            {
                var ex = new InvalidOperationException("SupabaseUrl đang trống hoặc chưa được cấu hình.");
                _logger.LogError(ex, "CreateRequest thất bại: Cấu hình SupabaseUrl không hợp lệ.");
                throw ex;
            }

            try
            {
                // Chuẩn hóa path tránh lỗi double-slash nếu path có dấu / ở đầu
                string cleanPath = path.TrimStart('/');
                string url = $"{SupabaseUrl.TrimEnd('/')}/rest/v1/{cleanPath}";

                _logger.LogInformation("Đang tạo HttpRequestMessage: Method = {Method}, URL = {Url}", method.Method, url);

                var req = new HttpRequestMessage(method, url);
                req.Headers.Add("apikey", SupabaseKey);
                req.Headers.Add("Authorization", $"Bearer {SupabaseKey}");

                _logger.LogDebug("Headers đã được thiết lập thành công: apikey, Authorization (Bearer)");

                return req;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi ngoại lệ khi tạo HttpRequestMessage: {Message}", ex.Message);
                throw;
            }
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
                if (res.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Cập nhật thành công {Count} phim lên Supabase.", ds.Count);
                }
                else
                {
                    _logger.LogWarning("Lỗi cập nhật phim. Mã phản hồi: {StatusCode} ({ReasonPhrase})", (int)res.StatusCode, res.ReasonPhrase);
                }
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi gửi request cập nhật phim: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<List<Phim>> LayTatCaPhimAsync()
        {
            try
            {
                var req = CreateRequest(HttpMethod.Get, $"{TableName}?select=*&order=tmdb_id.desc");
                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Lỗi tải danh sách phim. Mã phản hồi: {StatusCode} ({ReasonPhrase})", (int)res.StatusCode, res.ReasonPhrase);
                    return new();
                }

                var data = JsonSerializer.Deserialize<List<Phim>>(await res.Content.ReadAsStringAsync(), _json) ?? new();
                for (int i = 0; i < data.Count; i++) data[i].ChuanHoaThongTin(i);
                _logger.LogInformation("Tải thành công {Count} phim từ Supabase.", data.Count);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi tải phim từ Supabase: {Message}", ex.Message);
                return new();
            }
        }
    }
}