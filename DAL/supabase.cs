using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.DAL
{
    public class SupabaseService
    {
        public static string SupabaseUrl { get; set; } = "https://mndebpggpzheidwteppi.supabase.co";
        public static string SupabaseKey { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1uZGVicGdncHpoZWlkd3RlcHBpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3OTMwOTc5OTIsImV4cCI6MjEwODY3Mzk5Mn0.F6qYcj38GHTzLwyEYgR5wLk09IdEhOCZ1pDJKvOndY0";
        public static string TableName { get; set; } = "movies";

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(15) };
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        private readonly ILogger<SupabaseService> _logger;

        public SupabaseService(ILogger<SupabaseService>? logger = null)
        {
            _logger = logger ?? AppLogging.CreateLogger<SupabaseService>();
        }

        public HttpRequestMessage CreateRequest(HttpMethod method, string path, string? accessToken = null)
        {
            ArgumentNullException.ThrowIfNull(method);
            ArgumentNullException.ThrowIfNull(path);
            if (string.IsNullOrWhiteSpace(SupabaseUrl))
                throw new InvalidOperationException("SupabaseUrl đang trống hoặc chưa được cấu hình.");

            string url = $"{SupabaseUrl.TrimEnd('/')}/rest/v1/{path.TrimStart('/')}";
            _logger.LogInformation("Request: {Method} {Url}", method.Method, url);

            var req = new HttpRequestMessage(method, url);
            req.Headers.TryAddWithoutValidation("apikey", SupabaseKey);
            string bearerToken = string.IsNullOrWhiteSpace(accessToken) ? SupabaseKey : accessToken;
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            return req;
        }

        private async Task<List<T>> LayDanhSachAsync<T>(string path)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogDebug("Bắt đầu tải dữ liệu Supabase: {Path}", path);
            try
            {
                var res = await _http.SendAsync(CreateRequest(HttpMethod.Get, path));
                string body = await res.Content.ReadAsStringAsync();
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Tải {Path} thất bại với HTTP {Status} sau {ElapsedMilliseconds} ms: {Body}",
                        path, (int)res.StatusCode, stopwatch.ElapsedMilliseconds, body);
                    return new();
                }

                var data = JsonSerializer.Deserialize<List<T>>(body, _json) ?? new();
                _logger.LogInformation(
                    "Tải thành công {Count} bản ghi từ {Path} trong {ElapsedMilliseconds} ms.",
                    data.Count, path, stopwatch.ElapsedMilliseconds);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi tải dữ liệu Supabase từ {Path} sau {ElapsedMilliseconds} ms.",
                    path, stopwatch.ElapsedMilliseconds);
                return new();
            }
        }

        private async Task<bool> GuiAsync(HttpRequestMessage req, string action)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Thao tác {Action} thất bại với HTTP {Status} sau {ElapsedMilliseconds} ms: {Body}",
                        action, (int)res.StatusCode, stopwatch.ElapsedMilliseconds,
                        await res.Content.ReadAsStringAsync());
                }
                else
                {
                    _logger.LogInformation(
                        "Thao tác {Action} thành công với HTTP {Status} trong {ElapsedMilliseconds} ms.",
                        action, (int)res.StatusCode, stopwatch.ElapsedMilliseconds);
                }
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi khi thực hiện {Action} sau {ElapsedMilliseconds} ms.",
                    action, stopwatch.ElapsedMilliseconds);
                return false;
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

            var req = CreateRequest(HttpMethod.Post, $"{TableName}?on_conflict=tmdb_id");
            req.Headers.Add("Prefer", "resolution=merge-duplicates");
            req.Content = new StringContent(JsonSerializer.Serialize(ds), Encoding.UTF8, "application/json");
            return await GuiAsync(req, $"cập nhật {ds.Count} phim");
        }

        public async Task<List<Phim>> LayTatCaPhimAsync()
        {
            var data = await LayDanhSachAsync<Phim>($"{TableName}?select=*&order=tmdb_id.desc");
            for (int i = 0; i < data.Count; i++) data[i].ChuanHoaThongTin(i);
            return data;
        }
         public async Task<HttpResponseMessage> GuiAsync(
     HttpMethod method, string path, object? body, string loi, string? accessToken = null)
        {
            if (string.IsNullOrWhiteSpace(SupabaseUrl) ||
                string.IsNullOrWhiteSpace(SupabaseKey))
            {
                _logger.LogError("Thiếu cấu hình SupabaseUrl hoặc SupabaseKey");
                throw new InvalidOperationException(
                    "Bạn cần cấu hình SupabaseUrl và SupabaseKey trong SupabaseService.");
            }

            using var request = new HttpRequestMessage(
                method, $"{SupabaseUrl.TrimEnd('/')}/{path}")
            {
                Content = body == null ? null : JsonContent.Create(body)
            };
            request.Headers.TryAddWithoutValidation("apikey", SupabaseKey);
            if (accessToken != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var stopwatch = Stopwatch.StartNew();
            _logger.LogDebug("Gửi yêu cầu Supabase {Method} {Path}", method, path);

            HttpResponseMessage response;
            try
            {
                response = await _http.SendAsync(request);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(
                    ex,
                    "Supabase {Method} {Path} hết thời gian chờ sau {ElapsedMilliseconds} ms",
                    method, path, stopwatch.ElapsedMilliseconds);
                throw new InvalidOperationException("Máy chủ phản hồi quá lâu, vui lòng thử lại.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    ex,
                    "Không kết nối được Supabase {Method} {Path} sau {ElapsedMilliseconds} ms",
                    method, path, stopwatch.ElapsedMilliseconds);
                throw new InvalidOperationException("Không kết nối được tới máy chủ. Kiểm tra mạng rồi thử lại.", ex);
            }

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug(
                    "Supabase {Method} {Path} thành công: HTTP {Status} trong {ElapsedMilliseconds} ms",
                    method, path, (int)response.StatusCode, stopwatch.ElapsedMilliseconds);
                return response;
            }

            string noiDung = await response.Content.ReadAsStringAsync();
            _logger.LogWarning(
                "Supabase {Method} {Path} thất bại: HTTP {Status} sau {ElapsedMilliseconds} ms: {Body}",
                method, path, (int)response.StatusCode, stopwatch.ElapsedMilliseconds, noiDung);
            response.Dispose();
            throw new InvalidOperationException(loi);
        }

        public Task<List<PhongChieu>> LayTatCaPhongChieuAsync() =>
            LayDanhSachAsync<PhongChieu>("phong_chieu?select=*&order=ma_phong.asc");
        public Task<List<SuatChieu>> LayTatCaSuatChieuAsync() =>
            LayDanhSachAsync<SuatChieu>("suat_chieu?select=*,phong_chieu(*),movies(*)&order=ngay_chieu.asc,gio_bat_dau.asc");

        public Task<bool> ThemSuatChieuAsync(SuatChieu sc)
        {
            var body = new
            {
                ma_phim = sc.MaPhim,
                ma_phong = sc.MaPhong,
                ngay_chieu = sc.NgayChieu.ToString("yyyy-MM-dd"),
                gio_bat_dau = sc.GioBatDau.ToString(@"hh\:mm\:ss"),
                gio_ket_thuc = sc.GioKetThuc.ToString(@"hh\:mm\:ss"),
                dinh_dang = sc.DinhDang,
                gia_ve = sc.GiaVe,
                trang_thai = sc.TrangThai
            };
            var req = CreateRequest(HttpMethod.Post, "suat_chieu");
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            return GuiAsync(req, "thêm suất chiếu");
        }
        public Task<bool> CapNhatSuatChieuAsync(SuatChieu sc)
        {
            var body = new
            {
                ma_phim = sc.MaPhim,
                ma_phong = sc.MaPhong,
                ngay_chieu = sc.NgayChieu.ToString("yyyy-MM-dd"),
                gio_bat_dau = sc.GioBatDau.ToString(@"hh\:mm\:ss"),
                gio_ket_thuc = sc.GioKetThuc.ToString(@"hh\:mm\:ss"),
                dinh_dang = sc.DinhDang,
                gia_ve = sc.GiaVe,
                trang_thai = sc.TrangThai
            };
            var req = CreateRequest(HttpMethod.Patch, $"suat_chieu?ma_suat_chieu=eq.{sc.MaSuatChieu}");
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            return GuiAsync(req, $"cập nhật suất chiếu {sc.MaSuatChieu}");
        }
        public Task<bool> XoaSuatChieuAsync(int maSuatChieu) =>
            XoaAsync("suat_chieu", "ma_suat_chieu", maSuatChieu);


        public Task<bool> XoaAsync(string table, string cot, int id) =>
            GuiAsync(CreateRequest(HttpMethod.Delete, $"{table}?{cot}=eq.{id}"), $"xóa {table} {id}");
       
    }
}
