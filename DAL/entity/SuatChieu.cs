using System;
using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class SuatChieu
    {
        [JsonPropertyName("ma_phong")]
        public int MaPhong { get; set; }
        [JsonPropertyName("ma_suat_chieu")]
        public int MaSuatChieu { get; set; }

        [JsonPropertyName("ma_phim")]
        public int MaPhim { get; set; }

        [JsonPropertyName("ten_phim")]
        public string TenPhim { get; set; } = "";

        [JsonPropertyName("ten_phong")]
        public string TenPhong { get; set; } = "Phòng 01 (2D Standard)";

        [JsonPropertyName("ngay_chieu")]
        public DateTime NgayChieu { get; set; } = DateTime.Today;

        [JsonPropertyName("gio_bat_dau")]
        public TimeSpan GioBatDau { get; set; } = new TimeSpan(9, 30, 0);

        [JsonPropertyName("gio_ket_thuc")]
        public TimeSpan GioKetThuc { get; set; } = new TimeSpan(11, 30, 0);

        [JsonPropertyName("dinh_dang")]
        public string DinhDang { get; set; } = "2D Phụ đề";

        [JsonPropertyName("gia_ve")]
        public decimal GiaVe { get; set; } = 90000;

        [JsonPropertyName("trang_thai")]
        public string TrangThai { get; set; } = "Sắp chiếu"; // Sắp chiếu, Đang chiếu, Hoàn tất, Đã hủy

        // Thuộc tính tiện ích hiển thị
        [JsonIgnore]
        public string KhungGio => $"{GioBatDau:hh\\:mm} - {GioKetThuc:hh\\:mm}";

        [JsonIgnore]
        public string NgayChieuFormatted => NgayChieu.ToString("dd/MM/yyyy");

        [JsonIgnore]
        public string GiaVeFormatted => $"{GiaVe:#,##0} VNĐ";
    }
}
