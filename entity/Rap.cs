using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class Rap
    {
        [JsonPropertyName("ma_rap")]
        public int MaRap { get; set; }

        [JsonPropertyName("ten_rap")]
        public string TenRap { get; set; } = "";

        [JsonPropertyName("dia_chi")]
        public string DiaChi { get; set; } = "";

        public override string ToString() => TenRap;
    }
}
