namespace cinema_project.DAL
{
    /// <summary>
    /// Đối tượng thông tin phim ánh xạ với bảng Phim trong SQL Server
    /// </summary>
    public class Phim
    {
        public int MaPhim { get; set; }
        public int? TmdbId { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string TheLoai { get; set; } = string.Empty;
        public string DoTuoi { get; set; } = string.Empty;
        public string ThoiLuong { get; set; } = string.Empty;
        public string NgayChieu { get; set; } = string.Empty;
        public string LoaiPhim { get; set; } = string.Empty; // "Đang chiếu", "Đặc biệt", "Sắp chiếu"
        public decimal GiaVe { get; set; } = 90000;
        public string DuongDanAnh { get; set; } = string.Empty; // URL TMDB hoặc đường dẫn ảnh
        public string MoTa { get; set; } = string.Empty;
    }
}
