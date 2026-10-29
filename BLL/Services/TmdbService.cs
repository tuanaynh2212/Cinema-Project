using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL;
using Microsoft.Extensions.Logging;

namespace cinema_project.BLL.Services
{
    public class TmdbService
    {
        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(5) };
        private const string BaseApiUrl = "https://api.themoviedb.org/3";
        private const string BaseImageUrl = "https://image.tmdb.org/t/p/w500";
        public static string ApiKey { get; set; } = "93e884c92367fcd34e9a974accfb4f3b";

        private static readonly ILoggerFactory _defaultLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        private readonly ILogger<TmdbService> _logger;

        public TmdbService(ILogger<TmdbService>? logger = null)
        {
            _logger = logger ?? _defaultLoggerFactory.CreateLogger<TmdbService>();
        }

        private static readonly Dictionary<int, string> _theLoaiMap = new()
        {
            [28] = "Hành Động", [12] = "Phiêu Lưu", [16] = "Hoạt Hình", [35] = "Hài Hước",
            [80] = "Tội Phạm", [99] = "Tài Liệu", [18] = "Tâm Lý, Chính Kịch", [10751] = "Gia Đình",
            [14] = "Giả Tưởng", [36] = "Lịch Sử", [27] = "Kinh Dị", [10402] = "Âm Nhạc",
            [9648] = "Bí Ẩn", [10749] = "Tình Cảm, Lãng Mạn", [878] = "Khoa Học Viễn Tưởng",
            [53] = "Giật Gân", [10752] = "Chiến Tranh", [37] = "Miền Tây"
        };

