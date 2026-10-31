using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private int _idx;
        private int _bannerIdx;

        public frmMain() => InitializeComponent();

        private async void frmMain_Load(object? sender, EventArgs e)
        {
            tabMain.SelectedIndex = 0;

            try { await _phimBLL.DongBoVaLayPhimAsync(); }
            catch { }

            TaiDanhSachPhim();
            HienThiBanner();
            tmrBanner.Start();
        }

        private void TaiDanhSachPhim()
        {
            string loai = tabMain.SelectedIndex switch
            {
                0 => "Đang chiếu",
                1 => "Đặc biệt",
                2 => "Sắp chiếu",
                _ => string.Empty
            };

            _danhSachPhim = string.IsNullOrEmpty(loai) ? new() : _phimBLL.LayPhimTheoLoai(loai);
            _idx = 0;
            HienThiCarousel();
        }

        private void HienThiCarousel()
        {
            PictureBox[] posters = { picXaTrai, picTrai, picGiua, picPhai, picXaPhai };

            if (_danhSachPhim.Count == 0)
            {
                lblTenPhim.Text = "Chưa có phim";
                lblDoTuoi.Visible = false;
                lblThongTin.Text = lblThuHang.Text = string.Empty;
                foreach (var p in posters) { p.Visible = false; p.ImageLocation = null; }
                return;
            }

            Phim phim = _danhSachPhim[_idx];

            lblTenPhim.Text = phim.Ten;
            lblDoTuoi.Text = phim.DoTuoi;
            lblDoTuoi.Visible = !string.IsNullOrEmpty(phim.DoTuoi);
            lblDoTuoi.BackColor = phim.DoTuoi switch
            {
                "T18" => Color.FromArgb(231, 26, 15),
                "T16" => Color.FromArgb(255, 149, 0),
                "T13" => Color.FromArgb(255, 204, 0),
                "P" => Color.FromArgb(52, 199, 89),
                _ => Color.FromArgb(142, 142, 147)
            };
            lblThongTin.Text = $"Thời lượng: {phim.ThoiLuong}  ·  Khởi chiếu: {phim.NgayChieu}";
            lblThuHang.Text = $"{_idx + 1}/{_danhSachPhim.Count}";

            for (int i = 0; i < posters.Length; i++)
            {
                int vt = _idx + i - 2;
                bool hopLe = vt >= 0 && vt < _danhSachPhim.Count;
                posters[i].Visible = hopLe;
                posters[i].ImageLocation = hopLe ? _danhSachPhim[vt].DuongDanAnh : null;
            }
        }

        private void DiChuyen(int delta)
        {
            int moi = Math.Clamp(_idx + delta, 0, Math.Max(_danhSachPhim.Count - 1, 0));
            if (moi == _idx) return;
            _idx = moi;
            HienThiCarousel();
        }

        private void btnPrev_Click(object? sender, EventArgs e) => DiChuyen(-1);
        private void btnNext_Click(object? sender, EventArgs e) => DiChuyen(1);
        private void picTrai_Click(object? sender, EventArgs e) => DiChuyen(-1);
        private void picPhai_Click(object? sender, EventArgs e) => DiChuyen(1);
        private void picXaTrai_Click(object? sender, EventArgs e) => DiChuyen(-2);
        private void picXaPhai_Click(object? sender, EventArgs e) => DiChuyen(2);
        private void picGiua_Click(object? sender, EventArgs e) { }

        private void frmMain_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Left or Keys.Right)
            {
                DiChuyen(e.KeyCode == Keys.Left ? -1 : 1);
                e.Handled = true;
            }
        }

        private void frmMain_MouseWheel(object? sender, MouseEventArgs e) => DiChuyen(e.Delta > 0 ? -1 : 1);

        private void tabMain_SelectedIndexChanged(object? sender, EventArgs e) => TaiDanhSachPhim();

        private void tabMain_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabs) return;
            Graphics g = e.Graphics;
            Rectangle bounds = tabs.GetTabRect(e.Index);
            bool chon = tabs.SelectedIndex == e.Index;
            Color mau = chon ? Color.FromArgb(231, 26, 15) : Color.FromArgb(142, 142, 147);

            g.FillRectangle(Brushes.White, bounds);

            using var font = new Font("Segoe UI", 10.5F, chon ? FontStyle.Bold : FontStyle.Regular);
            using var brush = new SolidBrush(mau);
            using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(tabs.TabPages[e.Index].Text, font, brush, bounds, sf);

            if (chon)
            {
                using var pen = new Pen(mau, 3F);
                g.DrawLine(pen, bounds.Left + 10, bounds.Bottom - 2, bounds.Right - 10, bounds.Bottom - 2);
            }
        }

        private void btnDatVe_Click(object? sender, EventArgs e)
        {
            if (_danhSachPhim.Count == 0)
            {
                MessageBox.Show("Hiện chưa có phim để đặt vé.", "CGV Cinemas");
                return;
            }

            Phim phim = _danhSachPhim[_idx];
            MessageBox.Show(
                $"Phim: {phim.Ten}\nĐộ tuổi: {phim.DoTuoi}\nThời lượng: {phim.ThoiLuong}\nNgày chiếu: {phim.NgayChieu}\nGiá vé: {phim.GiaVe:#,##0} VNĐ",
                "Đặt vé CGV");
        }

        private void btnVe_Click(object? sender, EventArgs e) => MessageBox.Show("TODO: Màn hình vé.", "CGV Cinemas");
        private void btnMenu_Click(object? sender, EventArgs e) => MessageBox.Show("TODO: Menu chức năng.", "CGV Cinemas");

        private void tmrBanner_Tick(object? sender, EventArgs e)
        {
            _bannerIdx = (_bannerIdx + 1) % _dsBanner.Length;
            HienThiBanner();
        }

        private void HienThiBanner() =>
            picBanner.ImageLocation = Path.Combine(AppContext.BaseDirectory, "UI", "Images", _dsBanner[_bannerIdx]);
    }
}