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

            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = Color.White, Padding = new Padding(24, 0, 24, 0) };
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
            lblNavRap.Visible = false;
            lblNavLichChieu.Visible = false;
            lblNavLichChieu.Click += (s, e) => MoFormLichChieu();

            txtSearch = new TextBox
            {
                Size = new Size(220, 34), Font = new Font("Segoe UI", 10f), ForeColor = C_SUB,
                BackColor = Color.FromArgb(247, 246, 248), BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Tìm phim, rạp..."
            };
            txtSearch.TextChanged += (_, _) => LocPhim();
            pnlHeader.Controls.Add(txtSearch);

            pnlHeaderRight = new FlowLayoutPanel
            {
                Size = new Size(438, 70), Dock = DockStyle.Right, BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 15, 0, 0)
            };
            btnLichChieu = CreateHeaderBtn("Lịch chiếu", false, new Size(115, 38), new Padding(0, 0, 8, 0), (s, e) => MoFormLichChieu());
            btnTicket = CreateHeaderBtn("Vé của tôi", false, new Size(96, 38), new Padding(0, 0, 8, 0), (s, e) => MoFormVeCuaBan());
            btnLichChieu.Visible = btnTicket.Visible = false;
            btnAdmin = CreateHeaderBtn("Quản trị", false, new Size(102, 38), new Padding(0, 0, 8, 0), BtnAdmin_Click);
            btnAdmin.Visible = PhienDangNhap.NguoiDung?.LaAdmin == true;
            btnLogin = CreateHeaderBtn("Đăng nhập", true, new Size(100, 38), Padding.Empty, BtnLogin_Click);
            pnlHeaderRight.Controls.AddRange(new Control[] { btnLichChieu, btnTicket, btnAdmin, btnLogin });
            pnlHeader.Controls.Add(pnlHeaderRight);

            pnlHeader.Resize += (s, e) => ArrangeHeaderSearch();

            pnlBanner = new Panel { Dock = DockStyle.Top, Height = 108, BackColor = Color.FromArgb(243, 239, 240), Padding = new Padding(20, 8, 20, 0) };
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
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left, Size = new Size(600, 50), Location = new Point(12, 40),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, Text = ""
            };
            picBanner.Controls.Add(lblBannerTitle);
            pnlBanner.Controls.Add(picBanner);

            tmrBanner = new System.Windows.Forms.Timer(components) { Interval = 4000 };
            tmrBanner.Tick += TmrBanner_Tick;

            pnlTabBar = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(24, 9, 24, 9) };
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
                BackColor = Color.FromArgb(36, 35, 42), Size = new Size(110, 30), Cursor = Cursors.Hand, Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnXemTatCa.FlatAppearance.BorderColor = C_BORDER;
            btnXemTatCa.Click += (_, _) => ResetBoLoc();

            pnlTabBar.Controls.AddRange(new Control[] { btnTabDangChieu, btnTabDacBiet, btnTabSapChieu, cmbGenre, btnXemTatCa });
            pnlTabBar.Resize += (s, e) =>
            {
                cmbGenre.Location = new Point(pnlTabBar.Width - 296, 14);
                btnXemTatCa.Location = new Point(pnlTabBar.Width - 126, 14);
            };

            pnlCarouselOuter = new Panel { Dock = DockStyle.Top, Height = 320, BackColor = C_BG, Padding = new Padding(0, 8, 0, 4) };
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

            pnlInfo = new Panel { Name = "pnlFilmInfo", Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(28, 12, 28, 12) };
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
                Text = "Trailer & chi tiết", Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = C_RED, BackColor = Color.White,
                FlatStyle = FlatStyle.Flat, Size = new Size(200, 40), Location = new Point(0, 70), Cursor = Cursors.Hand
            };
            btnTrailer.FlatAppearance.BorderColor = C_BORDER;
            btnTrailer.FlatAppearance.BorderSize = 1;
            btnTrailer.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 244);
            btnTrailer.Click += BtnTrailer_Click;

            pnlInfoRight.Controls.AddRange(new Control[] { btnDatVe, btnTrailer });

            var pnlInfoLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            pnlInfoLeft.Controls.AddRange(new Control[] { lblMoTa, lblMeta, flpTenBadge });

            pnlInfo.Controls.AddRange(new Control[] { pnlInfoRight, pnlInfoLeft });

            pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 48, BackColor = Color.White, Padding = new Padding(24, 0, 24, 0) };
            lblFooterText = new Label
            {
                Dock = DockStyle.Fill, Text = "CGV Cinemas Vietnam  |  Hotline: 1900 6017  |  © 2025 CJ CGV Co., Ltd.",
                Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(117, 112, 121), BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleCenter
            };
            pnlFooter.Controls.Add(lblFooterText);

            Controls.AddRange(new Control[] { pnlInfo, pnlCarouselOuter, pnlTabBar, pnlBanner, pnlHeader, pnlFooter });

            Load += FrmMain_Load;
            KeyDown += FrmMain_KeyDown;
            MouseWheel += FrmMain_MouseWheel;
            Resize += (s, e) => { ArrangeHeaderSearch(); ArrangePosters(); };

            CinemaFormTheme.Apply(this);
            ResumeLayout(false);
        }

        internal static readonly Color C_RED      = Color.FromArgb(231, 26, 15);
        internal static readonly Color C_REDHOVER = Color.FromArgb(196, 21, 11);
        internal static readonly Color C_TEXT     = Color.FromArgb(39, 37, 43);
        internal static readonly Color C_SUB      = Color.FromArgb(117, 112, 121);
        internal static readonly Color C_BORDER   = Color.FromArgb(231, 228, 233);
        internal static readonly Color C_BG       = Color.FromArgb(247, 246, 248);

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
                ForeColor = solid ? Color.White : C_TEXT, BackColor = solid ? C_RED : Color.White,
                FlatStyle = FlatStyle.Flat, Size = size, Margin = margin, Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = solid ? 0 : 1;
            b.FlatAppearance.BorderColor = C_RED;
            b.FlatAppearance.MouseOverBackColor = solid ? C_REDHOVER : Color.FromArgb(248, 247, 249);
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
                ForeColor = C_TEXT, BackColor = Color.FromArgb(220, 255, 255, 255),
                FlatStyle = FlatStyle.Flat, Size = new Size(40, 40), Cursor = Cursors.Hand, Anchor = anchor
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 255, 255, 255);
            b.Region = new Region(RoundedPath(new Rectangle(0, 0, 40, 40), 20));
            b.Click += (_, _) => onClick();
            return b;
        }

        private PictureBox CreatePoster(int delta)
        {
            var p = new PictureBox
            {
                Name = $"picPoster{delta}",
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(237, 234, 238),
                Cursor = Cursors.Hand,
                ErrorImage = null
            };
            if (delta != 0) p.Click += (_, _) => DiChuyen(delta);
            else p.Click += (_, _) => MoChiTietPhim();
            return p;
        }

        internal static void StylePillTab(Button b, string text, bool selected)
        {
            b.Text = text;
            b.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            b.ForeColor = selected ? Color.White : C_SUB;
            b.BackColor = selected ? C_RED : Color.FromArgb(247, 246, 248);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = selected ? C_REDHOVER : Color.FromArgb(239, 237, 241);
            b.Size = new Size(128, 38);
            b.Cursor = Cursors.Hand;
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

        private void ArrangeHeaderSearch()
        {
            if (pnlHeaderRight is null || txtSearch is null || lblNavPhim is null)
                return;

            int left = lblNavPhim.Right + 20;
            int right = pnlHeaderRight.Left - 20;
            int available = Math.Max(0, right - left);
            txtSearch.Width = Math.Min(220, Math.Max(120, available));
            txtSearch.Location = new Point(
                left + Math.Max(0, (available - txtSearch.Width) / 2),
                18);
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

    internal static class CinemaFormTheme
    {
        private static readonly Color Background = Color.FromArgb(247, 246, 248);
        private static readonly Color Surface = Color.White;
        private static readonly Color RaisedSurface = Color.FromArgb(250, 249, 251);
        private static readonly Color InputSurface = Color.White;
        private static readonly Color Border = Color.FromArgb(231, 228, 233);
        private static readonly Color PrimaryText = Color.FromArgb(39, 37, 43);
        private static readonly Color MutedText = Color.FromArgb(117, 112, 121);
        private static readonly Color Accent = Color.FromArgb(215, 25, 32);
        private static readonly Color AccentHover = Color.FromArgb(184, 20, 27);
        private static readonly Color AccentSoft = Color.FromArgb(174, 23, 30);
        private static readonly Color DangerSurface = Color.FromArgb(255, 241, 241);
        private static readonly Color DangerText = Color.FromArgb(180, 35, 44);

        internal static void Apply(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = PrimaryText;
            Apply(form.Controls);
            AddAuthenticationSurface(form);
            ApplyRoundedSurfaces(form.Controls);
        }

        private static void Apply(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is DataGridView grid)
                    ApplyGrid(grid);
                else if (control is TextBoxBase textBox)
                {
                    textBox.BackColor = InputSurface;
                    textBox.ForeColor = PrimaryText;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = InputSurface;
                    comboBox.ForeColor = PrimaryText;
                    comboBox.FlatStyle = FlatStyle.Flat;
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    numericUpDown.BackColor = InputSurface;
                    numericUpDown.ForeColor = PrimaryText;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.CalendarMonthBackground = Surface;
                    dateTimePicker.CalendarForeColor = PrimaryText;
                    dateTimePicker.CalendarTitleBackColor = Accent;
                    dateTimePicker.CalendarTitleForeColor = Color.White;
                    dateTimePicker.CalendarTrailingForeColor = MutedText;
                }
                else if (control is Button button)
                    ApplyButton(button);
                else if (control is Label label)
                    ApplyLabel(label);
                else if (control is CheckBox checkBox)
                    ApplyTextColor(checkBox);
                else if (control is Panel || control is TableLayoutPanel || control is FlowLayoutPanel || control is PictureBox || control is GroupBox)
                {
                    ApplySurface(control);
                }

                if (control.HasChildren)
                    Apply(control.Controls);
            }
        }

        private static void ApplyButton(Button button)
        {
            bool danger = button.Text.Contains("xóa", StringComparison.OrdinalIgnoreCase) ||
                button.Text.Contains("đăng xuất", StringComparison.OrdinalIgnoreCase);
            bool primary = IsAccent(button.BackColor);
            bool selectedNavigation = button.Name.Contains("Sidebar", StringComparison.OrdinalIgnoreCase) &&
                button.ForeColor.ToArgb() == Color.White.ToArgb();

            if (!button.Enabled)
            {
                button.BackColor = Color.FromArgb(240, 238, 242);
                button.ForeColor = Color.FromArgb(155, 150, 158);
            }
            else if (danger)
            {
                button.BackColor = DangerSurface;
                button.ForeColor = DangerText;
            }
            else if (primary)
            {
                button.BackColor = Accent;
                button.ForeColor = Color.White;
            }
            else if (selectedNavigation)
            {
                button.BackColor = DangerSurface;
                button.ForeColor = AccentSoft;
            }
            else
            {
                button.BackColor = Surface;
                button.ForeColor = IsAccent(button.ForeColor) ? AccentSoft : PrimaryText;
            }

            button.UseVisualStyleBackColor = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = primary || selectedNavigation ? 0 : 1;
            button.FlatAppearance.BorderColor = danger ? Color.FromArgb(246, 213, 216) : Border;
            button.FlatAppearance.MouseOverBackColor = danger ? Color.FromArgb(255, 228, 229) :
                primary ? AccentHover : selectedNavigation ? Color.FromArgb(255, 228, 229) : Color.FromArgb(245, 243, 246);
            SetRoundedRegion(button, button.Height >= 40 ? 12 : 8);
        }

        private static void ApplyGrid(DataGridView grid)
        {
            grid.BackgroundColor = Surface;
            grid.GridColor = Border;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(246, 245, 247);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = PrimaryText;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = PrimaryText;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 237, 238);
            grid.DefaultCellStyle.SelectionForeColor = PrimaryText;
            grid.RowsDefaultCellStyle.BackColor = Surface;
            grid.RowsDefaultCellStyle.ForeColor = PrimaryText;
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 237, 238);
            grid.RowsDefaultCellStyle.SelectionForeColor = PrimaryText;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 249, 251);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = PrimaryText;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 237, 238);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = PrimaryText;
        }

        private static void ApplyLabel(Label label)
        {
            if (label.Name == "lblManHinh")
            {
                label.BackColor = Color.FromArgb(40, 38, 42);
                label.ForeColor = Color.White;
                return;
            }

            if (label.BackColor != Color.Transparent && IsAccent(label.BackColor))
            {
                label.ForeColor = Color.White;
                return;
            }

            if (label.BackColor != Color.Transparent && IsNeutral(label.BackColor) &&
                Luminance(label.BackColor) < 205)
                label.BackColor = Color.FromArgb(246, 245, 247);

            if (label.BackColor != Color.Transparent && !IsNeutral(label.BackColor))
            {
                label.BackColor = Color.FromArgb(246, 245, 247);
                label.ForeColor = IsAccent(label.ForeColor) ? AccentSoft : PrimaryText;
            }

            if (label.ForeColor.ToArgb() == Color.White.ToArgb() &&
                label.Parent is not PictureBox && label.Name != "lblBannerTitle")
                label.ForeColor = PrimaryText;
            else if (IsNeutral(label.ForeColor))
                label.ForeColor = Luminance(label.ForeColor) < 105 ? PrimaryText : MutedText;
            else if (Luminance(label.ForeColor) > 190)
                label.ForeColor = PrimaryText;
        }

        private static void ApplyTextColor(Control control)
        {
            Color color = control.ForeColor;
            if (color == Color.Empty || IsNeutral(color))
                control.ForeColor = Luminance(color) < 105 ? PrimaryText : MutedText;
        }

        private static void ApplySurface(Control control)
        {
            if (control.BackColor == Color.Transparent)
                return;

            if (control.Name is "pnlVideoContainer" or "picTrailerThumbnail")
            {
                control.BackColor = Color.FromArgb(40, 38, 42);
                return;
            }
            if (control.Name is "pnlOverview" or "pnlFilmInfo" or "pnlReportRevenueCard" or "pnlReportTicketsCard")
            {
                control.BackColor = Surface;
                return;
            }
            if (control.Width <= 2 || control.Height <= 2)
            {
                control.BackColor = Border;
                return;
            }

            Color color = control.BackColor;
            if (IsNeutral(color))
            {
                double luminance = Luminance(color);
                if (luminance < 75)
                    control.BackColor = control.Dock == DockStyle.Fill ? Background : Surface;
                else if (luminance < 205)
                    control.BackColor = Background;
                else if (luminance >= 240 && color.ToArgb() != Color.White.ToArgb())
                    control.BackColor = Background;
                else
                    control.BackColor = Surface;
            }
            else if (Luminance(color) < 100)
                control.BackColor = Background;
            else if (Luminance(color) < 190)
                control.BackColor = RaisedSurface;
        }

        private static void AddAuthenticationSurface(Form form)
        {
            if (form.Name != "frmDangNhap" && form.Name != "frmDangKy")
                return;

            var surface = new Panel
            {
                Name = "pnlAuthenticationSurface",
                Location = new Point(10, 10),
                Size = new Size(form.ClientSize.Width - 20, form.ClientSize.Height - 20),
                BackColor = Surface,
                BorderStyle = BorderStyle.None
            };
            Control[] content = form.Controls.Cast<Control>().ToArray();
            form.Controls.Add(surface);
            foreach (Control control in content)
            {
                Point location = control.Location;
                form.Controls.Remove(control);
                control.Location = new Point(location.X - surface.Left, location.Y - surface.Top);
                surface.Controls.Add(control);
            }
            ApplyRoundedSurface(surface);
        }

        private static void ApplyRoundedSurfaces(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is Panel &&
                    (control.Name is "moduleCard" or "pnlDetail" or "pnlSoDoGhe" or "pnlLegend" or
                        "pnlTongKet" or "pnlOverview" or "pnlFilmInfo" or "pnlReportRevenueCard" or
                        "pnlReportTicketsCard" or "pnlReportFilter" or "pnlTicketContent"))
                    ApplyRoundedSurface(control);
                else if (control is PictureBox &&
                    (control.Name.StartsWith("picPoster", StringComparison.Ordinal) ||
                     control.Name is "picMoviePoster" or "picTrailerThumbnail"))
                    ApplyRoundedSurface(control);

                if (control.HasChildren)
                    ApplyRoundedSurfaces(control.Controls);
            }
        }

        private static void ApplyRoundedSurface(Control control)
        {
            SetRoundedRegion(control, 14);
            control.Paint += (_, e) =>
            {
                if (control.ClientSize.Width < 2 || control.ClientSize.Height < 2)
                    return;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var path = CreateRoundedPath(
                    new Rectangle(0, 0, control.ClientSize.Width - 1, control.ClientSize.Height - 1), 14);
                using var pen = new Pen(Border);
                e.Graphics.DrawPath(pen, path);
            };
        }

        private static void SetRoundedRegion(Control control, int radius)
        {
            void UpdateRegion()
            {
                if (control.IsDisposed || control.Width < 2 || control.Height < 2)
                    return;

                int safeRadius = Math.Min(radius, Math.Min(control.Width, control.Height) / 2);
                using var path = CreateRoundedPath(
                    new Rectangle(0, 0, control.Width, control.Height), safeRadius);
                Region? previousRegion = control.Region;
                control.Region = new Region(path);
                previousRegion?.Dispose();
            }

            control.SizeChanged += (_, _) => UpdateRegion();
            UpdateRegion();
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = Math.Max(1, radius * 2);
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static bool IsNeutral(Color color) =>
            Math.Max(color.R, Math.Max(color.G, color.B)) - Math.Min(color.R, Math.Min(color.G, color.B)) < 22;

        private static bool IsAccent(Color color) =>
            color.R > 130 && color.R > color.G * 1.35 && color.R > color.B * 1.15;

        private static double Luminance(Color color) =>
            (color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114);
    }
}