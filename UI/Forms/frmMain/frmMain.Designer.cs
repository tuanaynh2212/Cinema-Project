#nullable enable
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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

            // ── Controls ──────────────────────────────────────────
            pnlHeader        = new Panel();
            lblLogo          = new Label();
            lblNavPhim       = new Label();
            lblNavRap        = new Label();
            lblNavKM         = new Label();
            lblNavUuDai      = new Label();
            txtSearch        = new TextBox();
            pnlHeaderRight   = new FlowLayoutPanel();
            btnTicket        = new Button();
            btnLogin         = new Button();
            btnRegister      = new Button();

            pnlBanner        = new Panel();
            picBanner        = new PictureBox();
            lblBannerTitle   = new Label();

            pnlTabBar        = new Panel();
            btnTabDangChieu  = new Button();
            btnTabDacBiet    = new Button();
            btnTabSapChieu   = new Button();
            cmbGenre         = new ComboBox();
            btnXemTatCa      = new Button();

            pnlCarouselOuter = new Panel();
            btnPrev          = new Button();
            btnNext          = new Button();
            pnlPosters       = new Panel();
            pnlDots          = new FlowLayoutPanel();
            picXaTrai        = new PictureBox();
            picTrai          = new PictureBox();
            picGiua          = new PictureBox();
            picPhai          = new PictureBox();
            picXaPhai        = new PictureBox();

            pnlInfo          = new Panel();
            flpTenBadge      = new FlowLayoutPanel();
            lblTenPhim       = new Label();
            lblDoTuoi        = new Label();
            lblMeta          = new Label();
            lblMoTa          = new Label();
            pnlInfoRight     = new Panel();
            btnDatVe         = new Button();
            btnTrailer       = new Button();

            pnlKhuyenMai    = new Panel();
            lblKMTitle      = new Label();
            flpKM           = new FlowLayoutPanel();

            pnlFooter       = new Panel();
            lblFooterText   = new Label();

            tmrBanner       = new System.Windows.Forms.Timer(components);

            // ─────────────────────────────────────────────────────
            SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBanner).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picXaTrai).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTrai).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picGiua).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPhai).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picXaPhai).BeginInit();

            // ════════════════════════════════════════════════════
            // FORM
            // ════════════════════════════════════════════════════
            AutoScaleMode   = AutoScaleMode.Font;
            BackColor       = C_BG;
            Font            = new Font("Segoe UI", 10f);
            MinimumSize     = new Size(1024, 680);
            Size            = new Size(1280, 800);
            StartPosition   = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview      = true;
            DoubleBuffered  = true;
            Name            = "frmMain";
            Text            = "CGV Cinemas – Đặt Vé Xem Phim";

            // ════════════════════════════════════════════════════
            // HEADER  70px
            // ════════════════════════════════════════════════════
            pnlHeader.Dock      = DockStyle.Top;
            pnlHeader.Height    = 70;
            pnlHeader.BackColor = Color.White;
            pnlHeader.Padding   = new Padding(20, 0, 20, 0);
            pnlHeader.Paint    += (s, e) =>
            {
                // bóng nhẹ dưới header
                using var pen = new Pen(Color.FromArgb(18, 0, 0, 0), 1);
                e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            lblLogo.AutoSize  = false;
            lblLogo.Size      = new Size(72, 70);
            lblLogo.Location  = new Point(0, 0);
            lblLogo.Text      = "CGV";
            lblLogo.Font      = new Font("Segoe UI", 22f, FontStyle.Bold);
            lblLogo.ForeColor = C_RED;
            lblLogo.BackColor = Color.Transparent;
            lblLogo.TextAlign = ContentAlignment.MiddleLeft;

            // Nav labels
            int navX = 86;
            foreach ((Label lbl, string txt) in new[] {
                (lblNavPhim,  "Phim"),
                (lblNavRap,   "Rạp"),
                (lblNavKM,    "Khuyến mãi"),
                (lblNavUuDai, "Ưu đãi") })
            {
                lbl.AutoSize  = true;
                lbl.Text      = txt;
                lbl.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
                lbl.ForeColor = C_SUB;
                lbl.BackColor = Color.Transparent;
                lbl.Cursor    = Cursors.Hand;
                lbl.Location  = new Point(navX, 24);
                lbl.MouseEnter += (s, _) => ((Label)s!).ForeColor = C_TEXT;
                lbl.MouseLeave += (s, _) => ((Label)s!).ForeColor = C_SUB;
                navX += lbl.PreferredWidth + 24;
            }

            txtSearch.Size            = new Size(230, 34);
            txtSearch.Font            = new Font("Segoe UI", 10f);
            txtSearch.ForeColor       = C_SUB;
            txtSearch.BackColor       = Color.FromArgb(237, 237, 242);
            txtSearch.BorderStyle     = BorderStyle.None;
            txtSearch.PlaceholderText = "Tìm phim, rạp...";
            txtSearch.TextChanged    += (_, _) => LocPhim();

            // Header right buttons
            pnlHeaderRight.AutoSize       = false;
            pnlHeaderRight.Size           = new Size(290, 70);
            pnlHeaderRight.Dock           = DockStyle.Right;
            pnlHeaderRight.BackColor      = Color.Transparent;
            pnlHeaderRight.FlowDirection  = FlowDirection.LeftToRight;
            pnlHeaderRight.WrapContents   = false;
            pnlHeaderRight.Padding        = new Padding(0, 15, 0, 0);

            StyleHeaderBtn(btnTicket, "🎫 Vé", solid: false);
            btnTicket.Size   = new Size(72, 38);
            btnTicket.Margin = new Padding(0, 0, 6, 0);
            btnTicket.Click += (s, e) => MessageBox.Show("TODO: Vé của bạn");

            StyleHeaderBtn(btnLogin, "Đăng nhập", solid: false);
            btnLogin.ForeColor = C_RED;
            btnLogin.Size      = new Size(106, 38);
            btnLogin.Margin    = new Padding(0, 0, 8, 0);
            btnLogin.FlatAppearance.BorderColor = C_RED;
            btnLogin.FlatAppearance.BorderSize  = 1;
            btnLogin.Click += (s, e) => MessageBox.Show("TODO: Đăng nhập");

            StyleHeaderBtn(btnRegister, "Đăng ký", solid: true);
            btnRegister.Size   = new Size(90, 38);

            pnlHeaderRight.Controls.Add(btnTicket);
            pnlHeaderRight.Controls.Add(btnLogin);
            pnlHeaderRight.Controls.Add(btnRegister);

            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblNavPhim);
            pnlHeader.Controls.Add(lblNavRap);
            pnlHeader.Controls.Add(lblNavKM);
            pnlHeader.Controls.Add(lblNavUuDai);
            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(pnlHeaderRight);

            pnlHeader.Resize += (s, e) =>
                txtSearch.Location = new Point((pnlHeader.Width - txtSearch.Width) / 2, 18);

            // ════════════════════════════════════════════════════
            // BANNER HERO  220px
            // ════════════════════════════════════════════════════
            pnlBanner.Dock      = DockStyle.Top;
            pnlBanner.Height    = 220;
            pnlBanner.BackColor = Color.FromArgb(40, 20, 20);
            pnlBanner.Padding   = new Padding(16, 8, 16, 0);

            picBanner.Dock      = DockStyle.Fill;
            picBanner.SizeMode  = PictureBoxSizeMode.Zoom;
            picBanner.BackColor = Color.Transparent;
            picBanner.ErrorImage = null;
            picBanner.Paint    += (s, e) =>
            {
                // gradient overlay đen mờ phía dưới
                var r = picBanner.ClientRectangle;
                var gradH = r.Height / 2;
                using var brush = new LinearGradientBrush(
                    new Point(0, r.Bottom - gradH), new Point(0, r.Bottom),
                    Color.Transparent, Color.FromArgb(170, 0, 0, 0));
                e.Graphics.FillRectangle(brush, 0, r.Bottom - gradH, r.Width, gradH);
            };

            lblBannerTitle.AutoSize  = false;
            lblBannerTitle.Anchor    = AnchorStyles.Bottom | AnchorStyles.Left;
            lblBannerTitle.Size      = new Size(600, 50);
            lblBannerTitle.Location  = new Point(12, 160);
            lblBannerTitle.Font      = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblBannerTitle.ForeColor = Color.White;
            lblBannerTitle.BackColor = Color.Transparent;
            lblBannerTitle.Text      = "";

            picBanner.Controls.Add(lblBannerTitle);
            pnlBanner.Controls.Add(picBanner);

            tmrBanner.Interval = 4000;
            tmrBanner.Tick    += TmrBanner_Tick;

            // ════════════════════════════════════════════════════
            // TAB BAR (pill tabs)  58px
            // ════════════════════════════════════════════════════
            pnlTabBar.Dock      = DockStyle.Top;
            pnlTabBar.Height    = 58;
            pnlTabBar.BackColor = C_BG;
            pnlTabBar.Padding   = new Padding(20, 10, 20, 10);

            StylePillTab(btnTabDangChieu, "Đang Chiếu", selected: true);
            btnTabDangChieu.Location = new Point(0, 0);
            btnTabDangChieu.Click   += (_, _) => SelectTab(0);

            StylePillTab(btnTabDacBiet, "Đặc Biệt", selected: false);
            btnTabDacBiet.Location = new Point(140, 0);
            btnTabDacBiet.Click   += (_, _) => SelectTab(1);

            StylePillTab(btnTabSapChieu, "Sắp Chiếu", selected: false);
            btnTabSapChieu.Location = new Point(280, 0);
            btnTabSapChieu.Click   += (_, _) => SelectTab(2);

            cmbGenre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGenre.Font          = new Font("Segoe UI", 9.5f);
            cmbGenre.FlatStyle     = FlatStyle.Flat;
            cmbGenre.Size          = new Size(160, 30);
            cmbGenre.Anchor        = AnchorStyles.Right | AnchorStyles.Top;
            cmbGenre.Items.AddRange(new object[] {
                "Tất cả thể loại", "Hành Động", "Tình Cảm", "Kinh Dị", "Hoạt Hình" });
            cmbGenre.SelectedIndex = 0;
            cmbGenre.SelectedIndexChanged += (_, _) => LocPhim();

            btnXemTatCa.Text      = "Xem tất cả →";
            btnXemTatCa.Font      = new Font("Segoe UI", 9f);
            btnXemTatCa.ForeColor = C_TEXT;
            btnXemTatCa.FlatStyle = FlatStyle.Flat;
            btnXemTatCa.FlatAppearance.BorderColor = C_BORDER;
            btnXemTatCa.BackColor = Color.White;
            btnXemTatCa.Size      = new Size(110, 30);
            btnXemTatCa.Cursor    = Cursors.Hand;
            btnXemTatCa.Anchor    = AnchorStyles.Right | AnchorStyles.Top;
            btnXemTatCa.Click    += (_, _) => ResetBoLoc();

            pnlTabBar.Controls.Add(btnTabDangChieu);
            pnlTabBar.Controls.Add(btnTabDacBiet);
            pnlTabBar.Controls.Add(btnTabSapChieu);
            pnlTabBar.Controls.Add(cmbGenre);
            pnlTabBar.Controls.Add(btnXemTatCa);

            pnlTabBar.Resize += (s, e) =>
            {
                cmbGenre.Location    = new Point(pnlTabBar.Width - 296, 14);
                btnXemTatCa.Location = new Point(pnlTabBar.Width - 126, 14);
            };

            // ════════════════════════════════════════════════════
            // CAROUSEL  290px
            // ════════════════════════════════════════════════════
            pnlCarouselOuter.Dock      = DockStyle.Top;
            pnlCarouselOuter.Height    = 310;
            pnlCarouselOuter.BackColor = C_BG;
            pnlCarouselOuter.Padding   = new Padding(0, 10, 0, 6);

            pnlPosters.Dock      = DockStyle.Fill;
            pnlPosters.BackColor = Color.Transparent;

            // Nav buttons – tròn trắng bóng
            StyleNavBtn(btnPrev, "‹");
            btnPrev.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            btnPrev.Click += (_, _) => DiChuyen(-1);

            StyleNavBtn(btnNext, "›");
            btnNext.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnNext.Click += (_, _) => DiChuyen(1);

            // Dots
            pnlDots.Dock          = DockStyle.Bottom;
            pnlDots.Height        = 20;
            pnlDots.BackColor     = Color.Transparent;
            pnlDots.FlowDirection = FlowDirection.LeftToRight;
            pnlDots.Padding       = new Padding(0, 4, 0, 0);
            pnlDots.WrapContents  = false;

            // PictureBoxes poster
            foreach ((PictureBox p, string n) in new[] {
                (picXaTrai, "picXaTrai"), (picTrai, "picTrai"), (picGiua, "picGiua"),
                (picPhai, "picPhai"),    (picXaPhai, "picXaPhai") })
            {
                p.SizeMode   = PictureBoxSizeMode.Zoom;
                p.BackColor  = Color.FromArgb(210, 210, 218);
                p.Cursor     = Cursors.Hand;
                p.ErrorImage = null;
                p.Name       = n;
            }
            picXaTrai.Click += (_, _) => DiChuyen(-2);
            picTrai.Click   += (_, _) => DiChuyen(-1);
            picPhai.Click   += (_, _) => DiChuyen(1);
            picXaPhai.Click += (_, _) => DiChuyen(2);

            pnlPosters.Controls.Add(picXaTrai);
            pnlPosters.Controls.Add(picTrai);
            pnlPosters.Controls.Add(picGiua);
            pnlPosters.Controls.Add(picPhai);
            pnlPosters.Controls.Add(picXaPhai);

            pnlCarouselOuter.Controls.Add(pnlPosters);
            pnlCarouselOuter.Controls.Add(pnlDots);
            pnlCarouselOuter.Controls.Add(btnPrev);
            pnlCarouselOuter.Controls.Add(btnNext);
            pnlCarouselOuter.Resize += (s, e) => ArrangePosters();

            // ════════════════════════════════════════════════════
            // INFO CARD  150px
            // ════════════════════════════════════════════════════
            pnlInfo.Dock      = DockStyle.Top;
            pnlInfo.Height    = 150;
            pnlInfo.BackColor = Color.White;
            pnlInfo.Padding   = new Padding(28, 14, 28, 14);
            pnlInfo.Paint    += PnlInfo_Paint;

            flpTenBadge.Dock         = DockStyle.Top;
            flpTenBadge.Height       = 42;
            flpTenBadge.BackColor    = Color.Transparent;
            flpTenBadge.WrapContents = false;
            flpTenBadge.Padding      = new Padding(0);

            lblTenPhim.AutoSize  = true;
            lblTenPhim.Text      = "Chọn phim";
            lblTenPhim.Font      = new Font("Segoe UI", 17f, FontStyle.Bold);
            lblTenPhim.ForeColor = C_TEXT;
            lblTenPhim.BackColor = Color.Transparent;
            lblTenPhim.Margin    = new Padding(0, 2, 10, 0);

            lblDoTuoi.AutoSize  = false;
            lblDoTuoi.Size      = new Size(42, 24);
            lblDoTuoi.TextAlign = ContentAlignment.MiddleCenter;
            lblDoTuoi.Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            lblDoTuoi.ForeColor = Color.White;
            lblDoTuoi.BackColor = Color.FromArgb(52, 199, 89);
            lblDoTuoi.Text      = "P";
            lblDoTuoi.Margin    = new Padding(0, 10, 0, 0);
            lblDoTuoi.Visible   = false;
            lblDoTuoi.Paint    += (s, e) =>
            {
                var lbl = (Label)s!;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedPath(new Rectangle(0, 0, lbl.Width - 1, lbl.Height - 1), lbl.Height / 2);
                using var fill = new SolidBrush(lbl.BackColor);
                e.Graphics.FillPath(fill, path);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(lbl.Text, lbl.Font, Brushes.White, new RectangleF(0, 0, lbl.Width, lbl.Height), sf);
            };

            flpTenBadge.Controls.Add(lblTenPhim);
            flpTenBadge.Controls.Add(lblDoTuoi);

            lblMeta.Dock      = DockStyle.Top;
            lblMeta.Height    = 26;
            lblMeta.Text      = "";
            lblMeta.Font      = new Font("Segoe UI", 9.5f);
            lblMeta.ForeColor = C_SUB;
            lblMeta.BackColor = Color.Transparent;

            lblMoTa.Dock      = DockStyle.Top;
            lblMoTa.Height    = 42;
            lblMoTa.Text      = "";
            lblMoTa.Font      = new Font("Segoe UI", 9.5f);
            lblMoTa.ForeColor = C_SUB;
            lblMoTa.BackColor = Color.Transparent;

            pnlInfoRight.Dock      = DockStyle.Right;
            pnlInfoRight.Width     = 230;
            pnlInfoRight.BackColor = Color.Transparent;
            pnlInfoRight.Padding   = new Padding(14, 0, 0, 0);

            btnDatVe.Text      = "ĐẶT VÉ";
            btnDatVe.Font      = new Font("Segoe UI", 12f, FontStyle.Bold);
            btnDatVe.ForeColor = Color.White;
            btnDatVe.BackColor = C_RED;
            btnDatVe.FlatStyle = FlatStyle.Flat;
            btnDatVe.FlatAppearance.BorderSize = 0;
            btnDatVe.FlatAppearance.MouseOverBackColor = C_REDHOVER;
            btnDatVe.Size      = new Size(200, 50);
            btnDatVe.Location  = new Point(0, 10);
            btnDatVe.Cursor    = Cursors.Hand;
            btnDatVe.Click    += BtnDatVe_Click;

            btnTrailer.Text     = "▶  Trailer";
            btnTrailer.Font     = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnTrailer.ForeColor = C_RED;
            btnTrailer.BackColor = Color.White;
            btnTrailer.FlatStyle = FlatStyle.Flat;
            btnTrailer.FlatAppearance.BorderColor = C_RED;
            btnTrailer.FlatAppearance.BorderSize  = 1;
            btnTrailer.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 240, 240);
            btnTrailer.Size     = new Size(200, 40);
            btnTrailer.Location = new Point(0, 70);
            btnTrailer.Cursor   = Cursors.Hand;
            btnTrailer.Click   += BtnTrailer_Click;

            pnlInfoRight.Controls.Add(btnDatVe);
            pnlInfoRight.Controls.Add(btnTrailer);

            // panel trái info
            var pnlInfoLeft = new Panel
            {
                Dock = DockStyle.Fill, BackColor = Color.Transparent
            };
            pnlInfoLeft.Controls.Add(lblMoTa);
            pnlInfoLeft.Controls.Add(lblMeta);
            pnlInfoLeft.Controls.Add(flpTenBadge);

            pnlInfo.Controls.Add(pnlInfoRight);
            pnlInfo.Controls.Add(pnlInfoLeft);

            // ════════════════════════════════════════════════════
            // KHUYẾN MÃI  200px
            // ════════════════════════════════════════════════════
            pnlKhuyenMai.Dock      = DockStyle.Top;
            pnlKhuyenMai.Height    = 210;
            pnlKhuyenMai.BackColor = C_BG;
            pnlKhuyenMai.Padding   = new Padding(24, 10, 24, 10);

            lblKMTitle.Dock      = DockStyle.Top;
            lblKMTitle.Height    = 38;
            lblKMTitle.Text      = "Khuyến mãi & Ưu đãi";
            lblKMTitle.Font      = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblKMTitle.ForeColor = C_TEXT;
            lblKMTitle.BackColor = Color.Transparent;

            flpKM.Dock           = DockStyle.Fill;
            flpKM.AutoScroll     = true;
            flpKM.FlowDirection  = FlowDirection.LeftToRight;
            flpKM.WrapContents   = false;
            flpKM.BackColor      = Color.Transparent;

            pnlKhuyenMai.Controls.Add(flpKM);
            pnlKhuyenMai.Controls.Add(lblKMTitle);

            // ════════════════════════════════════════════════════
            // FOOTER  70px
            // ════════════════════════════════════════════════════
            pnlFooter.Dock      = DockStyle.Bottom;
            pnlFooter.Height    = 70;
            pnlFooter.BackColor = Color.FromArgb(26, 26, 34);
            pnlFooter.Padding   = new Padding(24, 0, 24, 0);

            lblFooterText.Dock      = DockStyle.Fill;
            lblFooterText.Text      = "CGV Cinemas Vietnam  |  Hotline: 1900 6017  |  © 2025 CJ CGV Co., Ltd.";
            lblFooterText.Font      = new Font("Segoe UI", 9f);
            lblFooterText.ForeColor = Color.FromArgb(160, 160, 175);
            lblFooterText.BackColor = Color.Transparent;
            lblFooterText.TextAlign = ContentAlignment.MiddleCenter;

            pnlFooter.Controls.Add(lblFooterText);

            // ════════════════════════════════════════════════════
            // Thêm vào Form theo thứ tự Dock
            // ════════════════════════════════════════════════════
            Controls.Add(pnlKhuyenMai);
            Controls.Add(pnlInfo);
            Controls.Add(pnlCarouselOuter);
            Controls.Add(pnlTabBar);
            Controls.Add(pnlBanner);
            Controls.Add(pnlHeader);
            Controls.Add(pnlFooter);

            Load       += FrmMain_Load;
            KeyDown    += FrmMain_KeyDown;
            MouseWheel += FrmMain_MouseWheel;
            Resize     += (s, e) => { txtSearch.Location = new Point((pnlHeader.Width - txtSearch.Width) / 2, 18); ArrangePosters(); };

            // ─────────────────────────────────────────────────────
            ((System.ComponentModel.ISupportInitialize)picBanner).EndInit();
            ((System.ComponentModel.ISupportInitialize)picXaTrai).EndInit();
            ((System.ComponentModel.ISupportInitialize)picTrai).EndInit();
            ((System.ComponentModel.ISupportInitialize)picGiua).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPhai).EndInit();
            ((System.ComponentModel.ISupportInitialize)picXaPhai).EndInit();
            ResumeLayout(false);
        }

        // ────────────────────────────────────────────────────────
        // Color tokens (dùng trong cả 2 file partial)
        // ────────────────────────────────────────────────────────
        internal static readonly Color C_RED      = Color.FromArgb(231, 26,  15);
        internal static readonly Color C_REDHOVER = Color.FromArgb(196, 21,  11);
        internal static readonly Color C_TEXT     = Color.FromArgb( 26, 26,  34);
        internal static readonly Color C_SUB      = Color.FromArgb(107, 107, 118);
        internal static readonly Color C_BORDER   = Color.FromArgb(230, 230, 236);
        internal static readonly Color C_BG       = Color.FromArgb(247, 247, 250);

        // ────────────────────────────────────────────────────────
        // Helper: GraphicsPath bo góc
        // ────────────────────────────────────────────────────────
        internal static GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X,         r.Y,          d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y,          d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d,   0, 90);
            p.AddArc(r.X,         r.Bottom - d, d, d,  90, 90);
            p.CloseFigure();
            return p;
        }

        // ────────────────────────────────────────────────────────
        // Style helpers
        // ────────────────────────────────────────────────────────
        private static void StyleHeaderBtn(Button b, string text, bool solid)
        {
            b.Text      = text;
            b.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            b.ForeColor = solid ? Color.White : C_RED;
            b.BackColor = solid ? C_RED : Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize            = solid ? 0 : 1;
            b.FlatAppearance.MouseOverBackColor    = solid ? C_REDHOVER : Color.FromArgb(255, 242, 242);
            b.Cursor    = Cursors.Hand;
        }

        private static void StylePillTab(Button b, string text, bool selected)
        {
            b.Text      = text;
            b.Font      = new Font("Segoe UI", 10f, FontStyle.Bold);
            b.ForeColor = selected ? Color.White : Color.FromArgb(60, 60, 72);
            b.BackColor = selected ? C_RED : Color.FromArgb(237, 237, 242);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = selected ? C_REDHOVER : Color.FromArgb(222, 222, 230);
            b.Size      = new Size(128, 38);
            b.Cursor    = Cursors.Hand;
        }

        private static void StyleNavBtn(Button b, string text)
        {
            b.Text      = text;
            b.Font      = new Font("Segoe UI", 16f, FontStyle.Bold);
            b.ForeColor = C_TEXT;
            b.BackColor = Color.FromArgb(210, 255, 255, 255);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize            = 0;
            b.FlatAppearance.MouseOverBackColor    = Color.FromArgb(230, 255, 255, 255);
            b.Size      = new Size(40, 40);
            b.Cursor    = Cursors.Hand;
            // bo tròn
            b.Region = new Region(RoundedPath(new Rectangle(0, 0, 40, 40), 20));
        }

        // ────────────────────────────────────────────────────────
        // Paint info card – bóng đổ nhẹ phía trên
        // ────────────────────────────────────────────────────────
        private void PnlInfo_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(14, 0, 0, 0), 1);
            e.Graphics.DrawLine(pen, 0, 0, pnlInfo.Width, 0);
        }

        // ────────────────────────────────────────────────────────
        // Layout carousel posters & dots
        // ────────────────────────────────────────────────────────
        internal void ArrangePosters()
        {
            int W = pnlPosters.Width;
            int H = pnlPosters.Height;
            if (W <= 0 || H <= 0) return;

            int gW = 160, gH = 230;
            int sW = 125, sH = 180;
            int xW = 100, xH = 145;

            int cX = W / 2;
            int cY = H / 2;

            picGiua.Size = new Size(gW, gH);
            picGiua.Location = new Point(cX - gW / 2, cY - gH / 2);

            picTrai.Size = new Size(sW, sH);
            picTrai.Location = new Point(picGiua.Left - sW - 14, cY - sH / 2);

            picPhai.Size = new Size(sW, sH);
            picPhai.Location = new Point(picGiua.Right + 14, cY - sH / 2);

            picXaTrai.Size = new Size(xW, xH);
            picXaTrai.Location = new Point(picTrai.Left - xW - 12, cY - xH / 2);

            picXaPhai.Size = new Size(xW, xH);
            picXaPhai.Location = new Point(picPhai.Right + 12, cY - xH / 2);

            btnPrev.Location = new Point(24, Math.Max(10, (pnlCarouselOuter.Height - btnPrev.Height) / 2));
            btnNext.Location = new Point(pnlCarouselOuter.Width - btnNext.Width - 24, Math.Max(10, (pnlCarouselOuter.Height - btnNext.Height) / 2));
            btnPrev.BringToFront();
            btnNext.BringToFront();
        }

        internal void CapNhatDots(int count, int activeIdx)
        {
            pnlDots.SuspendLayout();
            pnlDots.Controls.Clear();
            if (count <= 1)
            {
                pnlDots.ResumeLayout();
                return;
            }
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
            pnlDots.ResumeLayout();
        }

        internal void NapKhuyenMai()
        {
            flpKM.Controls.Clear();
            var danhSach = new[]
            {
                ("🎁 Thứ Tư Vui Vẻ - Giá vé chỉ từ 55k", "Áp dụng mọi suất chiếu thứ 4 hàng tuần cho thành viên CGV"),
                ("🍿 Combo Bắp Nước Siêu Tiết Kiệm", "Giảm ngay 20% khi mua kèm vé trực tuyến trên ứng dụng"),
                ("💳 Ưu Đãi Thanh Toán Ngân Hàng", "Hoàn tiền tới 50.000đ khi thanh toán qua ví điện tử & thẻ"),
                ("⭐ Đặc Quyền Thành Viên CGV VIP", "Tích điểm nhân đôi và nhận quà sinh nhật hấp dẫn")
            };

            foreach (var (tieuDe, noiDung) in danhSach)
            {
                var card = new Panel
                {
                    Size = new Size(270, 140),
                    BackColor = Color.White,
                    Margin = new Padding(0, 0, 16, 0),
                    Padding = new Padding(16)
                };
                card.Paint += (s, e) =>
                {
                    using var p = new Pen(C_BORDER, 1);
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
                };

                var lblT = new Label
                {
                    Text = tieuDe,
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                    ForeColor = C_TEXT,
                    Dock = DockStyle.Top,
                    Height = 44,
                    BackColor = Color.Transparent
                };
                var lblD = new Label
                {
                    Text = noiDung,
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = C_SUB,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Transparent
                };

                card.Controls.Add(lblD);
                card.Controls.Add(lblT);
                flpKM.Controls.Add(card);
            }
        }


        // ────────────────────────────────────────────────────────
        // Khai báo controls
        // ────────────────────────────────────────────────────────
        private Panel           pnlHeader;
        private Label           lblLogo;
        private Label           lblNavPhim, lblNavRap, lblNavKM, lblNavUuDai;
        private TextBox         txtSearch;
        private FlowLayoutPanel pnlHeaderRight;
        private Button          btnTicket, btnLogin, btnRegister;

        private Panel           pnlBanner;
        private PictureBox      picBanner;
        private Label           lblBannerTitle;

        private Panel           pnlTabBar;
        private Button          btnTabDangChieu, btnTabDacBiet, btnTabSapChieu;
        private ComboBox        cmbGenre;
        private Button          btnXemTatCa;

        private Panel           pnlCarouselOuter;
        private Panel           pnlPosters;
        private Button          btnPrev, btnNext;
        private FlowLayoutPanel pnlDots;
        private PictureBox      picXaTrai, picTrai, picGiua, picPhai, picXaPhai;

        private Panel           pnlInfo;
        private FlowLayoutPanel flpTenBadge;
        private Label           lblTenPhim, lblDoTuoi, lblMeta, lblMoTa;
        private Panel           pnlInfoRight;
        private Button          btnDatVe, btnTrailer;

        private Panel           pnlKhuyenMai;
        private Label           lblKMTitle;
        private FlowLayoutPanel flpKM;

        private Panel           pnlFooter;
        private Label           lblFooterText;

        private System.Windows.Forms.Timer tmrBanner;
    }
}