using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL;

namespace cinema_project.BLL.Services
{
    /// <summary>
    /// Service gọi API TMDB (The Movie Database) trực tiếp - Không phụ thuộc SQL
    /// </summary>
    public class TmdbService
    {
        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(5) };
        private const string BaseApiUrl = "https://api.themoviedb.org/3";
        private const string BaseImageUrl = "https://image.tmdb.org/t/p/w500";

        // Cấu hình TMDB API Key (v3)
        public static string ApiKey { get; set; } = "e9e9d8da18ae29fc430845952232787c";

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

        /// <summary>
        /// Tự động lấy danh sách phim từ TMDB
        /// </summary>
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TMDB API Offline/Timeout]: {ex.Message}");
            }

            // Nếu không thể kết nối mạng tới TMDB, nạp danh sách phim TMDB chuẩn
            if (dsPhim.Count == 0)
            {
                dsPhim = LayDuLieuPhimTmdbChuan();
            }

            // Đánh mã phim tự tăng
            for (int i = 0; i < dsPhim.Count; i++)
            {
                dsPhim[i].MaPhim = i + 1;
            }

            return dsPhim;
        }

        private async Task<List<Phim>> LayPhimTuEndpointAsync(string apiKey, string endpoint, string loaiPhim, decimal giaVe)
        {
            var danhSach = new List<Phim>();
            if (string.IsNullOrWhiteSpace(apiKey)) return danhSach;

            string url = $"{BaseApiUrl}/{endpoint}?api_key={apiKey.Trim()}&language=vi-VN&page=1";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                url = $"{BaseApiUrl}/{endpoint}?api_key={apiKey.Trim()}&language=en-US&page=1";
                response = await _httpClient.GetAsync(url);
            }

            if (!response.IsSuccessStatusCode) return danhSach;

            string json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("results", out JsonElement resultsElement) && resultsElement.ValueKind == JsonValueKind.Array)
            {
                int dem = 0;
                foreach (JsonElement item in resultsElement.EnumerateArray())
                {
                    if (dem++ >= 8) break;

                    int id = item.GetProperty("id").GetInt32();
                    string title = item.TryGetProperty("title", out var titleProp) ? titleProp.GetString() ?? "" : "";
                    if (string.IsNullOrWhiteSpace(title) && item.TryGetProperty("original_title", out var origProp))
                    {
                        title = origProp.GetString() ?? "";
                    }

                    string overview = item.TryGetProperty("overview", out var ovProp) ? ovProp.GetString() ?? "" : "";
                    string releaseDate = item.TryGetProperty("release_date", out var rdProp) ? rdProp.GetString() ?? "" : "";
                    string posterPath = item.TryGetProperty("poster_path", out var ppProp) ? ppProp.GetString() ?? "" : "";

                    string ngayChieuFormatted = releaseDate;
                    if (DateTime.TryParse(releaseDate, out DateTime dt))
                    {
                        ngayChieuFormatted = dt.ToString("dd/MM/yyyy");
                    }

                    var theLoaiList = new List<string>();
                    if (item.TryGetProperty("genre_ids", out var genresProp) && genresProp.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var gId in genresProp.EnumerateArray())
                        {
                            int gidInt = gId.GetInt32();
                            if (_theLoaiMap.TryGetValue(gidInt, out string? tenTheLoai))
                            {
                                theLoaiList.Add(tenTheLoai);
                            }
                        }
                    }
                    string theLoai = theLoaiList.Count > 0 ? string.Join(", ", theLoaiList) : "Phim chiếu rạp";

                    bool adult = item.TryGetProperty("adult", out var adProp) && adProp.GetBoolean();
                    double vote = item.TryGetProperty("vote_average", out var vProp) ? vProp.GetDouble() : 7.0;
                    string doTuoi = adult ? "T18" : (vote > 7.5 ? "T16" : (vote > 6.0 ? "T13" : "P"));

                    string duongDanAnh = !string.IsNullOrEmpty(posterPath) ? $"{BaseImageUrl}{posterPath}" : "";

                    danhSach.Add(new Phim
                    {
                        TmdbId = id,
                        Ten = title,
                        TheLoai = theLoai,
                        DoTuoi = doTuoi,
                        ThoiLuong = "120 phút",
                        NgayChieu = ngayChieuFormatted,
                        LoaiPhim = loaiPhim,
                        GiaVe = giaVe,
                        DuongDanAnh = duongDanAnh,
                        MoTa = overview
                    });
                }
            }

            return danhSach;
        }

        private static List<Phim> LayDuLieuPhimTmdbChuan()
        {
            return new List<Phim>
            {
                // PHIM ĐANG CHIẾU
                new Phim
                {
                    TmdbId = 693134,
                    Ten = "Dune: Hành Tinh Cát - Phần Hai",
                    TheLoai = "Khoa Học Viễn Tưởng, Phiêu Lưu",
                    DoTuoi = "T16",
                    ThoiLuong = "166 phút",
                    NgayChieu = "01/03/2026",
                    LoaiPhim = "Đang chiếu",
                    GiaVe = 110000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg",
                    MoTa = "Paul Atreides hợp lực với Chani và người Fremen trên hành trình trả thù những kẻ đã hủy diệt gia tộc mình."
                },
                new Phim
                {
                    TmdbId = 1011985,
                    Ten = "Kung Fu Panda 4",
                    TheLoai = "Hoạt Hình, Hài Hước, Võ Thuật",
                    DoTuoi = "P",
                    ThoiLuong = "94 phút",
                    NgayChieu = "08/03/2026",
                    LoaiPhim = "Đang chiếu",
                    GiaVe = 85000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/kDp1vUBnMpe8ak4rjgl3cLELqjU.jpg",
                    MoTa = "Po phải tìm kiếm và huấn luyện một Thần Long Đại Hiệp mới trước khi nhận vai trò Lãnh Đạo Tinh Thần."
                },
                new Phim
                {
                    TmdbId = 823464,
                    Ten = "Godzilla x Kong: Đế Chế Mới",
                    TheLoai = "Hành Động, Viễn Tưởng, Quái Thú",
                    DoTuoi = "T13",
                    ThoiLuong = "115 phút",
                    NgayChieu = "29/03/2026",
                    LoaiPhim = "Đang chiếu",
                    GiaVe = 95000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/bQ2ywAy0z7Z5Zf222x3vE56aY9n.jpg",
                    MoTa = "Kong và Godzilla hợp lực chống lại một mối đe dọa khổng lồ chưa từng thấy ẩn sâu trong Trái Đất."
                },
                new Phim
                {
                    TmdbId = 1239192,
                    Ten = "Mai",
                    TheLoai = "Tâm Lý, Tình Cảm, Chính Kịch",
                    DoTuoi = "T18",
                    ThoiLuong = "131 phút",
                    NgayChieu = "10/02/2026",
                    LoaiPhim = "Đang chiếu",
                    GiaVe = 95000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg",
                    MoTa = "Cuộc đời đầy thăng trầm và tình yêu chất chứa của người phụ nữ tên Mai đối diện với định kiến xã hội."
                },

                // SUẤT CHIẾU ĐẶC BIỆT
                new Phim
                {
                    TmdbId = 533535,
                    Ten = "Deadpool & Wolverine (Bản IMAX 3D)",
                    TheLoai = "Hành Động, Hài Hước, Siêu Anh Hùng",
                    DoTuoi = "T18",
                    ThoiLuong = "128 phút",
                    NgayChieu = "26/07/2026",
                    LoaiPhim = "Đặc biệt",
                    GiaVe = 130000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/8cdWjvZQUExUUTzyp4t6EDMubfO.jpg",
                    MoTa = "Deadpool bị cơ quan TVA lôi kéo vào nhiệm vụ bảo vệ đa vũ trụ cùng sự đồng hành bất đắc dĩ của Wolverine."
                },
                new Phim
                {
                    TmdbId = 1022789,
                    Ten = "Inside Out 2: Những Mảnh Ghép Cảm Xúc (4DX)",
                    TheLoai = "Hoạt Hình, Gia Đình, Tâm Lý",
                    DoTuoi = "P",
                    ThoiLuong = "96 phút",
                    NgayChieu = "14/06/2026",
                    LoaiPhim = "Đặc biệt",
                    GiaVe = 125000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/vpnVM9B6NMmQpWeZvzLvDESb2QY.jpg",
                    MoTa = "Khi Riley bước vào tuổi dậy thì, trụ sở cảm xúc đón chào những thành viên mới đầy bỡ ngỡ và lo âu."
                },

                // PHIM SẮP CHIẾU
                new Phim
                {
                    TmdbId = 519182,
                    Ten = "Kẻ Trộm Mặt Trăng 4 (Despicable Me 4)",
                    TheLoai = "Hoạt Hình, Hài Hước, Gia Đình",
                    DoTuoi = "P",
                    ThoiLuong = "95 phút",
                    NgayChieu = "03/07/2026",
                    LoaiPhim = "Sắp chiếu",
                    GiaVe = 85000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/wWba3TaojhK7NdycRhoQpsG0FaH.jpg",
                    MoTa = "Gru và gia đình chào đón thành viên mới, nhưng sớm phải đối mặt với kẻ thù mới nguy hiểm."
                },
                new Phim
                {
                    TmdbId = 912649,
                    Ten = "Venom: Kèo Cuối (The Last Dance)",
                    TheLoai = "Hành Động, Viễn Tưởng, Giật Gân",
                    DoTuoi = "T16",
                    ThoiLuong = "110 phút",
                    NgayChieu = "25/10/2026",
                    LoaiPhim = "Sắp chiếu",
                    GiaVe = 95000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/aos8VhUb4tl9yAP8uqyZeaNmze1.jpg",
                    MoTa = "Eddie Brock và Venom đang trên đường chạy trốn khi bị cả hai thế giới truy đuổi gắt gao."
                },
                new Phim
                {
                    TmdbId = 1276023,
                    Ten = "Lật Mặt 7: Một Điều Ước",
                    TheLoai = "Tâm Lý, Gia Đình, Tình Cảm",
                    DoTuoi = "P",
                    ThoiLuong = "138 phút",
                    NgayChieu = "26/04/2026",
                    LoaiPhim = "Sắp chiếu",
                    GiaVe = 90000,
                    DuongDanAnh = "https://image.tmdb.org/t/p/w500/7WsyChQLEftFiDOVTGkv3hFpyyt.jpg",
                    MoTa = "Câu chuyện ấm áp tình mẫu tử về người mẹ già vùng cao và ước nguyện của 5 người con trưởng thành."
                }
            };
        }
    }
}
