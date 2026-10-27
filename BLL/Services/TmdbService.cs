using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL;

namespace cinema_project.BLL.Services
{

    public class TmdbService
    {
        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(5) };
        private const string BaseApiUrl = "https://api.themoviedb.org/3";
        private const string BaseImageUrl = "https://image.tmdb.org/t/p/w500";
        public static string ApiKey { get; set; } = "93e884c92367fcd34e9a974accfb4f3b";

        private static readonly Dictionary<int, string> _theLoaiMap = new()
        {
            { 28, "Hành Động" },
            { 12, "Phiêu Lưu" },
            { 16, "Hoạt Hình" },
            { 35, "Hài Hước" },
            { 80, "Tội Phạm" },
            { 99, "Tài Liệu" },
            { 18, "Tâm Lý, Chính Kịch" },
            { 10751, "Gia Đình" },
            { 14, "Giả Tưởng" },
            { 36, "Lịch Sử" },
            { 27, "Kinh Dị" },
            { 10402, "Âm Nhạc" },
            { 9648, "Bí Ẩn" },
            { 10749, "Tình Cảm, Lãng Mạn" },
            { 878, "Khoa Học Viễn Tưởng" },
            { 53, "Giật Gân" },
            { 10752, "Chiến Tranh" },
            { 37, "Miền Tây" }
        };
        public async Task<List<Phim>> LayDanhSachPhimTuTmdbAsync()
        {
            var dsPhim = new List<Phim>();

            try
            {
                var dsDangChieu = await LayPhimTuEndpointAsync(ApiKey, "movie/now_playing", "Đang chiếu", 90000);
                var dsDacBiet = await LayPhimTuEndpointAsync(ApiKey, "movie/top_rated", "Đặc biệt", 110000);
                var dsSapChieu = await LayPhimTuEndpointAsync(ApiKey, "movie/upcoming", "Sắp chiếu", 85000);

                dsPhim.AddRange(dsDangChieu);
                dsPhim.AddRange(dsDacBiet);
                dsPhim.AddRange(dsSapChieu);
            }
            catch
            {
            }

            return dsPhim;
        }

        private async Task<List<Phim>> LayPhimTuEndpointAsync(string apiKey, string endpoint, string loaiPhim, decimal giaVe)
        {
            var danhSach = new List<Phim>();
            if (string.IsNullOrWhiteSpace(apiKey)) return danhSach;

            string url = $"{BaseApiUrl}/{endpoint}?api_key={apiKey.Trim()}&language=vi-VN&page=1";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return danhSach;

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (!doc.RootElement.TryGetProperty("results", out var results)) return danhSach;

            foreach (var item in results.EnumerateArray().Take(8))
            {
                var theLoai = item.GetProperty("genre_ids")
                    .EnumerateArray()
                    .Select(g => _theLoaiMap.GetValueOrDefault(g.GetInt32()))
                    .Where(t => t != null);

                string posterPath = item.GetProperty("poster_path").GetString() ?? "";
                string backdropPath = item.TryGetProperty("backdrop_path", out var bp) ? bp.GetString() ?? "" : "";
                double vote = item.GetProperty("vote_average").GetDouble();
                string releaseDate = item.GetProperty("release_date").GetString() ?? "";

                danhSach.Add(new Phim
                {
                    TmdbId = item.GetProperty("id").GetInt32(),
                    Ten = item.GetProperty("title").GetString() ?? "",
                    TheLoai = string.Join(", ", theLoai),
                    DoTuoi = item.GetProperty("adult").GetBoolean() ? "T18" : vote > 7.5 ? "T16" : vote > 6 ? "T13" : "P",
                    ThoiLuong = "120 phút",
                    NgayChieu = string.IsNullOrWhiteSpace(releaseDate) ? null : releaseDate,
                    LoaiPhim = loaiPhim,
                    GiaVe = giaVe,
                    DuongDanAnh = posterPath != "" ? BaseImageUrl + posterPath : "",
                    BackdropUrl = backdropPath != "" ? BaseImageUrl + backdropPath : null,
                    VoteAverage = vote,
                    MoTa = item.GetProperty("overview").GetString() ?? ""
                });
            }

            return danhSach;
        }

    }
}
