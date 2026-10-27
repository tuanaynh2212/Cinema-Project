using System;
using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class Phim
    {
        [JsonPropertyName("tmdb_id")]
        public int? TmdbId { get; set; }

        [JsonPropertyName("title")]
        public string Ten { get; set; } = string.Empty;

        [JsonPropertyName("overview")]
        public string MoTa { get; set; } = string.Empty;

        [JsonPropertyName("release_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NgayChieu { get; set; }

        [JsonPropertyName("poster_url")]
        public string DuongDanAnh { get; set; } = string.Empty;

        [JsonPropertyName("backdrop_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BackdropUrl { get; set; }

        [JsonPropertyName("vote_average")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? VoteAverage { get; set; }

        // Các thuộc tính phụ cho giao diện và nghiệp vụ trong app
        [JsonIgnore]
        public int MaPhim { get; set; }

        [JsonIgnore]
        public string TheLoai { get; set; } = string.Empty;

        [JsonIgnore]
        public string DoTuoi { get; set; } = string.Empty;

        [JsonIgnore]
        public string ThoiLuong { get; set; } = "120 phút";

        [JsonIgnore]
        public string LoaiPhim { get; set; } = string.Empty;

        [JsonIgnore]
        public decimal GiaVe { get; set; } = 90000;

        public void ChuanHoaThongTin(int index = 0)
        {
            if (string.IsNullOrWhiteSpace(ThoiLuong))
                ThoiLuong = "120 phút";

            if (GiaVe <= 0)
                GiaVe = 90000;

            if (string.IsNullOrWhiteSpace(DoTuoi))
            {
                double vote = VoteAverage ?? 7.0;
                DoTuoi = vote >= 8.0 ? "T18" : vote >= 7.0 ? "T16" : vote >= 6.0 ? "T13" : "P";
            }

            if (string.IsNullOrWhiteSpace(LoaiPhim))
            {
                if (DateTime.TryParse(NgayChieu, out var dt) && dt > DateTime.Today)
                {
                    LoaiPhim = "Sắp chiếu";
                }
                else if ((VoteAverage ?? 0) >= 8.0 || index % 3 == 1)
                {
                    LoaiPhim = "Đặc biệt";
                }
                else if (index % 3 == 2)
                {
                    LoaiPhim = "Sắp chiếu";
                }
                else
                {
                    LoaiPhim = "Đang chiếu";
                }
            }
        }
    }
}

