using System.Text.Json.Serialization;

namespace cinema_project.DAL.DTO
{
    public sealed class GheSuatChieuDat
    {
        [JsonPropertyName("ma_suat_chieu")]
        public int MaSuatChieu { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("ma_ghe")]
        public string? MaGhe { get; set; }

        [JsonPropertyName("gia_ghe")]
        public decimal GiaGhe { get; set; }

        [JsonPropertyName("trang_thai")]
        public string? TrangThai { get; set; }
    }
}
