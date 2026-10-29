using System;
using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class BaoCaoDoanhThu
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("movie_id")]
        public long? MaPhim { get; set; }

        [JsonPropertyName("movie_title")]
        public string TenPhim { get; set; } = "";

        [JsonPropertyName("quantity")]
        public int SoVe { get; set; }

        [JsonPropertyName("unit_price")]
        public decimal DonGia { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal DoanhThu { get; set; }

        [JsonPropertyName("payment_status")]
        public string TrangThaiThanhToan { get; set; } = "";

        [JsonPropertyName("purchased_at")]
        public DateTime NgayMua { get; set; }
    }
}
