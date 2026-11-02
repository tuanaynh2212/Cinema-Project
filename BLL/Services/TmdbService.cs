using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.BLL.Services
{
    public class TmdbService
    {
        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(5) };
        private const string BaseApiUrl = "https://api.themoviedb.org/3";
        private const string BaseImageUrl = "https://image.tmdb.org/t/p/w500";
        public static string ApiKey { get; set; } = "93e884c92367fcd34e9a974accfb4f3b";

        private readonly ILogger<TmdbService> _logger;

        public TmdbService(ILogger<TmdbService>? logger = null)
        {
            _logger = logger ?? AppLogging.CreateLogger<TmdbService>();
        }

        private static readonly Dictionary<int, string> _theLoaiMap = new()
        {
            [28] = "Hành Động", [12] = "Phiêu Lưu", [16] = "Hoạt Hình", [35] = "Hài Hước",
            [80] = "Tội Phạm", [99] = "Tài Liệu", [18] = "Tâm Lý, Chính Kịch", [10751] = "Gia Đình",
            [14] = "Giả Tưởng", [36] = "Lịch Sử", [27] = "Kinh Dị", [10402] = "Âm Nhạc",
            [9648] = "Bí Ẩn", [10749] = "Tình Cảm, Lãng Mạn", [878] = "Khoa Học Viễn Tưởng",
            [53] = "Giật Gân", [10752] = "Chiến Tranh", [37] = "Miền Tây"
        };

        public static IReadOnlyList<string> DanhSachTheLoai { get; } =
            Array.AsReadOnly(_theLoaiMap.Values.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());

        private async Task<JsonElement?> GetJsonAsync(string path, string query = "")
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogDebug("Bắt đầu gọi TMDB: {Path}", path);
            try
            {
                using var response = await _http.GetAsync($"{BaseApiUrl}/{path}?api_key={ApiKey.Trim()}{query}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "TMDB {Path} trả về HTTP {Status} sau {ElapsedMilliseconds} ms",
                        path, (int)response.StatusCode, stopwatch.ElapsedMilliseconds);
                    return null;
                }
                string content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);
                _logger.LogDebug(
                    "TMDB {Path} thành công: {ContentCharacters} ký tự trong {ElapsedMilliseconds} ms",
                    path, content.Length, stopwatch.ElapsedMilliseconds);
                return doc.RootElement.Clone();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Lỗi gọi TMDB {Path} sau {ElapsedMilliseconds} ms",
                    path, stopwatch.ElapsedMilliseconds);
                return null;
            }
        }

        public async Task<List<Phim>> LayDanhSachPhimTuTmdbAsync()
        {
            var ds = new List<Phim>();
            ds.AddRange(await LayPhimTuEndpointAsync("movie/now_playing", "Đang chiếu", 90000));
            ds.AddRange(await LayPhimTuEndpointAsync("movie/top_rated", "Đặc biệt", 110000));
            ds.AddRange(await LayPhimTuEndpointAsync("movie/upcoming", "Sắp chiếu", 85000));
            _logger.LogInformation("Tải {Count} phim từ TMDB.", ds.Count);
            return ds;
        }

        public async Task<List<Phim>> LayPhimTuEndpointAsync(string endpoint, string loaiPhim, decimal giaVe)
        {
            var list = new List<Phim>();
            if (string.IsNullOrWhiteSpace(ApiKey)) return list;

            var root = await GetJsonAsync(endpoint, "&language=vi-VN&page=1");
            if (root is null || !root.Value.TryGetProperty("results", out var results)) return list;

            foreach (var item in results.EnumerateArray().Take(20))
            {
                var genres = item.GetProperty("genre_ids").EnumerateArray()
                    .Select(g => _theLoaiMap.GetValueOrDefault(g.GetInt32()))
                    .Where(t => t != null);

                string poster = item.GetProperty("poster_path").GetString() ?? "";
                string backdrop = item.TryGetProperty("backdrop_path", out var bp) ? bp.GetString() ?? "" : "";
                double vote = item.GetProperty("vote_average").GetDouble();
                string release = item.GetProperty("release_date").GetString() ?? "";

                list.Add(new Phim
                {
                    TmdbId = item.GetProperty("id").GetInt32(),
                    Ten = item.GetProperty("title").GetString() ?? "",
                    TheLoai = string.Join(", ", genres),
                    DoTuoi = item.GetProperty("adult").GetBoolean() ? "T18" : vote > 7.5 ? "T16" : vote > 6 ? "T13" : "P",
                    ThoiLuong = "120 phút",
                    NgayChieu = string.IsNullOrWhiteSpace(release) ? null : release,
                    LoaiPhim = loaiPhim,
                    GiaVe = giaVe,
                    DuongDanAnh = poster != "" ? BaseImageUrl + poster : "",
                    BackdropUrl = backdrop != "" ? BaseImageUrl + backdrop : null,
                    VoteAverage = vote,
                    MoTa = item.GetProperty("overview").GetString() ?? ""
                });
            }
            return list;
        }

        public async Task<string?> LayTrailerKeyAsync(int tmdbId, string? tenPhim = null)
        {
            var (_, _, trailerKey) = await LayChiTietPhimBoSungAsync(tmdbId, tenPhim);
            return trailerKey;
        }

        public async Task<(string? moTa, string? thoiLuong, string? trailerKey)> LayChiTietPhimBoSungAsync(int tmdbId, string? tenPhim = null)
        {
            if (string.IsNullOrWhiteSpace(ApiKey)) return (null, null, null);

            int id = tmdbId;
            if (id <= 0 && !string.IsNullOrWhiteSpace(tenPhim))
                id = await TimIdTheoTenAsync(tenPhim);
            if (id <= 0) return (null, null, null);

            string? moTa = null, thoiLuong = null;
            foreach (var lang in new[] { "vi-VN", "en-US" })
            {
                var root = await GetJsonAsync($"movie/{id}", $"&language={lang}");
                if (root is null) continue;

                if (string.IsNullOrWhiteSpace(moTa) && root.Value.TryGetProperty("overview", out var ov))
                    moTa = ov.GetString();

                if (thoiLuong is null && root.Value.TryGetProperty("runtime", out var rt)
                    && rt.ValueKind == JsonValueKind.Number && rt.GetInt32() > 0)
                    thoiLuong = $"{rt.GetInt32()} phút";

                if (!string.IsNullOrWhiteSpace(moTa)) break;
            }

            var key = await LayVideoKeyAsync(id, "&language=vi-VN") ?? await LayVideoKeyAsync(id, "");
            return (string.IsNullOrWhiteSpace(moTa) ? null : moTa, thoiLuong, key);
        }

        private async Task<int> TimIdTheoTenAsync(string ten)
        {
            var root = await GetJsonAsync("search/movie", $"&query={Uri.EscapeDataString(ten)}&language=vi-VN");
            if (root is null || !root.Value.TryGetProperty("results", out var rs) || rs.GetArrayLength() == 0) return 0;
            return rs[0].TryGetProperty("id", out var id) ? id.GetInt32() : 0;
        }

        private async Task<string?> LayVideoKeyAsync(int id, string query)
        {
            var root = await GetJsonAsync($"movie/{id}/videos", query);
            if (root is null || !root.Value.TryGetProperty("results", out var results)) return null;

            var yt = results.EnumerateArray()
                .Where(x => x.TryGetProperty("site", out var s) && s.GetString() == "YouTube")
                .ToList();
            if (yt.Count == 0) return null;

            static string Loai(JsonElement x) => x.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "";
            static bool Official(JsonElement x) => x.TryGetProperty("official", out var o) && o.ValueKind == JsonValueKind.True;

            var chosen = yt.OrderBy(x => Loai(x) == "Trailer" ? (Official(x) ? 0 : 1) : Loai(x) == "Teaser" ? 2 : 3).First();
            return chosen.TryGetProperty("key", out var k) ? k.GetString() : null;
        }
    }
}