        public async Task<List<Phim>> LayDanhSachPhimTuTmdbAsync()
        {
            var dsPhim = new List<Phim>();
            try
            {
                dsPhim.AddRange(await LayPhimTuEndpointAsync("movie/now_playing", "Đang chiếu", 90000));
                dsPhim.AddRange(await LayPhimTuEndpointAsync("movie/top_rated", "Đặc biệt", 110000));
                dsPhim.AddRange(await LayPhimTuEndpointAsync("movie/upcoming", "Sắp chiếu", 85000));
                _logger.LogInformation("Tải thành công {Count} phim từ TMDB.", dsPhim.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách phim từ TMDB: {Message}", ex.Message);
            }
            return dsPhim;
        }

        public async Task<List<Phim>> LayPhimTuEndpointAsync(string endpoint, string loaiPhim, decimal giaVe)
        {
            try
            {
                var list = new List<Phim>();
                if (string.IsNullOrWhiteSpace(ApiKey))
                {
                    _logger.LogError("API key is null or empty.");
                    return list;
                }

                var res = await _http.GetAsync($"{BaseApiUrl}/{endpoint}?api_key={ApiKey.Trim()}&language=vi-VN&page=1");
                if (!res.IsSuccessStatusCode)
                {
                    _logger.LogError("Lỗi API TMDB: {StatusCode}", res.StatusCode);
                    throw new Exception($"Lỗi API TMDB: {res.StatusCode}");
                }

                using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
                if (!doc.RootElement.TryGetProperty("results", out var results))
                {
                    _logger.LogError("Không có results trong JSON response");
                    return list;
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LayPhimTuEndpointAsync: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<string?> LayTrailerKeyAsync(int tmdbId, string? tenPhim = null)
        {
            var (_, _, trailerKey) = await LayChiTietPhimBoSungAsync(tmdbId, tenPhim);
            return trailerKey;
        }

        public async Task<(string? moTa, string? thoiLuong, string? trailerKey)> LayChiTietPhimBoSungAsync(int tmdbId, string? tenPhim = null)
        {
            string? moTa = null;
            string? thoiLuong = null;
            string? trailerKey = null;

            if (string.IsNullOrWhiteSpace(ApiKey))
                return (moTa, thoiLuong, trailerKey);

            int targetId = tmdbId;

            // Nếu tmdbId <= 0 và có tên phim, tìm kiếm ID theo tên phim
            if (targetId <= 0 && !string.IsNullOrWhiteSpace(tenPhim))
            {
                try
                {
                    string searchUrl = $"{BaseApiUrl}/search/movie?api_key={ApiKey.Trim()}&query={Uri.EscapeDataString(tenPhim)}&language=vi-VN";
                    var searchRes = await _http.GetAsync(searchUrl);
                    if (searchRes.IsSuccessStatusCode)
                    {
                        using var sDoc = JsonDocument.Parse(await searchRes.Content.ReadAsStringAsync());
                        if (sDoc.RootElement.TryGetProperty("results", out var sResults) && sResults.GetArrayLength() > 0)
                        {
                            var first = sResults[0];
                            if (first.TryGetProperty("id", out var idProp))
                            {
                                targetId = idProp.GetInt32();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Lỗi tìm kiếm phim theo tên '{TenPhim}': {Message}", tenPhim, ex.Message);
                }
            }

            if (targetId <= 0)
                return (moTa, thoiLuong, trailerKey);

            // 1. Lấy thông tin chi tiết phim (Overview, Runtime)
            try
            {
                // Thử vi-VN trước
                string detailUrlVi = $"{BaseApiUrl}/movie/{targetId}?api_key={ApiKey.Trim()}&language=vi-VN";
                var detResVi = await _http.GetAsync(detailUrlVi);
                if (detResVi.IsSuccessStatusCode)
                {
                    using var dDoc = JsonDocument.Parse(await detResVi.Content.ReadAsStringAsync());
                    var root = dDoc.RootElement;
                    if (root.TryGetProperty("overview", out var ov) && !string.IsNullOrWhiteSpace(ov.GetString()))
                    {
                        moTa = ov.GetString();
                    }
                    if (root.TryGetProperty("runtime", out var rt) && rt.GetInt32() > 0)
                    {
                        thoiLuong = $"{rt.GetInt32()} phút";
                    }
                }

                // Nếu overview vi-VN bị trống, fallback sang en-US
                if (string.IsNullOrWhiteSpace(moTa))
                {
                    string detailUrlEn = $"{BaseApiUrl}/movie/{targetId}?api_key={ApiKey.Trim()}&language=en-US";
                    var detResEn = await _http.GetAsync(detailUrlEn);
                    if (detResEn.IsSuccessStatusCode)
                    {
                        using var dDocEn = JsonDocument.Parse(await detResEn.Content.ReadAsStringAsync());
                        var rootEn = dDocEn.RootElement;
                        if (rootEn.TryGetProperty("overview", out var ovEn) && !string.IsNullOrWhiteSpace(ovEn.GetString()))
                        {
                            moTa = ovEn.GetString();
                        }
                        if (string.IsNullOrWhiteSpace(thoiLuong) && rootEn.TryGetProperty("runtime", out var rtEn) && rtEn.GetInt32() > 0)
                        {
                            thoiLuong = $"{rtEn.GetInt32()} phút";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Lỗi lấy chi tiết movie ID {Id}: {Message}", targetId, ex.Message);
            }

            // 2. Lấy video trailer (ưu tiên vi-VN, sau đó fallback sang mặc định/en-US)
            trailerKey = await LayTrailerKeyTuTmdbIdAsync(targetId);

            return (moTa, thoiLuong, trailerKey);
        }

        private async Task<string?> LayTrailerKeyTuTmdbIdAsync(int tmdbId)
        {
            // Thử 1: vi-VN
            string? key = await QueryVideosEndpointAsync($"{BaseApiUrl}/movie/{tmdbId}/videos?api_key={ApiKey.Trim()}&language=vi-VN");
            if (!string.IsNullOrEmpty(key)) return key;

            // Thử 2: mặc định (không giới hạn ngôn ngữ, hầu hết trailer quốc tế/official đều nằm ở đây)
            key = await QueryVideosEndpointAsync($"{BaseApiUrl}/movie/{tmdbId}/videos?api_key={ApiKey.Trim()}");
            return key;
        }

        private async Task<string?> QueryVideosEndpointAsync(string url)
        {
            try
            {
                var res = await _http.GetAsync(url);
                if (!res.IsSuccessStatusCode) return null;

                using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
                if (!doc.RootElement.TryGetProperty("results", out var results)) return null;

                var ytVideos = results.EnumerateArray()
                    .Where(x => x.TryGetProperty("site", out var s) && s.GetString()?.Equals("YouTube", StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();

                if (ytVideos.Count == 0) return null;

                var chosen = ytVideos.FirstOrDefault(x =>
                    x.TryGetProperty("type", out var t) && t.GetString()?.Equals("Trailer", StringComparison.OrdinalIgnoreCase) == true
                    && x.TryGetProperty("official", out var o) && o.GetBoolean());

                if (chosen.ValueKind == JsonValueKind.Undefined)
                {
                    chosen = ytVideos.FirstOrDefault(x =>
                        x.TryGetProperty("type", out var t) && t.GetString()?.Equals("Trailer", StringComparison.OrdinalIgnoreCase) == true);
                }

                if (chosen.ValueKind == JsonValueKind.Undefined)
                {
                    chosen = ytVideos.FirstOrDefault(x =>
                        x.TryGetProperty("type", out var t) && t.GetString()?.Equals("Teaser", StringComparison.OrdinalIgnoreCase) == true);
                }

                if (chosen.ValueKind == JsonValueKind.Undefined)
                {
                    chosen = ytVideos.FirstOrDefault();
                }

                if (chosen.ValueKind != JsonValueKind.Undefined && chosen.TryGetProperty("key", out var k))
                {
                    return k.GetString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Lỗi khi gọi API videos từ TMDB: {Message}", ex.Message);
            }

            return null;
        }
    }
}
