using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cinema_project.BLL;
using cinema_project.DAL;

namespace cinema_project.UI
{
    public partial class frmMain : Form
    {
        private readonly PhimBLL _phimBLL = new();
        private readonly string[] _dsBanner = { "banner1.jpg", "banner2.jpg", "banner3.jpg" };

        private List<Phim> _danhSachPhim = new();
        private List<Phim> _danhSachGoc = new();
        private int _idx = 0;
        private int _bannerIdx = 0;
        private int _selectedTab = 0;

        public frmMain() => InitializeComponent();

        private async void FrmMain_Load(object? sender, EventArgs e)
        {
            SelectTab(0);
            HienThiBanner();
            tmrBanner.Start();

            try { await _phimBLL.DongBoVaLayPhimAsync(); } catch { }
            TaiDanhSachPhim();
        }

        private void SelectTab(int idx)
        {
            _selectedTab = idx;
            StylePillTab(btnTabDangChieu, "Đang Chiếu", idx == 0);
            StylePillTab(btnTabDacBiet,   "Đặc Biệt",   idx == 1);
            StylePillTab(btnTabSapChieu,  "Sắp Chiếu",  idx == 2);
            TaiDanhSachPhim();
        }

        private void TaiDanhSachPhim()
        {
            string loai = _selectedTab switch { 0 => "Đang chiếu", 1 => "Đặc biệt", 2 => "Sắp chiếu", _ => "" };
            _danhSachGoc = string.IsNullOrEmpty(loai) ? _phimBLL.LayTatCaPhim() : _phimBLL.LayPhimTheoLoai(loai);
            LocPhim();
        }

        internal void LocPhim()
        {
            string tuKhoa = txtSearch?.Text?.Trim().ToLowerInvariant() ?? "";
            string theLoai = cmbGenre?.SelectedItem?.ToString() ?? "Tất cả thể loại";

            _danhSachPhim = _danhSachGoc.Where(p =>
                (string.IsNullOrEmpty(tuKhoa) || (p.Ten != null && p.Ten.ToLowerInvariant().Contains(tuKhoa))) &&
                (theLoai == "Tất cả thể loại" || (p.TheLoai != null && p.TheLoai.Contains(theLoai, StringComparison.OrdinalIgnoreCase)))
            ).ToList();

            _idx = 0;
            HienThiCarousel();
        }

        internal void ResetBoLoc()
        {
            if (txtSearch != null) txtSearch.Text = "";
            if (cmbGenre != null) cmbGenre.SelectedIndex = 0;
            SelectTab(0);
        }

        private void HienThiCarousel()
        {
            PictureBox[] posters = { picXaTrai, picTrai, picGiua, picPhai, picXaPhai };

            if (_danhSachPhim.Count == 0)
            {
                lblTenPhim.Text = "Không có phim phù hợp";
                lblDoTuoi.Visible = false;
                lblMeta.Text = "";
                lblMoTa.Text = "Vui lòng chọn danh mục khác hoặc xóa từ khóa tìm kiếm.";
                foreach (var p in posters) { p.Visible = false; p.ImageLocation = null; }
                CapNhatDots(0, 0);
                return;
            }

            _idx = Math.Clamp(_idx, 0, _danhSachPhim.Count - 1);
            Phim phim = _danhSachPhim[_idx];

            lblTenPhim.Text = phim.Ten;
            lblDoTuoi.Text = string.IsNullOrEmpty(phim.DoTuoi) ? "P" : phim.DoTuoi;
            lblDoTuoi.Visible = true;
            lblDoTuoi.BackColor = phim.DoTuoi switch
            {
                "T18" => C_RED,
                "T16" => Color.FromArgb(255, 149, 0),
                "T13" => Color.FromArgb(255, 204, 0),
                _     => Color.FromArgb(52, 199, 89)
            };

            lblMeta.Text = $"⭐ {phim.VoteAverage?.ToString("0.0") ?? "8.0"}  |  ⏱️ {phim.ThoiLuong}  |  📅 {phim.NgayChieu ?? "Đang cập nhật"}";
            lblMoTa.Text = string.IsNullOrWhiteSpace(phim.MoTa) ? "Mô tả phim đang được cập nhật." : phim.MoTa;

            // Hiển thị poster theo vòng tròn, không để trống hai bên
            for (int i = 0; i < posters.Length; i++)
            {
                int vt = (_idx + i - 2) % _danhSachPhim.Count;

                // C# có thể trả về số dư âm, nên cần điều chỉnh
                if (vt < 0)
                {
                    vt += _danhSachPhim.Count;
                }

                posters[i].Visible = true;
                posters[i].ImageLocation = _danhSachPhim[vt].DuongDanAnh;
            }

            CapNhatDots(_danhSachPhim.Count, _idx);
        }

        private void DiChuyen(int delta)
        {
            if (_danhSachPhim.Count == 0) return;
            int moi = Math.Clamp(_idx + delta, 0, _danhSachPhim.Count - 1);
            if (moi != _idx)
            {
                _idx = moi;
                HienThiCarousel();
            }
        }

        internal void NhayDenIndex(int viTri)
        {
            if (viTri >= 0 && viTri < _danhSachPhim.Count && viTri != _idx)
            {
                _idx = viTri;
                HienThiCarousel();
            }
        }

        private void BtnDatVe_Click(object? sender, EventArgs e)
        {
            if (_danhSachPhim.Count == 0)
            {
                MessageBox.Show("Hiện chưa có phim để đặt vé.", "CGV Cinemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Phim phim = _danhSachPhim[_idx];
            MessageBox.Show(
                $"🎬 Phim: {phim.Ten}\n🔞 Độ tuổi: {phim.DoTuoi}\n⏱️ Thời lượng: {phim.ThoiLuong}\n📅 Ngày chiếu: {phim.NgayChieu}\n💵 Giá vé: {phim.GiaVe:#,##0} VNĐ",
                "Đặt vé CGV",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtnTrailer_Click(object? sender, EventArgs e)
        {
            if (_danhSachPhim.Count == 0) return;
            Phim phim = _danhSachPhim[_idx];
            MessageBox.Show($"Xem trailer: {phim.Ten}", "CGV Trailer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TmrBanner_Tick(object? sender, EventArgs e)
        {
            _bannerIdx = (_bannerIdx + 1) % _dsBanner.Length;
            HienThiBanner();
        }

        private void HienThiBanner()
        {
            string duongDan = Path.Combine(AppContext.BaseDirectory, "UI", "Images", _dsBanner[_bannerIdx]);
            if (File.Exists(duongDan))
            {
                picBanner.ImageLocation = duongDan;
            }
        }

        private void FrmMain_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Left or Keys.Right)
            {
                DiChuyen(e.KeyCode == Keys.Left ? -1 : 1);
                e.Handled = true;
            }
        }

        private void FrmMain_MouseWheel(object? sender, MouseEventArgs e) => DiChuyen(e.Delta > 0 ? -1 : 1);
    }
}