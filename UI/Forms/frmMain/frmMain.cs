using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using cinema_project.BLL;
using cinema_project.DAL;

namespace cinema_project.UI
{
    public partial class frmMain : Form
    {
        private readonly PhimBLL _phimBLL;
        private List<Phim> _danhSachPhimHienTai = new();
        private static readonly HttpClient _httpClient = new();

        public frmMain()
        {
            InitializeComponent();
            _phimBLL = new PhimBLL();
        }

        private async void frmMain_Load(object? sender, EventArgs e)
        {
            KhoiTaoGiaoDien();
            cboLoaiPhim.SelectedIndex = 0; // "Tất cả"

            lblStatusTongSo.Text = "Đang nạp dữ liệu phim từ TMDB...";
            try
            {
                // Tự động gọi API TMDB lấy phim về nạp vào bộ nhớ
                await _phimBLL.NapPhimTuTmdbAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Lỗi nạp TMDB]: {ex.Message}");
            }

            // Hiển thị danh sách phim lên bảng
            TaiDanhSachPhim();
        }

        /// <summary>
        /// Khởi tạo cấu trúc cột cho DataGridView
        /// </summary>
        private void KhoiTaoGiaoDien()
        {
            dgvPhim.Columns.Clear();

            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaPhim", HeaderText = "Mã", FillWeight = 30 });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn { Name = "Ten", HeaderText = "Tên phim", FillWeight = 140 });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn { Name = "TheLoai", HeaderText = "Thể loại", FillWeight = 110 });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoaiPhim", HeaderText = "Loại", FillWeight = 60 });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn { Name = "DoTuoi", HeaderText = "Độ tuổi", FillWeight = 45 });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayChieu", HeaderText = "Khởi chiếu", FillWeight = 65 });
            dgvPhim.Columns.Add(new DataGridViewTextBoxColumn { Name = "GiaVe", HeaderText = "Giá vé (VNĐ)", FillWeight = 70 });
        }

        /// <summary>
        /// Tải danh sách phim lên bảng theo bộ lọc phân loại
        /// </summary>
        private void TaiDanhSachPhim()
        {
            string loai = cboLoaiPhim.SelectedItem?.ToString() ?? "Tất cả";
            _danhSachPhimHienTai = _phimBLL.LayPhimTheoLoai(loai);
            HienThiLenBang(_danhSachPhimHienTai);
        }

        /// <summary>
        /// Đưa danh sách phim lên DataGridView
        /// </summary>
        private void HienThiLenBang(List<Phim> ds)
        {
            dgvPhim.Rows.Clear();

            foreach (var phim in ds)
            {
                dgvPhim.Rows.Add(
                    phim.MaPhim,
                    phim.Ten,
                    phim.TheLoai,
                    phim.LoaiPhim,
                    phim.DoTuoi,
                    phim.NgayChieu,
                    phim.GiaVe.ToString("#,##0")
                );
            }

            lblStatusTongSo.Text = $"Tổng số phim: {ds.Count}";

            if (ds.Count > 0)
            {
                dgvPhim.Rows[0].Selected = true;
                CapNhatChiTiet(ds[0]);
            }
            else
            {
                XoaChiTiet();
            }
        }

        /// <summary>
        /// Cập nhật thông tin chi tiết phim sang panel bên phải
        /// </summary>
        private void CapNhatChiTiet(Phim phim)
        {
            lblTenPhim.Text = phim.Ten;
            lblTheLoai.Text = $"Thể loại: {phim.TheLoai}";
            lblThoiLuong.Text = $"Thời lượng: {phim.ThoiLuong}";
            lblNgayChieu.Text = $"Khởi chiếu: {phim.NgayChieu}";
            lblDoTuoi.Text = $"Độ tuổi: {phim.DoTuoi} | Phân loại: {phim.LoaiPhim}";
            lblGiaVe.Text = $"Giá vé: {phim.GiaVe:#,##0} VNĐ";

            TaiAnhPosterAsync(phim.DuongDanAnh);
        }

        /// <summary>
        /// Xóa thông tin chi tiết
        /// </summary>
        private void XoaChiTiet()
        {
            lblTenPhim.Text = "Không có phim";
            lblTheLoai.Text = "Thể loại: -";
            lblThoiLuong.Text = "Thời lượng: -";
            lblNgayChieu.Text = "Khởi chiếu: -";
            lblDoTuoi.Text = "Độ tuổi: -";
            lblGiaVe.Text = "Giá vé: -";

            picPoster.Image?.Dispose();
            picPoster.Image = null;
        }

        /// <summary>
        /// Tải ảnh poster từ URL TMDB hoặc thư mục nội bộ an toàn và không gây đơ giao diện
        /// </summary>
        private async void TaiAnhPosterAsync(string duongDanAnh)
        {
            picPoster.Image?.Dispose();
            picPoster.Image = null;

            if (string.IsNullOrWhiteSpace(duongDanAnh)) return;

            try
            {
                // Nếu là URL online từ TMDB
                if (duongDanAnh.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                    duongDanAnh.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] data = await _httpClient.GetByteArrayAsync(duongDanAnh);
                    using var ms = new MemoryStream(data);
                    picPoster.Image = new Bitmap(ms);
                    return;
                }

                // Nếu là file ảnh cục bộ
                string localPath = Path.Combine(AppContext.BaseDirectory, "UI", "Images", duongDanAnh);
                if (File.Exists(localPath))
                {
                    using var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    picPoster.Image = new Bitmap(stream);
                }
            }
            catch
            {
                picPoster.Image = null;
            }
        }

        /// <summary>
        /// Khi người dùng chọn dòng trên DataGridView
        /// </summary>
        private void dgvPhim_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPhim.CurrentRow != null && dgvPhim.CurrentRow.Index >= 0)
            {
                int index = dgvPhim.CurrentRow.Index;
                if (index < _danhSachPhimHienTai.Count)
                {
                    CapNhatChiTiet(_danhSachPhimHienTai[index]);
                }
            }
        }

        /// <summary>
        /// Khi thay đổi bộ lọc phân loại phim
        /// </summary>
        private void cboLoaiPhim_SelectedIndexChanged(object? sender, EventArgs e)
        {
            TaiDanhSachPhim();
        }

        /// <summary>
        /// Tìm kiếm phim theo tên hoặc thể loại
        /// </summary>
        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void txtTimKiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ThucHienTimKiem();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void ThucHienTimKiem()
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            _danhSachPhimHienTai = _phimBLL.TimKiemPhim(tuKhoa);
            HienThiLenBang(_danhSachPhimHienTai);
        }

        /// <summary>
        /// Làm mới danh sách và ô tìm kiếm
        /// </summary>
        private void btnLamMoi_Click(object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLoaiPhim.SelectedIndex = 0;
            TaiDanhSachPhim();
        }

        /// <summary>
        /// Xử lý đặt vé xem phim
        /// </summary>
        private void btnDatVe_Click(object? sender, EventArgs e)
        {
            if (dgvPhim.CurrentRow == null || dgvPhim.CurrentRow.Index < 0 || dgvPhim.CurrentRow.Index >= _danhSachPhimHienTai.Count)
            {
                MessageBox.Show("Vui lòng chọn một bộ phim từ danh sách để đặt vé!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Phim phimChon = _danhSachPhimHienTai[dgvPhim.CurrentRow.Index];

            string thongBao = $"XÁC NHẬN ĐẶT VÉ XEM PHIM\n\n" +
                              $"🎬 Tên phim: {phimChon.Ten}\n" +
                              $"⏱️ Thời lượng: {phimChon.ThoiLuong}\n" +
                              $"🔞 Độ tuổi: {phimChon.DoTuoi}\n" +
                              $"📅 Khởi chiếu: {phimChon.NgayChieu}\n" +
                              $"💵 Giá vé tiêu chuẩn: {phimChon.GiaVe:#,##0} VNĐ\n\n" +
                              $"Bạn có muốn tiếp tục tiến hành đặt vé không?";

            DialogResult ketQua = MessageBox.Show(thongBao, "Đặt vé CGV", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ketQua == DialogResult.Yes)
            {
                MessageBox.Show($"Đặt vé thành công cho phim:\n\"{phimChon.Ten}\"\n\nChúc bạn xem phim vui vẻ tại CGV!",
                                "Thông báo thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
