#nullable enable
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using cinema_project.BLL.Services;
using cinema_project.DAL;

namespace cinema_project.UI
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            BackColor = C_BG;
            Font = new Font("Segoe UI", 10f);
            MinimumSize = new Size(1024, 680);
            Size = new Size(1280, 800);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            DoubleBuffered = true;
            Name = "frmMain";
            Text = "CGV Cinemas – Đặt Vé Xem Phim";

            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.White, Padding = new Padding(20, 0, 20, 0) };
            pnlHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(18, 0, 0, 0), 1);
                e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            lblLogo = new Label
            {
                Size = new Size(72, 70), Location = new Point(0, 0), Text = "CGV",
                Font = new Font("Segoe UI", 22f, FontStyle.Bold), ForeColor = C_RED,
                BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblLogo);

            int navX = 86;
            foreach (var (lbl, txt) in new[] { (lblNavPhim = new(), "Phim"), (lblNavRap = new(), "Rạp"), (lblNavLichChieu = new(), "Lịch chiếu") })
            {
                lbl.AutoSize = true;
                lbl.Text = txt;
                lbl.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                lbl.ForeColor = C_SUB;
                lbl.BackColor = Color.Transparent;
                lbl.Cursor = Cursors.Hand;
                lbl.Location = new Point(navX, 24);
                lbl.MouseEnter += (s, _) => ((Label)s!).ForeColor = C_TEXT;
                lbl.MouseLeave += (s, _) => ((Label)s!).ForeColor = C_SUB;
                pnlHeader.Controls.Add(lbl);
                navX += lbl.PreferredWidth + 24;
            }
            lblNavLichChieu.Click += (s, e) => MoFormLichChieu();

            txtSearch = new TextBox
            {
                Size = new Size(220, 34), Font = new Font("Segoe UI", 10f), ForeColor = C_SUB,
                BackColor = Color.FromArgb(237, 237, 242), BorderStyle = BorderStyle.None,
                PlaceholderText = "Tìm phim, rạp..."
            };
            txtSearch.TextChanged += (_, _) => LocPhim();
            pnlHeader.Controls.Add(txtSearch);

            pnlHeaderRight = new FlowLayoutPanel
            {
                Size = new Size(438, 70), Dock = DockStyle.Right, BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 15, 0, 0)
            };
            btnLichChieu = CreateHeaderBtn("🎬 Lịch Chiếu", false, new Size(115, 38), new Padding(0, 0, 8, 0), (s, e) => MoFormLichChieu());
            btnTicket = CreateHeaderBtn("🎫 Vé", false, new Size(72, 38), new Padding(0, 0, 8, 0), (s, e) => MoFormVeCuaBan());
            btnLichChieu.Visible = btnTicket.Visible = false;
            btnAdmin = CreateHeaderBtn("Quản trị", false, new Size(102, 38), new Padding(0, 0, 8, 0), BtnAdmin_Click);
            btnAdmin.Visible = PhienDangNhap.NguoiDung?.LaAdmin == true;
            btnLogin = CreateHeaderBtn("Đăng nhập", true, new Size(100, 38), Padding.Empty, BtnLogin_Click);
            pnlHeaderRight.Controls.AddRange(new Control[] { btnLichChieu, btnTicket, btnAdmin, btnLogin });
            pnlHeader.Controls.Add(pnlHeaderRight);

            pnlHeader.Resize += (s, e) => txtSearch.Location = new Point((pnlHeader.Width - txtSearch.Width) / 2, 18);

            pnlBanner = new Panel { Dock = DockStyle.Top, Height = 180, BackColor = Color.FromArgb(40, 20, 20), Padding = new Padding(16, 8, 16, 0) };
            picBanner = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent, ErrorImage = null };
            picBanner.Paint += (s, e) =>
            {
                var r = picBanner.ClientRectangle;
                int gradH = r.Height / 2;
                using var brush = new LinearGradientBrush(new Point(0, r.Bottom - gradH), new Point(0, r.Bottom), Color.Transparent, Color.FromArgb(170, 0, 0, 0));
                e.Graphics.FillRectangle(brush, 0, r.Bottom - gradH, r.Width, gradH);
            };

            lblBannerTitle = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left, Size = new Size(600, 50), Location = new Point(12, 160),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, Text = ""
            };
            picBanner.Controls.Add(lblBannerTitle);
            pnlBanner.Controls.Add(picBanner);

            tmrBanner = new System.Windows.Forms.Timer(components) { Interval = 4000 };
            tmrBanner.Tick += TmrBanner_Tick;

            pnlTabBar = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = C_BG, Padding = new Padding(20, 10, 20, 10) };
            btnTabDangChieu = CreatePillTab("Đang Chiếu", 0, true, () => SelectTab(0));
            btnTabDacBiet = CreatePillTab("Đặc Biệt", 140, false, () => SelectTab(1));
            btnTabSapChieu = CreatePillTab("Sắp Chiếu", 280, false, () => SelectTab(2));

            cmbGenre = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f), FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 30), Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            cmbGenre.Items.AddRange(new object[] { "Tất cả thể loại" }
                .Concat(TmdbService.DanhSachTheLoai.Cast<object>())
                .ToArray());
            cmbGenre.SelectedIndex = 0;
            cmbGenre.SelectedIndexChanged += (_, _) => LocPhim();

            btnXemTatCa = new Button
            {
                Text = "Xem tất cả →", Font = new Font("Segoe UI", 9f), ForeColor = C_TEXT, FlatStyle = FlatStyle.Flat,
                BackColor = Color.White, Size = new Size(110, 30), Cursor = Cursors.Hand, Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnXemTatCa.FlatAppearance.BorderColor = C_BORDER;
            btnXemTatCa.Click += (_, _) => ResetBoLoc();

            pnlTabBar.Controls.AddRange(new Control[] { btnTabDangChieu, btnTabDacBiet, btnTabSapChieu, cmbGenre, btnXemTatCa });
            pnlTabBar.Resize += (s, e) =>
            {
                cmbGenre.Location = new Point(pnlTabBar.Width - 296, 14);
                btnXemTatCa.Location = new Point(pnlTabBar.Width - 126, 14);
            };

            pnlCarouselOuter = new Panel { Dock = DockStyle.Top, Height = 350, BackColor = C_BG, Padding = new Padding(0, 10, 0, 6) };
            pnlPosters = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            btnPrev = CreateNavBtn("‹", AnchorStyles.Left | AnchorStyles.Top, () => DiChuyen(-1));
            btnNext = CreateNavBtn("›", AnchorStyles.Right | AnchorStyles.Top, () => DiChuyen(1));

            pnlDots = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom, Height = 20, BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 4, 0, 0), WrapContents = false
            };

            picXaTrai = CreatePoster(-2);
            picTrai = CreatePoster(-1);
            picGiua = CreatePoster(0);
            picPhai = CreatePoster(1);
            picXaPhai = CreatePoster(2);

            pnlPosters.Controls.AddRange(new Control[] { picXaTrai, picTrai, picGiua, picPhai, picXaPhai });
            pnlCarouselOuter.Controls.AddRange(new Control[] { pnlPosters, pnlDots, btnPrev, btnNext });
            pnlCarouselOuter.Resize += (s, e) => ArrangePosters();

            pnlInfo = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(28, 14, 28, 14) };
            pnlInfo.Paint += PnlInfo_Paint;

            flpTenBadge = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, BackColor = Color.Transparent, WrapContents = false };
            lblTenPhim = new Label
            {
                AutoSize = true, Text = "Chọn phim", Font = new Font("Segoe UI", 17f, FontStyle.Bold),
                ForeColor = C_TEXT, BackColor = Color.Transparent, Margin = new Padding(0, 2, 10, 0),
                Cursor = Cursors.Hand
            };
            lblTenPhim.Click += (_, _) => MoChiTietPhim();
            lblDoTuoi = new Label
            {
                Size = new Size(42, 24), TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White, BackColor = Color.FromArgb(52, 199, 89), Text = "P", Margin = new Padding(0, 10, 0, 0), Visible = false
            };
            lblDoTuoi.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedPath(new Rectangle(0, 0, lblDoTuoi.Width - 1, lblDoTuoi.Height - 1), lblDoTuoi.Height / 2);
                using var fill = new SolidBrush(lblDoTuoi.BackColor);
                e.Graphics.FillPath(fill, path);
                e.Graphics.DrawString(lblDoTuoi.Text, lblDoTuoi.Font, Brushes.White, new RectangleF(0, 0, lblDoTuoi.Width, lblDoTuoi.Height),
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            };
            flpTenBadge.Controls.AddRange(new Control[] { lblTenPhim, lblDoTuoi });

            lblMeta = new Label { Dock = DockStyle.Top, Height = 26, Font = new Font("Segoe UI", 9.5f), ForeColor = C_SUB, BackColor = Color.Transparent };
            lblMoTa = new Label { Dock = DockStyle.Top, Height = 42, Font = new Font("Segoe UI", 9.5f), ForeColor = C_SUB, BackColor = Color.Transparent };

            pnlInfoRight = new Panel { Dock = DockStyle.Right, Width = 230, BackColor = Color.Transparent, Padding = new Padding(14, 0, 0, 0) };
            btnDatVe = new Button
            {
                Text = "ĐẶT VÉ", Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = Color.White, BackColor = C_RED,
                FlatStyle = FlatStyle.Flat, Size = new Size(200, 50), Location = new Point(0, 10), Cursor = Cursors.Hand
            };
            btnDatVe.FlatAppearance.BorderSize = 0;
            btnDatVe.FlatAppearance.MouseOverBackColor = C_REDHOVER;
            btnDatVe.Click += BtnDatVe_Click;

            btnTrailer = new Button
            {
                Text = "▶  Trailer & Chi Tiết", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = C_RED, BackColor = Color.White,
                FlatStyle = FlatStyle.Flat, Size = new Size(200, 40), Location = new Point(0, 70), Cursor = Cursors.Hand
            };
            btnTrailer.FlatAppearance.BorderColor = C_RED;
            btnTrailer.FlatAppearance.BorderSize = 1;
            btnTrailer.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 240, 240);
            btnTrailer.Click += BtnTrailer_Click;

            pnlInfoRight.Controls.AddRange(new Control[] { btnDatVe, btnTrailer });

            var pnlInfoLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            pnlInfoLeft.Controls.AddRange(new Control[] { lblMoTa, lblMeta, flpTenBadge });

            pnlInfo.Controls.AddRange(new Control[] { pnlInfoRight, pnlInfoLeft });

            pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.FromArgb(26, 26, 34), Padding = new Padding(24, 0, 24, 0) };
            lblFooterText = new Label
            {
                Dock = DockStyle.Fill, Text = "CGV Cinemas Vietnam  |  Hotline: 1900 6017  |  © 2025 CJ CGV Co., Ltd.",
                Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(160, 160, 175), BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter
            };
            pnlFooter.Controls.Add(lblFooterText);

            Controls.AddRange(new Control[] { pnlInfo, pnlCarouselOuter, pnlTabBar, pnlBanner, pnlHeader, pnlFooter });

            Load += FrmMain_Load;
            KeyDown += FrmMain_KeyDown;
            MouseWheel += FrmMain_MouseWheel;
            Resize += (s, e) => { txtSearch.Location = new Point((pnlHeader.Width - txtSearch.Width) / 2, 18); ArrangePosters(); };

            ResumeLayout(false);
        }

        internal static readonly Color C_RED      = Color.FromArgb(231, 26, 15);
        internal static readonly Color C_REDHOVER = Color.FromArgb(196, 21, 11);
        internal static readonly Color C_TEXT     = Color.FromArgb(26, 26, 34);
        internal static readonly Color C_SUB      = Color.FromArgb(107, 107, 118);
        internal static readonly Color C_BORDER   = Color.FromArgb(230, 230, 236);
        internal static readonly Color C_BG       = Color.FromArgb(247, 247, 250);

        internal static GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static Button CreateHeaderBtn(string text, bool solid, Size size, Padding margin, EventHandler? onClick)
        {
            var b = new Button
            {
                Text = text, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = solid ? Color.White : C_RED, BackColor = solid ? C_RED : Color.White,
                FlatStyle = FlatStyle.Flat, Size = size, Margin = margin, Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = solid ? 0 : 1;
            b.FlatAppearance.BorderColor = C_RED;
            b.FlatAppearance.MouseOverBackColor = solid ? C_REDHOVER : Color.FromArgb(255, 242, 242);
            if (onClick != null) b.Click += onClick;
            return b;
        }

        private Button CreatePillTab(string text, int x, bool selected, Action onClick)
        {
            var b = new Button { Location = new Point(x, 0) };
            StylePillTab(b, text, selected);
            b.Click += (_, _) => onClick();
            return b;
        }

        private Button CreateNavBtn(string text, AnchorStyles anchor, Action onClick)
        {
            var b = new Button
            {
                Text = text, Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = C_TEXT, BackColor = Color.FromArgb(210, 255, 255, 255),
                FlatStyle = FlatStyle.Flat, Size = new Size(40, 40), Cursor = Cursors.Hand, Anchor = anchor
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 255, 255, 255);
            b.Region = new Region(RoundedPath(new Rectangle(0, 0, 40, 40), 20));
            b.Click += (_, _) => onClick();
            return b;
        }

        private PictureBox CreatePoster(int delta)
        {
            var p = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.FromArgb(210, 210, 218), Cursor = Cursors.Hand, ErrorImage = null };
            if (delta != 0) p.Click += (_, _) => DiChuyen(delta);
            else p.Click += (_, _) => MoChiTietPhim();
            return p;
        }

        internal static void StylePillTab(Button b, string text, bool selected)
        {
            b.Text = text;
            b.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            b.ForeColor = selected ? Color.White : Color.FromArgb(60, 60, 72);
            b.BackColor = selected ? C_RED : Color.FromArgb(237, 237, 242);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = selected ? C_REDHOVER : Color.FromArgb(222, 222, 230);
            b.Size = new Size(128, 38);
            b.Cursor = Cursors.Hand;
        }

        private void PnlInfo_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(14, 0, 0, 0), 1);
            e.Graphics.DrawLine(pen, 0, 0, pnlInfo.Width, 0);
        }

        internal void ArrangePosters()
        {
            int W = pnlPosters.Width, H = pnlPosters.Height;
            if (W <= 0 || H <= 0) return;
            int cX = W / 2, cY = H / 2;

            picGiua.Size = new Size(190, 285);
            picGiua.Location = new Point(cX - picGiua.Width / 2, cY - picGiua.Height / 2);

            picTrai.Size = picPhai.Size = new Size(140, 210);
            picTrai.Location = new Point(picGiua.Left - picTrai.Width - 16, cY - picTrai.Height / 2);
            picPhai.Location = new Point(picGiua.Right + 16, cY - picPhai.Height / 2);

            picXaTrai.Size = picXaPhai.Size = new Size(108, 162);
            picXaTrai.Location = new Point(picTrai.Left - picXaTrai.Width - 12, cY - picXaTrai.Height / 2);
            picXaPhai.Location = new Point(picPhai.Right + 12, cY - picXaPhai.Height / 2);

            btnPrev.Location = new Point(24, Math.Max(10, (pnlCarouselOuter.Height - btnPrev.Height) / 2));
            btnNext.Location = new Point(pnlCarouselOuter.Width - btnNext.Width - 24, btnPrev.Top);
            btnPrev.BringToFront();
            btnNext.BringToFront();
        }

        internal void CapNhatDots(int count, int activeIdx)
        {
            pnlDots.SuspendLayout();
            pnlDots.Controls.Clear();
            if (count > 1)
            {
                int total = Math.Min(count, 10);
                for (int i = 0; i < total; i++)
                {
                    int dotIdx = i;
                    var dot = new Panel
                    {
                        Size = new Size(i == activeIdx ? 20 : 8, 8),
                        Margin = new Padding(3, 4, 3, 0),
                        BackColor = i == activeIdx ? C_RED : Color.FromArgb(200, 200, 210),
                        Cursor = Cursors.Hand
                    };
                    dot.Click += (s, e) => NhayDenIndex(dotIdx);
                    pnlDots.Controls.Add(dot);
                }
                pnlDots.Padding = new Padding(Math.Max(0, (pnlCarouselOuter.Width - (total * 16)) / 2), 4, 0, 0);
            }
            pnlDots.ResumeLayout();
        }

        private Panel           pnlHeader = null!, pnlBanner = null!, pnlTabBar = null!, pnlCarouselOuter = null!, pnlPosters = null!, pnlInfo = null!, pnlInfoRight = null!, pnlFooter = null!;
        private FlowLayoutPanel pnlHeaderRight = null!, pnlDots = null!, flpTenBadge = null!;
        private Label           lblLogo = null!, lblNavPhim = null!, lblNavRap = null!, lblNavLichChieu = null!, lblBannerTitle = null!, lblTenPhim = null!, lblDoTuoi = null!, lblMeta = null!, lblMoTa = null!, lblFooterText = null!;
        private TextBox         txtSearch = null!;
        private Button          btnLichChieu = null!, btnTicket = null!, btnAdmin = null!, btnLogin = null!, btnTabDangChieu = null!, btnTabDacBiet = null!, btnTabSapChieu = null!, btnXemTatCa = null!, btnPrev = null!, btnNext = null!, btnDatVe = null!, btnTrailer = null!;
        private ComboBox        cmbGenre = null!;
        private PictureBox      picBanner = null!, picXaTrai = null!, picTrai = null!, picGiua = null!, picPhai = null!, picXaPhai = null!;
        private System.Windows.Forms.Timer tmrBanner = null!;
    }
}