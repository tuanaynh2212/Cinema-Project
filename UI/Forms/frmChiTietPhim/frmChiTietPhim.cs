#nullable enable
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using cinema_project.BLL;
using cinema_project.DAL;

namespace cinema_project.UI.Forms.frmChiTietPhim
{
    public partial class frmChiTietPhim : Form
    {
        private readonly Phim _phim;
        private readonly PhimBLL _phimBLL = new();
        private string? _trailerKey;
        private string _youtubeUrl = "";

        public frmChiTietPhim(Phim phim)
        {
            _phim = phim ?? throw new ArgumentNullException(nameof(phim));
            InitializeComponent();
        }

        private async void FrmChiTietPhim_Load(object? sender, EventArgs e)
        {
            HienThiThongTinPhim();
            await TaiChiTietVaTrailerAsync();
        }

        private void HienThiThongTinPhim()
        {
            lblMovieTitle.Text = _phim.Ten;
            Text = $"{_phim.Ten} – Chi Tiết & Trailer";

            // Độ tuổi
            string doTuoi = string.IsNullOrWhiteSpace(_phim.DoTuoi) ? "P" : _phim.DoTuoi;
            lblAgeBadge.Text = doTuoi;
            lblAgeBadge.BackColor = doTuoi switch
            {
                "T18" => Color.FromArgb(231, 26, 15),
                "T16" => Color.FromArgb(255, 149, 0),
                "T13" => Color.FromArgb(255, 204, 0),
                _     => Color.FromArgb(52, 199, 89)
            };

            // Trạng thái phim
            string loaiPhim = string.IsNullOrWhiteSpace(_phim.LoaiPhim) ? "Đang chiếu" : _phim.LoaiPhim;
            lblStatusBadge.Text = loaiPhim;
            lblStatusBadge.BackColor = loaiPhim switch
            {
                "Đặc biệt" => Color.FromArgb(175, 82, 222),
                "Sắp chiếu" => Color.FromArgb(255, 149, 0),
                _          => Color.FromArgb(0, 122, 255)
            };

            // Thông tin bên cột trái
            lblGenre.Text = $"🎭 Thể loại: {(string.IsNullOrWhiteSpace(_phim.TheLoai) ? "Đang cập nhật" : _phim.TheLoai)}";
            lblReleaseDate.Text = $"📅 Ngày chiếu: {_phim.NgayChieu ?? "Đang cập nhật"}";
            lblDuration.Text = $"⏱️ Thời lượng: {_phim.ThoiLuong}";
            lblRating.Text = $"⭐ Điểm TMDB: {_phim.VoteAverage?.ToString("0.0") ?? "Chưa đánh giá"} / 10";
            lblPrice.Text = $"💵 Giá vé: {_phim.GiaVe:#,##0} VNĐ";

            // Mô tả nội dung
            txtOverview.Text = string.IsNullOrWhiteSpace(_phim.MoTa)
                ? "Mô tả của bộ phim này hiện đang được cập nhật."
                : _phim.MoTa;

            // Tải ảnh Poster
            TaiAnhPoster();
        }

        private void TaiAnhPoster()
        {
            if (string.IsNullOrWhiteSpace(_phim.DuongDanAnh)) return;

            try
            {
                if (File.Exists(_phim.DuongDanAnh))
                {
                    picPoster.ImageLocation = _phim.DuongDanAnh;
                    return;
                }

                if (Uri.TryCreate(_phim.DuongDanAnh, UriKind.Absolute, out _))
                {
                    picPoster.ImageLocation = _phim.DuongDanAnh;
                }
            }
            catch
            {
                // Giữ nền trống nếu lỗi tải ảnh
            }
        }

        private async Task TaiChiTietVaTrailerAsync()
        {
            lblVideoStatus.Text = "Đang tìm video trailer & chi tiết phim...";

            try
            {
                int targetId = _phim.TmdbId ?? _phim.MaPhim;
                var (moTa, thoiLuong, trailerKey) = await _phimBLL.LayChiTietPhimBoSungAsync(targetId, _phim.Ten);

                // Cập nhật mô tả nếu ban đầu chưa có hoặc ngắn
                if (!string.IsNullOrWhiteSpace(moTa) && (string.IsNullOrWhiteSpace(_phim.MoTa) || _phim.MoTa.Length < moTa.Length))
                {
                    _phim.MoTa = moTa;
                    txtOverview.Text = moTa;
                }

                // Cập nhật thời lượng nếu có
                if (!string.IsNullOrWhiteSpace(thoiLuong) && (_phim.ThoiLuong == "120 phút" || string.IsNullOrWhiteSpace(_phim.ThoiLuong)))
                {
                    _phim.ThoiLuong = thoiLuong;
                    lblDuration.Text = $"⏱️ Thời lượng: {_phim.ThoiLuong}";
                }

                _trailerKey = trailerKey;

                if (!string.IsNullOrEmpty(_trailerKey))
                {
                    _youtubeUrl = $"https://www.youtube.com/watch?v={_trailerKey}";
                    lblVideoStatus.Text = "Nhấn để xem Trailer chính thức trên YouTube";
                    btnPlayVideo.Enabled = true;

                    try
                    {
                        picTrailerThumbnail.ImageLocation = $"https://img.youtube.com/vi/{_trailerKey}/hqdefault.jpg";
                    }
                    catch { }
                }
                else
                {
                    string searchQuery = Uri.EscapeDataString($"{_phim.Ten} official trailer");
                    _youtubeUrl = $"https://www.youtube.com/results?search_query={searchQuery}";
                    lblVideoStatus.Text = "Nhấn để tìm kiếm trailer phim trên YouTube";
                    btnPlayVideo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblVideoStatus.Text = "Lỗi khi nạp dữ liệu: " + ex.Message;
                btnPlayVideo.Enabled = true;
            }
        }

        private void BtnPlayVideo_Click(object? sender, EventArgs e) => MoTrinhDuyetNgoai();

        private void BtnOpenBrowser_Click(object? sender, EventArgs e) => MoTrinhDuyetNgoai();

        private void MoTrinhDuyetNgoai()
        {
            string url = !string.IsNullOrEmpty(_youtubeUrl)
                ? _youtubeUrl
                : $"https://www.youtube.com/results?search_query={Uri.EscapeDataString(_phim.Ten + " trailer")}";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở trình duyệt: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnBookTicket_Click(object? sender, EventArgs e)
        {
            try
            {
                using var frm = new frmLichChieu.frmLichChieu();
                Hide();
                frm.ShowDialog();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Đặt vé cho phim: {_phim.Ten}\nGiá vé: {_phim.GiaVe:#,##0} VNĐ\nLỗi mở lịch chiếu: {ex.Message}",
                    "Đặt vé",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
