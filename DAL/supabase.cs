using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        private static readonly ILoggerFactory _defaultLoggerFactory = LoggerFactory.Create(b =>
        {
            b.AddConsole();
            b.SetMinimumLevel(LogLevel.Debug);
        });

        private readonly ILogger<SupabaseService> _logger;

        public SupabaseService(ILogger<SupabaseService>? logger = null)
        {
            _logger = logger ?? _defaultLoggerFactory.CreateLogger<SupabaseService>();
        }

        public HttpRequestMessage CreateRequest(HttpMethod method, string path)
        {
            ArgumentNullException.ThrowIfNull(method);
            ArgumentNullException.ThrowIfNull(path);
            if (string.IsNullOrWhiteSpace(SupabaseUrl))
                throw new InvalidOperationException("SupabaseUrl đang trống hoặc chưa được cấu hình.");

            string url = $"{SupabaseUrl.TrimEnd('/')}/rest/v1/{path.TrimStart('/')}";
            _logger.LogInformation("Request: {Method} {Url}", method.Method, url);

            var req = new HttpRequestMessage(method, url);
            req.Headers.Add("apikey", SupabaseKey);
            req.Headers.Add("Authorization", $"Bearer {SupabaseKey}");
            return req;
        }

        private async Task<List<T>> LayDanhSachAsync<T>(string path)
        {
            try
            {
                var res = await _http.SendAsync(CreateRequest(HttpMethod.Get, path));
                string body = await res.Content.ReadAsStringAsync();
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Lỗi tải {Path}: {Status} - {Body}", path, (int)res.StatusCode, body);
                    return new();
                }

                var data = JsonSerializer.Deserialize<List<T>>(body, _json) ?? new();
                _logger.LogInformation("Tải thành công {Count} bản ghi từ {Path}.", data.Count, path);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi tải {Path}: {Message}", path, ex.Message);
                return new();
            }
        }

        private async Task<bool> GuiAsync(HttpRequestMessage req, string action)
        {
            try
            {
                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode)
                    _logger.LogWarning("Lỗi {Action}: {Status} - {Body}", action, (int)res.StatusCode, await res.Content.ReadAsStringAsync());
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception khi {Action}: {Message}", action, ex.Message);
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