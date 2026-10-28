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

            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTieuDe.ForeColor = Color.FromArgb(231, 26, 15);
            this.lblTieuDe.Location = new Point(25, 20);
            this.lblTieuDe.Text = "CGV CINEMAS - THỐNG KÊ DOANH THU";

            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Location = new Point(30, 85);
            this.lblTuNgay.Text = "Từ ngày:";

            this.dtpTuNgay.Format = DateTimePickerFormat.Short;
            this.dtpTuNgay.Location = new Point(105, 80);
            this.dtpTuNgay.Size = new Size(130, 25);

            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Location = new Point(260, 85);
            this.lblDenNgay.Text = "Đến ngày:";

            this.dtpDenNgay.Format = DateTimePickerFormat.Short;
            this.dtpDenNgay.Location = new Point(340, 80);
            this.dtpDenNgay.Size = new Size(130, 25);

            this.btnThongKe.BackColor = Color.FromArgb(231, 26, 15);
            this.btnThongKe.FlatStyle = FlatStyle.Flat;
            this.btnThongKe.ForeColor = Color.White;
            this.btnThongKe.Location = new Point(500, 77);
            this.btnThongKe.Size = new Size(170, 35);
            this.btnThongKe.Text = "Thống kê doanh thu";
            this.btnThongKe.UseVisualStyleBackColor = false;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);

            this.lblTongDoanhThu.AutoSize = true;
            this.lblTongDoanhThu.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            this.lblTongDoanhThu.ForeColor = Color.FromArgb(231, 26, 15);
            this.lblTongDoanhThu.Location = new Point(30, 140);
            this.lblTongDoanhThu.Text = "Tổng doanh thu: 0 VNĐ";

            this.lblTongVe.AutoSize = true;
            this.lblTongVe.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            this.lblTongVe.Location = new Point(520, 140);
            this.lblTongVe.Text = "Tổng số vé: 0";

            this.lblChiTiet.AutoSize = true;
            this.lblChiTiet.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblChiTiet.Location = new Point(30, 195);
            this.lblChiTiet.Text = "CHI TIẾT DOANH THU";

            this.dgvBaoCao.AllowUserToAddRows = false;
            this.dgvBaoCao.AllowUserToDeleteRows = false;
            this.dgvBaoCao.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBaoCao.BackgroundColor = Color.White;
            this.dgvBaoCao.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 26, 15);
            this.dgvBaoCao.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvBaoCao.EnableHeadersVisualStyles = false;
            this.dgvBaoCao.Location = new Point(30, 230);
            this.dgvBaoCao.ReadOnly = true;
            this.dgvBaoCao.RowHeadersVisible = false;
            this.dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBaoCao.Size = new Size(970, 300);
            this.dgvBaoCao.Columns.Add("NgayMua", "Ngày mua");
            this.dgvBaoCao.Columns.Add("TenPhim", "Tên phim");
            this.dgvBaoCao.Columns.Add("SoVe", "Số vé");
            this.dgvBaoCao.Columns.Add("DonGia", "Đơn giá (VNĐ)");
            this.dgvBaoCao.Columns.Add("DoanhThu", "Doanh thu (VNĐ)");

            this.lblTrangThai.AutoSize = true;
            this.lblTrangThai.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblTrangThai.Location = new Point(30, 550);
            this.lblTrangThai.Text = "Đang chờ dữ liệu...";

            this.btnXuat.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnXuat.BackColor = Color.FromArgb(231, 26, 15);
            this.btnXuat.FlatStyle = FlatStyle.Flat;
            this.btnXuat.ForeColor = Color.White;
            this.btnXuat.Location = new Point(790, 535);
            this.btnXuat.Size = new Size(210, 40);
            this.btnXuat.Text = "Xuất báo cáo CSV";
            this.btnXuat.UseVisualStyleBackColor = false;
            this.btnXuat.Click += new System.EventHandler(this.btnXuat_Click);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(248, 248, 250);
            this.ClientSize = new Size(1034, 611);
            this.Controls.Add(this.btnXuat);
            this.Controls.Add(this.lblTrangThai);
            this.Controls.Add(this.dgvBaoCao);
            this.Controls.Add(this.lblChiTiet);
            this.Controls.Add(this.lblTongVe);
            this.Controls.Add(this.lblTongDoanhThu);
            this.Controls.Add(this.btnThongKe);
            this.Controls.Add(this.dtpDenNgay);
            this.Controls.Add(this.lblDenNgay);
            this.Controls.Add(this.dtpTuNgay);
            this.Controls.Add(this.lblTuNgay);
            this.Controls.Add(this.lblTieuDe);
            this.Font = new Font("Segoe UI", 9F);
            this.MinimumSize = new Size(850, 550);
            this.Name = "frmThongKeBaoCao";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "CGV Cinemas - Thống kê doanh thu";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaoCao)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
