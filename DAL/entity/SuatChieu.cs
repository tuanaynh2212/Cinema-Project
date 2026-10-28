using System;
using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class SuatChieu
    {
        [JsonPropertyName("ma_suat_chieu")]
        public int MaSuatChieu { get; set; }

        [JsonPropertyName("ma_phong")]
        public int MaPhong { get; set; }

        [JsonPropertyName("ma_phim")]
        public int MaPhim { get; set; }

        [JsonPropertyName("movies")]
        public Phim? PhimThongTin { get; set; }

        [JsonPropertyName("phong_chieu")]
        public PhongChieu? PhongChieuThongTin { get; set; }

        private string _tenPhim = "";
        [JsonPropertyName("ten_phim")]
        public string TenPhim
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_tenPhim)) return _tenPhim;
                if (PhimThongTin != null && !string.IsNullOrWhiteSpace(PhimThongTin.Ten)) return PhimThongTin.Ten;
                return "";
            }
            set => _tenPhim = value;
        }

        private string _tenPhong = "";
        [JsonPropertyName("ten_phong")]
        public string TenPhong
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_tenPhong)) return _tenPhong;
                if (PhongChieuThongTin != null && !string.IsNullOrWhiteSpace(PhongChieuThongTin.TenPhong)) return PhongChieuThongTin.TenPhong;
                return MaPhong > 0 ? $"Phòng {MaPhong:D2}" : "";
            }
            set => _tenPhong = value;
        }

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
