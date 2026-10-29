using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class Phim
    {
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MaPhim { get; set; }

        [JsonPropertyName("tmdb_id")]
        public int? TmdbId { get; set; }

        [JsonPropertyName("ten")]
        public string Ten { get; set; } = string.Empty;

        [JsonPropertyName("the_loai")]
        public string TheLoai { get; set; } = string.Empty;

        [JsonPropertyName("do_tuoi")]
        public string DoTuoi { get; set; } = string.Empty;

        [JsonPropertyName("thoi_luong")]
        public string ThoiLuong { get; set; } = string.Empty;

        [JsonPropertyName("ngay_chieu")]
        public string NgayChieu { get; set; } = string.Empty;

        [JsonPropertyName("loai_phim")]
        public string LoaiPhim { get; set; } = string.Empty; 

        [JsonPropertyName("gia_ve")]
        public decimal GiaVe { get; set; } = 90000;

        [JsonPropertyName("duong_dan_anh")]
        public string DuongDanAnh { get; set; } = string.Empty; 

        [JsonPropertyName("mo_ta")]
        public string MoTa { get; set; } = string.Empty;
    }
}
