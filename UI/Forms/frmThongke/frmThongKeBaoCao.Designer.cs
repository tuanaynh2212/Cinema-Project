using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI
{
    partial class frmThongKeBaoCao
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTieuDe;
        private Label lblTuNgay;
        private Label lblDenNgay;
        private DateTimePicker dtpTuNgay;
        private DateTimePicker dtpDenNgay;
        private Button btnThongKe;
        private Label lblTongDoanhThu;
        private Label lblTongVe;
        private Label lblChiTiet;
        private DataGridView dgvBaoCao;
        private Label lblTrangThai;
        private Button btnXuat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTieuDe = new Label();
            this.lblTuNgay = new Label();
            this.lblDenNgay = new Label();
            this.dtpTuNgay = new DateTimePicker();
            this.dtpDenNgay = new DateTimePicker();
            this.btnThongKe = new Button();
            this.lblTongDoanhThu = new Label();
            this.lblTongVe = new Label();
            this.lblChiTiet = new Label();
            this.dgvBaoCao = new DataGridView();
            this.lblTrangThai = new Label();
            this.btnXuat = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCao)).BeginInit();
            this.SuspendLayout();

            this.lblTieuDe.Dock = DockStyle.Fill;
            this.lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTieuDe.ForeColor = Color.FromArgb(39, 37, 43);
            this.lblTieuDe.Text = "Thống kê doanh thu";
            this.lblTieuDe.TextAlign = ContentAlignment.MiddleLeft;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(247, 246, 248),
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(24),
                Margin = Padding.Empty
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));

            var header = new Panel
            {
                Name = "pnlReportHeader",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(2, 0, 2, 8)
            };
            header.Controls.Add(this.lblTieuDe);

            var filterPanel = new Panel
            {
                Name = "pnlReportFilter",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(14, 8, 14, 8),
                Margin = new Padding(0, 0, 0, 12)
            };

            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Location = new Point(12, 22);
            this.lblTuNgay.Text = "Từ ngày:";
            this.dtpTuNgay.Format = DateTimePickerFormat.Short;
            this.dtpTuNgay.Location = new Point(82, 17);
            this.dtpTuNgay.Size = new Size(132, 28);
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Location = new Point(234, 22);
            this.lblDenNgay.Text = "Đến ngày:";
            this.dtpDenNgay.Format = DateTimePickerFormat.Short;
            this.dtpDenNgay.Location = new Point(306, 17);
            this.dtpDenNgay.Size = new Size(132, 28);

            this.btnThongKe.BackColor = Color.FromArgb(215, 25, 32);
            this.btnThongKe.FlatStyle = FlatStyle.Flat;
            this.btnThongKe.FlatAppearance.BorderSize = 0;
            this.btnThongKe.ForeColor = Color.White;
            this.btnThongKe.Location = new Point(458, 12);
            this.btnThongKe.Size = new Size(178, 38);
            this.btnThongKe.Text = "Thống kê doanh thu";
            this.btnThongKe.UseVisualStyleBackColor = false;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);

            filterPanel.Controls.AddRange(new Control[]
            {
                this.lblTuNgay, this.dtpTuNgay, this.lblDenNgay, this.dtpDenNgay, this.btnThongKe
            });

            var metrics = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            metrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            var revenueCard = new Panel
            {
                Name = "pnlReportRevenueCard",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 8, 12),
                Padding = new Padding(18)
            };
            var ticketsCard = new Panel
            {
                Name = "pnlReportTicketsCard",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(8, 0, 0, 12),
                Padding = new Padding(18)
            };

            this.lblTongDoanhThu.Dock = DockStyle.Fill;
            this.lblTongDoanhThu.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            this.lblTongDoanhThu.ForeColor = Color.FromArgb(39, 37, 43);
            this.lblTongDoanhThu.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTongDoanhThu.Text = "Tổng doanh thu: 0 VNĐ";
            this.lblTongVe.Dock = DockStyle.Fill;
            this.lblTongVe.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            this.lblTongVe.ForeColor = Color.FromArgb(39, 37, 43);
            this.lblTongVe.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTongVe.Text = "Tổng số vé: 0";

            revenueCard.Controls.Add(this.lblTongDoanhThu);
            ticketsCard.Controls.Add(this.lblTongVe);
            metrics.Controls.Add(revenueCard, 0, 0);
            metrics.Controls.Add(ticketsCard, 1, 0);

            var tablePanel = new Panel
            {
                Name = "pnlReportTable",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 8)
            };
            this.lblChiTiet.Dock = DockStyle.Top;
            this.lblChiTiet.Height = 42;
            this.lblChiTiet.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblChiTiet.ForeColor = Color.FromArgb(39, 37, 43);
            this.lblChiTiet.Padding = new Padding(16, 0, 0, 0);
            this.lblChiTiet.Text = "Chi tiết doanh thu";
            this.lblChiTiet.TextAlign = ContentAlignment.MiddleLeft;

            this.dgvBaoCao.AllowUserToAddRows = false;
            this.dgvBaoCao.AllowUserToDeleteRows = false;
            this.dgvBaoCao.Dock = DockStyle.Fill;
            this.dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBaoCao.BackgroundColor = Color.White;
            this.dgvBaoCao.BorderStyle = BorderStyle.None;
            this.dgvBaoCao.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvBaoCao.GridColor = Color.FromArgb(231, 228, 233);
            this.dgvBaoCao.ColumnHeadersHeight = 42;
            this.dgvBaoCao.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(246, 245, 247);
            this.dgvBaoCao.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(39, 37, 43);
            this.dgvBaoCao.EnableHeadersVisualStyles = false;
            this.dgvBaoCao.ReadOnly = true;
            this.dgvBaoCao.RowHeadersVisible = false;
            this.dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBaoCao.MultiSelect = false;
            this.dgvBaoCao.DefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            this.dgvBaoCao.RowTemplate.Height = 38;
            this.dgvBaoCao.Columns.Add("NgayMua", "Ngày mua");
            this.dgvBaoCao.Columns.Add("TenPhim", "Tên phim");
            this.dgvBaoCao.Columns.Add("SoVe", "Số vé");
            this.dgvBaoCao.Columns.Add("DonGia", "Đơn giá (VNĐ)");
            this.dgvBaoCao.Columns.Add("DoanhThu", "Doanh thu (VNĐ)");
            tablePanel.Controls.Add(this.dgvBaoCao);
            tablePanel.Controls.Add(this.lblChiTiet);

            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Dock = DockStyle.Fill;
            this.lblTrangThai.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTrangThai.Text = "Đang chờ dữ liệu...";
            this.btnXuat.BackColor = Color.White;
            this.btnXuat.FlatStyle = FlatStyle.Flat;
            this.btnXuat.ForeColor = Color.FromArgb(39, 37, 43);
            this.btnXuat.FlatAppearance.BorderColor = Color.FromArgb(231, 228, 233);
            this.btnXuat.Size = new Size(210, 40);
            this.btnXuat.Anchor = AnchorStyles.Right;
            this.btnXuat.Text = "Xuất báo cáo CSV";
            this.btnXuat.UseVisualStyleBackColor = false;
            this.btnXuat.Click += new System.EventHandler(this.btnXuat_Click);

            var footer = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            footer.Controls.Add(this.lblTrangThai, 0, 0);
            footer.Controls.Add(this.btnXuat, 1, 0);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(filterPanel, 0, 1);
            root.Controls.Add(metrics, 0, 2);
            root.Controls.Add(tablePanel, 0, 3);
            root.Controls.Add(footer, 0, 4);
            this.Controls.Add(root);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(247, 246, 248);
            this.ClientSize = new Size(1034, 700);
            this.Font = new Font("Segoe UI", 9F);
            this.MinimumSize = new Size(850, 640);
            this.Name = "frmThongKeBaoCao";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CGV Cinemas - Thống kê doanh thu";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCao)).EndInit();
            cinema_project.UI.CinemaFormTheme.Apply(this);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
