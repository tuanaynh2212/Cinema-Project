using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class PhongChieu
    {
        [JsonPropertyName("ma_phong")]
        public int MaPhong { get; set; }

        [JsonPropertyName("ten_phong")]
        public string TenPhong { get; set; } = "";

        public override string ToString() => TenPhong;
    }
}