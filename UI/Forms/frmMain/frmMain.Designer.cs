namespace cinema_project.UI
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Design Tokens ─────────────────────────────────
            var cgvRed = System.Drawing.Color.FromArgb(231, 26, 15);
            var cgvRedHover = System.Drawing.Color.FromArgb(200, 20, 10);
            var bgLight = System.Drawing.Color.FromArgb(248, 248, 250);
            var surface = System.Drawing.Color.White;
            var textPrimary = System.Drawing.Color.FromArgb(29, 29, 31);
            var textSecondary = System.Drawing.Color.FromArgb(110, 110, 115);
            var border = System.Drawing.Color.FromArgb(229, 229, 234);
            var hoverBg = System.Drawing.Color.FromArgb(242, 242, 247);

            var fontBase = new System.Drawing.Font(
                "Segoe UI", 9.5F);
            var fontLogo = new System.Drawing.Font(
                "Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            var fontBtn = new System.Drawing.Font(
                "Segoe UI Semibold", 9.5F);
            var fontTab = new System.Drawing.Font(
                "Segoe UI Semibold", 10.5F);
            var fontTitle = new System.Drawing.Font(
                "Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            var fontBadge = new System.Drawing.Font(
                "Segoe UI Semibold", 9F);
            var fontInfo = new System.Drawing.Font(
                "Segoe UI", 9.5F);
            var fontDatVe = new System.Drawing.Font(
                "Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            var fontNav = new System.Drawing.Font(
                "Segoe UI", 22F);
            var fontRank = new System.Drawing.Font(
                "Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            var fontHoTro = new System.Drawing.Font(
                "Segoe UI", 9.5F);

            // Khởi tạo các control
            pnlHeader = new Panel();
            pnlHeaderBorder = new Panel();
            tlpHeader = new TableLayoutPanel();
            picAvatar = new PictureBox();
            lblLogo = new Label();
            tlpHeaderRight = new TableLayoutPanel();
            btnVe = new Button();
            btnMenu = new Button();

            picBanner = new PictureBox();
            tabMain = new TabControl();
            tabDangChieu = new TabPage();
            tabDacBiet = new TabPage();
            tabSapChieu = new TabPage();

            pnlHoTro = new Panel();
            picMascot = new PictureBox();
            lblHoTro = new Label();

            pnlThongTin = new Panel();
            pnlThongTinBorder = new Panel();
            tlpThongTin = new TableLayoutPanel();
            flpThongTinPhim = new FlowLayoutPanel();
            lblTenPhim = new Label();
            lblDoTuoi = new Label();
            lblThongTin = new Label();
            btnDatVe = new Button();

            pnlCarousel = new Panel();
            tlpCarousel = new TableLayoutPanel();
            btnPrev = new Button();
            picXaTrai = new PictureBox();
            picTrai = new PictureBox();
            picGiua = new PictureBox();
            picPhai = new PictureBox();
            picXaPhai = new PictureBox();
            btnNext = new Button();
            lblThuHang = new Label();

            tmrBanner = new System.Windows.Forms.Timer(components);
            tip = new ToolTip(components);

            // Tạm dừng bố cục
            SuspendLayout();
            pnlHeader.SuspendLayout();
            tlpHeader.SuspendLayout();
            tlpHeaderRight.SuspendLayout();
            pnlHoTro.SuspendLayout();
            pnlThongTin.SuspendLayout();
            tlpThongTin.SuspendLayout();
            flpThongTinPhim.SuspendLayout();
            pnlCarousel.SuspendLayout();
            tlpCarousel.SuspendLayout();

            ((System.ComponentModel.ISupportInitialize)picAvatar)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBanner)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)picMascot)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)picXaTrai)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTrai)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)picGiua)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPhai)
                .BeginInit();
            ((System.ComponentModel.ISupportInitialize)picXaPhai)
                .BeginInit();

            // ══════════════════════════════════════════════════
            // HEADER – logo trái, nút phải, nền trắng
            // ══════════════════════════════════════════════════
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 56;
            pnlHeader.BackColor = surface;
            pnlHeader.Padding = new Padding(16, 0, 16, 0);
            pnlHeader.Name = "pnlHeader";

            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.ColumnCount = 3;
            tlpHeader.RowCount = 1;
            tlpHeader.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 56F));
            tlpHeader.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 220F));
            tlpHeader.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));
            tlpHeader.BackColor = surface;
            tlpHeader.Name = "tlpHeader";

            picAvatar.Dock = DockStyle.Fill;
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.BackColor = surface;
            picAvatar.Name = "picAvatar";
            picAvatar.TabStop = false;

            lblLogo.Dock = DockStyle.Fill;
            lblLogo.Text = "CGV";
            lblLogo.TextAlign = ContentAlignment.MiddleLeft;
            lblLogo.Font = fontLogo;
            lblLogo.ForeColor = cgvRed;
            lblLogo.BackColor = surface;
            lblLogo.Padding = new Padding(4, 0, 0, 0);
            lblLogo.Name = "lblLogo";

            tlpHeaderRight.Dock = DockStyle.Fill;
            tlpHeaderRight.ColumnCount = 2;
            tlpHeaderRight.RowCount = 1;
            tlpHeaderRight.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));
            tlpHeaderRight.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));
            tlpHeaderRight.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));
            tlpHeaderRight.BackColor = surface;
            tlpHeaderRight.Name = "tlpHeaderRight";

            btnVe.Dock = DockStyle.None;
            btnVe.Anchor = AnchorStyles.None;
            btnVe.Size = new System.Drawing.Size(92, 36);
            btnVe.Text = "🎫 Vé";
            btnVe.Font = fontBtn;
            btnVe.FlatStyle = FlatStyle.Flat;
            btnVe.FlatAppearance.BorderColor = border;
            btnVe.FlatAppearance.BorderSize = 1;
            btnVe.FlatAppearance.MouseOverBackColor = hoverBg;
            btnVe.BackColor = surface;
            btnVe.ForeColor = textPrimary;
            btnVe.Cursor = Cursors.Hand;
            btnVe.Name = "btnVe";
            btnVe.Click += new EventHandler(btnVe_Click);

            btnMenu.Dock = DockStyle.None;
            btnMenu.Anchor = AnchorStyles.None;
            btnMenu.Size = new System.Drawing.Size(92, 36);
            btnMenu.Text = "☰ Menu";
            btnMenu.Font = fontBtn;
            btnMenu.FlatStyle = FlatStyle.Flat;
            btnMenu.FlatAppearance.BorderColor = border;
            btnMenu.FlatAppearance.BorderSize = 1;
            btnMenu.FlatAppearance.MouseOverBackColor = hoverBg;
            btnMenu.BackColor = surface;
            btnMenu.ForeColor = textPrimary;
            btnMenu.Cursor = Cursors.Hand;
            btnMenu.Name = "btnMenu";
            btnMenu.Click += new EventHandler(btnMenu_Click);

            tlpHeaderRight.Controls.Add(btnVe, 0, 0);
            tlpHeaderRight.Controls.Add(btnMenu, 1, 0);

            tlpHeader.Controls.Add(picAvatar, 0, 0);
            tlpHeader.Controls.Add(lblLogo, 1, 0);
            tlpHeader.Controls.Add(tlpHeaderRight, 2, 0);
            pnlHeader.Controls.Add(tlpHeader);

            // Đường kẻ phân cách header
            pnlHeaderBorder.Dock = DockStyle.Top;
            pnlHeaderBorder.Height = 1;
            pnlHeaderBorder.BackColor = border;
            pnlHeaderBorder.Name = "pnlHeaderBorder";

            // ══════════════════════════════════════════════════
            // BANNER quảng cáo
            // ══════════════════════════════════════════════════
            picBanner.Dock = DockStyle.Top;
            picBanner.Height = 120;
            picBanner.SizeMode = PictureBoxSizeMode.Zoom;
            picBanner.BackColor = bgLight;
            picBanner.Name = "picBanner";

            tmrBanner.Interval = 3000;
            tmrBanner.Tick += new EventHandler(tmrBanner_Tick);

            // ══════════════════════════════════════════════════
            // TABS – vẽ thủ công, gạch đỏ dưới tab đang chọn
            // ══════════════════════════════════════════════════
            tabMain.Dock = DockStyle.Top;
            tabMain.Height = 42;
            tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabMain.SizeMode = TabSizeMode.Fixed;
            tabMain.ItemSize = new System.Drawing.Size(110, 40);
            tabMain.Appearance = TabAppearance.FlatButtons;
            tabMain.Font = fontTab;
            tabMain.BackColor = surface;
            tabMain.Name = "tabMain";
            tabMain.SelectedIndexChanged +=
                new EventHandler(tabMain_SelectedIndexChanged);
            tabMain.DrawItem +=
                new DrawItemEventHandler(tabMain_DrawItem);

            tabDangChieu.Text = "Đang Chiếu";
            tabDangChieu.BackColor = surface;
            tabDangChieu.Name = "tabDangChieu";

            tabDacBiet.Text = "Đặc Biệt";
            tabDacBiet.BackColor = surface;
            tabDacBiet.Name = "tabDacBiet";

            tabSapChieu.Text = "Sắp Chiếu";
            tabSapChieu.BackColor = surface;
            tabSapChieu.Name = "tabSapChieu";

            tabMain.TabPages.AddRange(new TabPage[]
            {
                tabDangChieu,
                tabDacBiet,
                tabSapChieu
            });

            // ══════════════════════════════════════════════════
            // HỖ TRỢ – thanh nhỏ dưới cùng
            // ══════════════════════════════════════════════════
            pnlHoTro.Dock = DockStyle.Bottom;
            pnlHoTro.Height = 42;
            pnlHoTro.BackColor = surface;
            pnlHoTro.Padding = new Padding(20, 0, 20, 0);
            pnlHoTro.Name = "pnlHoTro";

            picMascot.Dock = DockStyle.Left;
            picMascot.Width = 36;
            picMascot.SizeMode = PictureBoxSizeMode.Zoom;
            picMascot.BackColor = surface;
            picMascot.Name = "picMascot";
            picMascot.TabStop = false;

            lblHoTro.Dock = DockStyle.Fill;
            lblHoTro.Text = "Bạn cần hỗ trợ gì?";
            lblHoTro.TextAlign = ContentAlignment.MiddleLeft;
            lblHoTro.Font = fontHoTro;
            lblHoTro.ForeColor = textSecondary;
            lblHoTro.BackColor = surface;
            lblHoTro.Padding = new Padding(8, 0, 0, 0);
            lblHoTro.Name = "lblHoTro";

            pnlHoTro.Controls.Add(lblHoTro);
            pnlHoTro.Controls.Add(picMascot);

            // ══════════════════════════════════════════════════
            // THÔNG TIN PHIM + NÚT ĐẶT VÉ
            // ══════════════════════════════════════════════════
            pnlThongTinBorder.Dock = DockStyle.Bottom;
            pnlThongTinBorder.Height = 1;
            pnlThongTinBorder.BackColor = border;
            pnlThongTinBorder.Name = "pnlThongTinBorder";

            pnlThongTin.Dock = DockStyle.Bottom;
            pnlThongTin.Height = 100;
            pnlThongTin.BackColor = surface;
            pnlThongTin.Padding = new Padding(28, 10, 28, 10);
            pnlThongTin.Name = "pnlThongTin";

            tlpThongTin.Dock = DockStyle.Fill;
            tlpThongTin.ColumnCount = 2;
            tlpThongTin.RowCount = 2;
            tlpThongTin.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 78F));
            tlpThongTin.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 22F));
            tlpThongTin.RowStyles.Add(
                new RowStyle(SizeType.Percent, 55F));
            tlpThongTin.RowStyles.Add(
                new RowStyle(SizeType.Percent, 45F));
            tlpThongTin.BackColor = surface;
            tlpThongTin.Name = "tlpThongTin";

            flpThongTinPhim.Dock = DockStyle.Fill;
            flpThongTinPhim.WrapContents = true;
            flpThongTinPhim.BackColor = surface;
            flpThongTinPhim.Name = "flpThongTinPhim";

            lblTenPhim.AutoSize = true;
            lblTenPhim.Text = "Chọn phim";
            lblTenPhim.Font = fontTitle;
            lblTenPhim.ForeColor = textPrimary;
            lblTenPhim.Margin = new Padding(0, 6, 10, 0);
            lblTenPhim.Name = "lblTenPhim";

            lblDoTuoi.AutoSize = true;
            lblDoTuoi.Text = "";
            lblDoTuoi.Font = fontBadge;
            lblDoTuoi.ForeColor = System.Drawing.Color.White;
            lblDoTuoi.BackColor = cgvRed;
            lblDoTuoi.Padding = new Padding(6, 4, 6, 4);
            lblDoTuoi.Margin = new Padding(0, 9, 0, 0);
            lblDoTuoi.Visible = false;
            lblDoTuoi.Name = "lblDoTuoi";

            flpThongTinPhim.Controls.Add(lblTenPhim);
            flpThongTinPhim.Controls.Add(lblDoTuoi);

            lblThongTin.Dock = DockStyle.Fill;
            lblThongTin.Text = "";
            lblThongTin.TextAlign = ContentAlignment.MiddleLeft;
            lblThongTin.Font = fontInfo;
            lblThongTin.ForeColor = textSecondary;
            lblThongTin.BackColor = surface;
            lblThongTin.Name = "lblThongTin";

            btnDatVe.Text = "ĐẶT VÉ";
            btnDatVe.Name = "btnDatVe";
            btnDatVe.Anchor = AnchorStyles.None;
            btnDatVe.Size = new System.Drawing.Size(160, 48);
            btnDatVe.Font = fontDatVe;
            btnDatVe.FlatStyle = FlatStyle.Flat;
            btnDatVe.FlatAppearance.BorderSize = 0;
            btnDatVe.FlatAppearance.MouseOverBackColor = cgvRedHover;
            btnDatVe.BackColor = cgvRed;
            btnDatVe.ForeColor = System.Drawing.Color.White;
            btnDatVe.Cursor = Cursors.Hand;
            btnDatVe.Click += new EventHandler(btnDatVe_Click);

            tlpThongTin.Controls.Add(flpThongTinPhim, 0, 0);
            tlpThongTin.Controls.Add(lblThongTin, 0, 1);
            tlpThongTin.Controls.Add(btnDatVe, 1, 0);
            tlpThongTin.SetRowSpan(btnDatVe, 2);

            pnlThongTin.Controls.Add(tlpThongTin);

            // ══════════════════════════════════════════════════
            // CAROUSEL – năm poster + hai nút điều hướng
            // ══════════════════════════════════════════════════
            pnlCarousel.Dock = DockStyle.Fill;
            pnlCarousel.BackColor = bgLight;
            pnlCarousel.Padding = new Padding(0, 8, 0, 8);
            pnlCarousel.Name = "pnlCarousel";

            tlpCarousel.Dock = DockStyle.Fill;
            tlpCarousel.ColumnCount = 7;
            tlpCarousel.RowCount = 1;
            tlpCarousel.BackColor = bgLight;
            tlpCarousel.Name = "tlpCarousel";

            float[] tyLeCot = { 5F, 10F, 16F, 38F, 16F, 10F, 5F };

            foreach (float tyLe in tyLeCot)
            {
                tlpCarousel.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, tyLe));
            }

            tlpCarousel.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            btnPrev.Dock = DockStyle.Fill;
            btnPrev.Text = "‹";
            btnPrev.Font = fontNav;
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.FlatAppearance.BorderSize = 0;
            btnPrev.FlatAppearance.MouseOverBackColor =
                System.Drawing.Color.FromArgb(235, 235, 237);
            btnPrev.BackColor = System.Drawing.Color.Transparent;
            btnPrev.ForeColor = textSecondary;
            btnPrev.Cursor = Cursors.Hand;
            btnPrev.Name = "btnPrev";
            btnPrev.Click += new EventHandler(btnPrev_Click);

            btnNext.Dock = DockStyle.Fill;
            btnNext.Text = "›";
            btnNext.Font = fontNav;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.FlatAppearance.MouseOverBackColor =
                System.Drawing.Color.FromArgb(235, 235, 237);
            btnNext.BackColor = System.Drawing.Color.Transparent;
            btnNext.ForeColor = textSecondary;
            btnNext.Cursor = Cursors.Hand;
            btnNext.Name = "btnNext";
            btnNext.Click += new EventHandler(btnNext_Click);

            picXaTrai.Dock = DockStyle.Fill;
            picXaTrai.Margin = new Padding(2, 60, 2, 60);
            picXaTrai.SizeMode = PictureBoxSizeMode.Zoom;
            picXaTrai.BackColor = System.Drawing.Color.Transparent;
            picXaTrai.Cursor = Cursors.Hand;
            picXaTrai.Name = "picXaTrai";
            picXaTrai.Click += new EventHandler(picXaTrai_Click);

            picTrai.Dock = DockStyle.Fill;
            picTrai.Margin = new Padding(2, 30, 2, 30);
            picTrai.SizeMode = PictureBoxSizeMode.Zoom;
            picTrai.BackColor = System.Drawing.Color.Transparent;
            picTrai.Cursor = Cursors.Hand;
            picTrai.Name = "picTrai";
            picTrai.Click += new EventHandler(picTrai_Click);

            picGiua.Dock = DockStyle.Fill;
            picGiua.Margin = new Padding(4);
            picGiua.SizeMode = PictureBoxSizeMode.Zoom;
            picGiua.BackColor = System.Drawing.Color.Transparent;
            picGiua.Cursor = Cursors.Hand;
            picGiua.Controls.Add(lblThuHang);
            picGiua.Name = "picGiua";
            picGiua.Click += new EventHandler(picGiua_Click);

            lblThuHang.Dock = DockStyle.Bottom;
            lblThuHang.Height = 52;
            lblThuHang.Text = "";
            lblThuHang.TextAlign = ContentAlignment.BottomLeft;
            lblThuHang.BackColor = System.Drawing.Color.Transparent;
            lblThuHang.ForeColor = System.Drawing.Color.White;
            lblThuHang.Font = fontRank;
            lblThuHang.Padding = new Padding(8, 0, 0, 4);
            lblThuHang.Name = "lblThuHang";

            picPhai.Dock = DockStyle.Fill;
            picPhai.Margin = new Padding(2, 30, 2, 30);
            picPhai.SizeMode = PictureBoxSizeMode.Zoom;
            picPhai.BackColor = System.Drawing.Color.Transparent;
            picPhai.Cursor = Cursors.Hand;
            picPhai.Name = "picPhai";
            picPhai.Click += new EventHandler(picPhai_Click);

            picXaPhai.Dock = DockStyle.Fill;
            picXaPhai.Margin = new Padding(2, 60, 2, 60);
            picXaPhai.SizeMode = PictureBoxSizeMode.Zoom;
            picXaPhai.BackColor = System.Drawing.Color.Transparent;
            picXaPhai.Cursor = Cursors.Hand;
            picXaPhai.Name = "picXaPhai";
            picXaPhai.Click += new EventHandler(picXaPhai_Click);

            tlpCarousel.Controls.Add(btnPrev, 0, 0);
            tlpCarousel.Controls.Add(picXaTrai, 1, 0);
            tlpCarousel.Controls.Add(picTrai, 2, 0);
            tlpCarousel.Controls.Add(picGiua, 3, 0);
            tlpCarousel.Controls.Add(picPhai, 4, 0);
            tlpCarousel.Controls.Add(picXaPhai, 5, 0);
            tlpCarousel.Controls.Add(btnNext, 6, 0);

            pnlCarousel.Controls.Add(tlpCarousel);

            // Chú thích thao tác nhanh
            tip.SetToolTip(btnPrev, "Phim trước");
            tip.SetToolTip(btnNext, "Phim tiếp theo");
            tip.SetToolTip(btnDatVe, "Đặt vé phim đang chọn");
            tip.SetToolTip(btnVe, "Vé của bạn");
            tip.SetToolTip(btnMenu, "Menu");

            // ══════════════════════════════════════════════════
            // FORM
            // ══════════════════════════════════════════════════
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgLight;
            Font = fontBase;
            MinimumSize = new System.Drawing.Size(1024, 700);
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            DoubleBuffered = true;
            Name = "frmMain";
            Text = "CGV Cinemas";

            // Thứ tự thêm control để Dock xếp chồng đúng
            Controls.Add(pnlCarousel);
            Controls.Add(pnlThongTinBorder);
            Controls.Add(pnlThongTin);
            Controls.Add(pnlHoTro);
            Controls.Add(tabMain);
            Controls.Add(picBanner);
            Controls.Add(pnlHeaderBorder);
            Controls.Add(pnlHeader);

            Load += new EventHandler(frmMain_Load);
            KeyDown += new KeyEventHandler(frmMain_KeyDown);
            MouseWheel += new MouseEventHandler(frmMain_MouseWheel);

            // Tiếp tục bố cục
            ((System.ComponentModel.ISupportInitialize)picAvatar)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)picBanner)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)picMascot)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)picXaTrai)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)picTrai)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)picGiua)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)picPhai)
                .EndInit();
            ((System.ComponentModel.ISupportInitialize)picXaPhai)
                .EndInit();

            pnlHeader.ResumeLayout(false);
            tlpHeader.ResumeLayout(false);
            tlpHeaderRight.ResumeLayout(false);
            pnlHoTro.ResumeLayout(false);
            pnlThongTin.ResumeLayout(false);
            tlpThongTin.ResumeLayout(false);
            flpThongTinPhim.ResumeLayout(false);
            flpThongTinPhim.PerformLayout();
            pnlCarousel.ResumeLayout(false);
            tlpCarousel.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Khai báo các control
        private Panel pnlHeader;
        private Panel pnlHeaderBorder;
        private TableLayoutPanel tlpHeader;
        private PictureBox picAvatar;
        private Label lblLogo;
        private TableLayoutPanel tlpHeaderRight;
        private Button btnVe;
        private Button btnMenu;

        private PictureBox picBanner;
        private TabControl tabMain;
        private TabPage tabDangChieu;
        private TabPage tabDacBiet;
        private TabPage tabSapChieu;

        private Panel pnlHoTro;
        private PictureBox picMascot;
        private Label lblHoTro;

        private Panel pnlThongTin;
        private Panel pnlThongTinBorder;
        private TableLayoutPanel tlpThongTin;
        private FlowLayoutPanel flpThongTinPhim;
        private Label lblTenPhim;
        private Label lblDoTuoi;
        private Label lblThongTin;
        private Button btnDatVe;

        private Panel pnlCarousel;
        private TableLayoutPanel tlpCarousel;
        private Button btnPrev;
        private PictureBox picXaTrai;
        private PictureBox picTrai;
        private PictureBox picGiua;
        private PictureBox picPhai;
        private PictureBox picXaPhai;
        private Button btnNext;
        private Label lblThuHang;

        private System.Windows.Forms.Timer tmrBanner;
        private ToolTip tip;
    }
}