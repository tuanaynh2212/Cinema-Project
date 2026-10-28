using System;
using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class Phim
    {
        [JsonPropertyName("tmdb_id")] public int? TmdbId { get; set; }
        [JsonPropertyName("title")] public string Ten { get; set; } = "";
        [JsonPropertyName("overview")] public string MoTa { get; set; } = "";
        [JsonPropertyName("release_date")] public string? NgayChieu { get; set; }
        [JsonPropertyName("poster_url")] public string DuongDanAnh { get; set; } = "";
        [JsonPropertyName("backdrop_url")] public string? BackdropUrl { get; set; }
        [JsonPropertyName("vote_average")] public double? VoteAverage { get; set; }

        [JsonIgnore] public int MaPhim { get; set; }
        [JsonIgnore] public string TheLoai { get; set; } = "";
        [JsonIgnore] public string DoTuoi { get; set; } = "";
        [JsonIgnore] public string ThoiLuong { get; set; } = "120 phút";
        [JsonIgnore] public string LoaiPhim { get; set; } = "";
        [JsonIgnore] public decimal GiaVe { get; set; } = 90000;

        public void ChuanHoaThongTin(int index = 0)
        {
            double vote = VoteAverage ?? 0;

            if (string.IsNullOrWhiteSpace(DoTuoi))
                DoTuoi = vote >= 8 ? "T18" : vote >= 7 ? "T16" : vote >= 6 ? "T13" : "P";

            if (string.IsNullOrWhiteSpace(LoaiPhim))
                LoaiPhim = DateTime.TryParse(NgayChieu, out var dt) && dt > DateTime.Today ? "Sắp chiếu"
                         : vote >= 8 || index % 3 == 1 ? "Đặc biệt"
                         : index % 3 == 2 ? "Sắp chiếu" : "Đang chiếu";
        }
    }
}