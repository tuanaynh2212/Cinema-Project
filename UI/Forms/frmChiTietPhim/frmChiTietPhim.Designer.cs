#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmChiTietPhim
{
    partial class frmChiTietPhim
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(1000, 680);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(16, 16, 20);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular, GraphicsUnit.Point);
            Text = "Chi Tiết Phim & Trailer";
            DoubleBuffered = true;

            // Top Header Bar
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(27, 27, 34),
                Padding = new Padding(24, 0, 24, 0)
            };

            btnBack = new Button
            {
                Text = "← Quay lại",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 40, 56),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 38),
                Location = new Point(24, 13),
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 80);
            btnBack.Click += (s, e) => Close();

            lblHeaderTitle = new Label
            {
                Text = "THÔNG TIN CHI TIẾT & TRAILER",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 26, 15),
                Location = new Point(150, 18),
                AutoSize = true
            };

            btnOpenBrowser = new Button
            {
                Text = "↗ Mở trên YouTube",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(232, 226, 232),
                BackColor = Color.FromArgb(38, 37, 45),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(165, 38),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Cursor = Cursors.Hand
            };
            btnOpenBrowser.FlatAppearance.BorderColor = Color.FromArgb(75, 70, 80);
            btnOpenBrowser.FlatAppearance.BorderSize = 1;
            btnOpenBrowser.FlatAppearance.MouseOverBackColor = Color.FromArgb(52, 50, 59);
            btnOpenBrowser.Click += BtnOpenBrowser_Click;

            pnlHeader.Controls.AddRange(new Control[] { btnBack, lblHeaderTitle, btnOpenBrowser });
            pnlHeader.Resize += (s, e) =>
            {
                btnOpenBrowser.Location = new Point(pnlHeader.Width - 189, 13);
            };

            // Left Poster & Meta Column
            pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 340,
                BackColor = Color.FromArgb(27, 27, 34),
                Padding = new Padding(20, 16, 20, 16)
            };

            picPoster = new PictureBox
            {
                Name = "picMoviePoster",
                Dock = DockStyle.Top,
                Height = 360,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(14, 14, 20),
                BorderStyle = BorderStyle.None
            };

            pnlLeftInfo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 12, 0, 0),
                AutoScroll = true
            };

            lblGenre = new Label
            {
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(200, 200, 215),
                Text = "Thể loại: "
            };

            lblReleaseDate = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(200, 200, 215),
                Text = "Ngày chiếu: "
            };

            lblDuration = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(200, 200, 215),
                Text = "Thời lượng: "
            };

            lblRating = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 193, 7),
                Text = "Đánh giá: "
            };

            lblPrice = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(232, 226, 232),
                Text = "Giá vé dự kiến: "
            };

            btnBookTicket = new Button
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Text = "Đặt vé ngay",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 26, 15),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBookTicket.FlatAppearance.BorderSize = 0;
            btnBookTicket.FlatAppearance.MouseOverBackColor = Color.FromArgb(196, 21, 11);
            btnBookTicket.Click += BtnBookTicket_Click;

            pnlLeftInfo.Controls.AddRange(new Control[] { lblPrice, lblRating, lblDuration, lblReleaseDate, lblGenre });
            pnlLeft.Controls.AddRange(new Control[] { pnlLeftInfo, picPoster, btnBookTicket });

            // Right Main Content Panel (Video Player + Storyline)
            pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 16, 20),
                Padding = new Padding(20, 16, 20, 16)
            };

            // Movie Title & Badges
            pnlTitleBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent,
                WrapContents = false
            };

            lblMovieTitle = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(520, 38),
                AutoEllipsis = true,
                Text = "Tên Phim",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Margin = new Padding(0, 0, 12, 0)
            };

            lblAgeBadge = new Label
            {
                Size = new Size(50, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 26, 15),
                Text = "T16",
                Margin = new Padding(0, 4, 10, 0)
            };

            lblStatusBadge = new Label
            {
                Size = new Size(95, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(57, 55, 64),
                Text = "Đang Chiếu",
                Margin = new Padding(0, 4, 0, 0)
            };

            pnlTitleBar.Controls.AddRange(new Control[] { lblMovieTitle, lblAgeBadge, lblStatusBadge });

            // Video Player Container
            pnlVideoContainer = new Panel
            {
                Name = "pnlVideoContainer",
                Dock = DockStyle.Top,
                Height = 360,
                BackColor = Color.Black,
                Margin = new Padding(0, 8, 0, 12)
            };

            picTrailerThumbnail = new PictureBox
            {
                Name = "picTrailerThumbnail",
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(16, 16, 22),
                Cursor = Cursors.Hand
            };
            picTrailerThumbnail.Click += (s, e) => MoTrinhDuyetNgoai();

            btnPlayVideo = new Button
            {
                Text = "Xem trailer trên YouTube",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(232, 226, 232),
                BackColor = Color.FromArgb(38, 37, 45),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(270, 50),
                Cursor = Cursors.Hand
            };
            btnPlayVideo.FlatAppearance.BorderColor = Color.FromArgb(75, 70, 80);
            btnPlayVideo.FlatAppearance.BorderSize = 1;
            btnPlayVideo.FlatAppearance.MouseOverBackColor = Color.FromArgb(52, 50, 59);
            btnPlayVideo.Click += BtnPlayVideo_Click;

            lblVideoStatus = new Label
            {
                Text = "Đang tìm video trailer...",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(220, 220, 235),
                BackColor = Color.Transparent,
                AutoSize = true
            };

            picTrailerThumbnail.Controls.AddRange(new Control[] { btnPlayVideo, lblVideoStatus });
            picTrailerThumbnail.Resize += (s, e) =>
            {
                btnPlayVideo.Location = new Point((picTrailerThumbnail.Width - btnPlayVideo.Width) / 2, (picTrailerThumbnail.Height - btnPlayVideo.Height) / 2 - 16);
                lblVideoStatus.Location = new Point((picTrailerThumbnail.Width - lblVideoStatus.Width) / 2, btnPlayVideo.Bottom + 14);
            };

            pnlVideoContainer.Controls.Add(picTrailerThumbnail);

            // Description / Storyline Section
            pnlOverview = new Panel
            {
                Name = "pnlOverview",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(27, 27, 34),
                Padding = new Padding(18),
                Margin = new Padding(0, 10, 0, 0)
            };

            lblOverviewHeader = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = "📖 NỘI DUNG PHIM",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 26, 15)
            };

            txtOverview = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(24, 24, 34),
                ForeColor = Color.FromArgb(220, 220, 235),
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular, GraphicsUnit.Point),
                BorderStyle = BorderStyle.None
            };

            pnlOverview.Controls.AddRange(new Control[] { txtOverview, lblOverviewHeader });

            // Assemble Right Panel
            pnlRight.Controls.AddRange(new Control[] { pnlOverview, pnlVideoContainer, pnlTitleBar });

            Controls.AddRange(new Control[] { pnlRight, pnlLeft, pnlHeader });

            Load += FrmChiTietPhim_Load;

            cinema_project.UI.CinemaFormTheme.Apply(this);
            ResumeLayout(false);
        }

        private Panel pnlHeader = null!;
        private Button btnBack = null!;
        private Label lblHeaderTitle = null!;
        private Button btnOpenBrowser = null!;

        private Panel pnlLeft = null!;
        private PictureBox picPoster = null!;
        private Panel pnlLeftInfo = null!;
        private Label lblGenre = null!;
        private Label lblReleaseDate = null!;
        private Label lblDuration = null!;
        private Label lblRating = null!;
        private Label lblPrice = null!;
        private Button btnBookTicket = null!;

        private Panel pnlRight = null!;
        private FlowLayoutPanel pnlTitleBar = null!;
        private Label lblMovieTitle = null!;
        private Label lblAgeBadge = null!;
        private Label lblStatusBadge = null!;

        private Panel pnlVideoContainer = null!;
        private PictureBox picTrailerThumbnail = null!;
        private Button btnPlayVideo = null!;
        private Label lblVideoStatus = null!;

        private Panel pnlOverview = null!;
        private Label lblOverviewHeader = null!;
        private TextBox txtOverview = null!;
    }
}
