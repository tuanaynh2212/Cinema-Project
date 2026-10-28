using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL;

namespace cinema_project.BLL.Services
{
    public class TmdbService
    {
        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(5) };
        private const string BaseApiUrl = "https://api.themoviedb.org/3";
        private const string BaseImageUrl = "https://image.tmdb.org/t/p/w500";
        public static string ApiKey { get; set; } = "93e884c92367fcd34e9a974accfb4f3b";

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
            }
            catch { }
            return dsPhim;
        }

        private async Task<List<Phim>> LayPhimTuEndpointAsync(string endpoint, string loaiPhim, decimal giaVe)
        {
            var list = new List<Phim>();
            if (string.IsNullOrWhiteSpace(ApiKey)) return list;

            var res = await _http.GetAsync($"{BaseApiUrl}/{endpoint}?api_key={ApiKey.Trim()}&language=vi-VN&page=1");
            if (!res.IsSuccessStatusCode) return list;

            using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
            if (!doc.RootElement.TryGetProperty("results", out var results)) return list;

            foreach (var item in results.EnumerateArray().Take(8))
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
    }
}
