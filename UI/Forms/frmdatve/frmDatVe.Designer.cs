#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmDatVe
{
    partial class frmDatVe
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel pnlHeader = null!;
        private Label lblLogo = null!;
        private Label lblTieuDe = null!;
        private Label lblTenPhim = null!;
        private Label lblThongTinPhim = null!;
        private Label lblLabelSuatChieu = null!;
        private ComboBox cmbSuatChieu = null!;
        private Label lblTrangThai = null!;
        private Label lblManHinh = null!;
        private Panel pnlSoDoGhe = null!;
        private Panel pnlTongKet = null!;
        private Label lblSuatDaChon = null!;
        private Label lblHuongDanChonGhe = null!;
        private Label lblSoGhe = null!;
        private Label lblDanhSachGhe = null!;
        private Label lblGiaLoaiTitle = null!;
        private Label lblGiaThuong = null!;
        private Label lblGiaVIP = null!;
        private Label lblGiaDoi = null!;
        private Label lblTongTienTitle = null!;
        private Label lblTongTien = null!;
        private Button btnXacNhan = null!;
        private Button btnHuy = null!;
        private Panel pnlLegend = null!;
        private Label lblLegendThuong = null!;
        private Label lblLegendVIP = null!;
        private Label lblLegendDoi = null!;
        private Label lblLegendChon = null!;
        private Label lblLegendDaDat = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblLogo = new Label();
            lblTieuDe = new Label();
            lblTenPhim = new Label();
            lblThongTinPhim = new Label();
            lblLabelSuatChieu = new Label();
            cmbSuatChieu = new ComboBox();
            lblTrangThai = new Label();
            lblManHinh = new Label();
            pnlSoDoGhe = new Panel();
            pnlTongKet = new Panel();
            lblSuatDaChon = new Label();
            lblHuongDanChonGhe = new Label();
            lblSoGhe = new Label();
            lblDanhSachGhe = new Label();
            lblGiaLoaiTitle = new Label();
            lblGiaThuong = new Label();
            lblGiaVIP = new Label();
            lblGiaDoi = new Label();
            lblTongTienTitle = new Label();
            lblTongTien = new Label();
            btnXacNhan = new Button();
            btnHuy = new Button();
            pnlLegend = new Panel();
            lblLegendThuong = new Label();
            lblLegendVIP = new Label();
            lblLegendDoi = new Label();
            lblLegendChon = new Label();
            lblLegendDaDat = new Label();
            pnlHeader.SuspendLayout();
            pnlTongKet.SuspendLayout();
            pnlLegend.SuspendLayout();
            SuspendLayout();

            pnlHeader.BackColor = Color.FromArgb(28, 28, 35);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 82;
            pnlHeader.Controls.Add(lblLogo);
            pnlHeader.Controls.Add(lblTieuDe);

            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(231, 26, 15);
            lblLogo.Location = new Point(24, 12);
            lblLogo.Text = "CGV";

            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(104, 20);
            lblTieuDe.Text = "CHỌN GHẾ NGỒI";

            lblTenPhim.AutoSize = false;
            lblTenPhim.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTenPhim.ForeColor = Color.FromArgb(231, 26, 15);
            lblTenPhim.Location = new Point(24, 94);
            lblTenPhim.Size = new Size(790, 28);
            lblTenPhim.Text = "Tên phim";

            lblThongTinPhim.AutoSize = false;
            lblThongTinPhim.Font = new Font("Segoe UI", 9F);
            lblThongTinPhim.ForeColor = Color.FromArgb(95, 95, 105);
            lblThongTinPhim.Location = new Point(25, 123);
            lblThongTinPhim.Size = new Size(790, 23);
            lblThongTinPhim.Text = "Thông tin phim";

            lblLabelSuatChieu.AutoSize = true;
            lblLabelSuatChieu.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblLabelSuatChieu.Location = new Point(25, 157);
            lblLabelSuatChieu.Text = "Chọn suất chiếu:";

            cmbSuatChieu.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSuatChieu.Font = new Font("Segoe UI", 9.5F);
            cmbSuatChieu.Location = new Point(145, 152);
            cmbSuatChieu.Size = new Size(630, 29);
            cmbSuatChieu.SelectedIndexChanged += cmbSuatChieu_SelectedIndexChanged;

            lblTrangThai.AutoSize = false;
            lblTrangThai.Font = new Font("Segoe UI", 8.5F);
            lblTrangThai.ForeColor = Color.FromArgb(110, 110, 120);
            lblTrangThai.Location = new Point(25, 183);
            lblTrangThai.Size = new Size(790, 23);
            lblTrangThai.Text = "Đang tải suất chiếu...";

            lblManHinh.BackColor = Color.FromArgb(52, 58, 70);
            lblManHinh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblManHinh.ForeColor = Color.White;
            lblManHinh.Location = new Point(40, 211);
            lblManHinh.Size = new Size(770, 28);
            lblManHinh.Text = "MÀN HÌNH";
            lblManHinh.TextAlign = ContentAlignment.MiddleCenter;

            pnlSoDoGhe.BackColor = Color.FromArgb(37, 37, 43);
            pnlSoDoGhe.BorderStyle = BorderStyle.FixedSingle;
            pnlSoDoGhe.Location = new Point(25, 244);
            pnlSoDoGhe.Size = new Size(800, 348);
            pnlSoDoGhe.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            pnlLegend.BackColor = Color.FromArgb(37, 37, 43);
            pnlLegend.Location = new Point(25, 599);
            pnlLegend.Size = new Size(800, 87);
            pnlLegend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pnlLegend.Controls.Add(lblLegendThuong);
            pnlLegend.Controls.Add(lblLegendVIP);
            pnlLegend.Controls.Add(lblLegendDoi);
            pnlLegend.Controls.Add(lblLegendChon);
            pnlLegend.Controls.Add(lblLegendDaDat);

            lblLegendThuong.AutoSize = true;
            lblLegendThuong.Font = new Font("Segoe UI", 8.5F);
            lblLegendThuong.ForeColor = Color.White;
            lblLegendThuong.Location = new Point(12, 12);
            lblLegendThuong.Text = "■ Ghế thường";
            lblLegendThuong.ForeColor = Color.FromArgb(186, 135, 255);

            lblLegendVIP.AutoSize = true;
            lblLegendVIP.Font = new Font("Segoe UI", 8.5F);
            lblLegendVIP.ForeColor = Color.FromArgb(255, 92, 105);
            lblLegendVIP.Location = new Point(160, 12);
            lblLegendVIP.Text = "■ Ghế VIP";

            lblLegendDoi.AutoSize = true;
            lblLegendDoi.Font = new Font("Segoe UI", 8.5F);
            lblLegendDoi.ForeColor = Color.FromArgb(255, 105, 190);
            lblLegendDoi.Location = new Point(270, 12);
            lblLegendDoi.Text = "■ Ghế đôi (H)";

            lblLegendChon.AutoSize = true;
            lblLegendChon.Font = new Font("Segoe UI", 8.5F);
            lblLegendChon.ForeColor = Color.FromArgb(76, 220, 208);
            lblLegendChon.Location = new Point(420, 12);
            lblLegendChon.Text = "■ Đang chọn";

            lblLegendDaDat.AutoSize = true;
            lblLegendDaDat.Font = new Font("Segoe UI", 8.5F);
            lblLegendDaDat.ForeColor = Color.FromArgb(180, 180, 185);
            lblLegendDaDat.Location = new Point(540, 12);
            lblLegendDaDat.Text = "■ Đã đặt";

            pnlTongKet.BackColor = Color.White;
            pnlTongKet.BorderStyle = BorderStyle.FixedSingle;
            pnlTongKet.Location = new Point(845, 94);
            pnlTongKet.Size = new Size(350, 592);
            pnlTongKet.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlTongKet.Controls.Add(lblSuatDaChon);
            pnlTongKet.Controls.Add(lblHuongDanChonGhe);
            pnlTongKet.Controls.Add(lblSoGhe);
            pnlTongKet.Controls.Add(lblDanhSachGhe);
            pnlTongKet.Controls.Add(lblGiaLoaiTitle);
            pnlTongKet.Controls.Add(lblGiaThuong);
            pnlTongKet.Controls.Add(lblGiaVIP);
            pnlTongKet.Controls.Add(lblGiaDoi);
            pnlTongKet.Controls.Add(lblTongTienTitle);
            pnlTongKet.Controls.Add(lblTongTien);
            pnlTongKet.Controls.Add(btnXacNhan);
            pnlTongKet.Controls.Add(btnHuy);

            lblSuatDaChon.AutoSize = false;
            lblSuatDaChon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSuatDaChon.ForeColor = Color.FromArgb(45, 45, 55);
            lblSuatDaChon.Location = new Point(16, 15);
            lblSuatDaChon.Size = new Size(310, 58);
            lblSuatDaChon.Text = "Chưa chọn suất chiếu";

            lblHuongDanChonGhe.AutoSize = false;
            lblHuongDanChonGhe.Font = new Font("Segoe UI", 8.5F);
            lblHuongDanChonGhe.ForeColor = Color.FromArgb(110, 110, 120);
            lblHuongDanChonGhe.Location = new Point(16, 82);
            lblHuongDanChonGhe.Size = new Size(310, 26);
            lblHuongDanChonGhe.Text = "Chọn một hoặc nhiều ghế để tiếp tục.";

            lblSoGhe.AutoSize = true;
            lblSoGhe.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSoGhe.Location = new Point(16, 116);
            lblSoGhe.Text = "Số ghế đã chọn: 0";

            lblDanhSachGhe.AutoSize = false;
            lblDanhSachGhe.Font = new Font("Segoe UI", 9.5F);
            lblDanhSachGhe.ForeColor = Color.FromArgb(85, 85, 95);
            lblDanhSachGhe.Location = new Point(16, 144);
            lblDanhSachGhe.Size = new Size(310, 48);
            lblDanhSachGhe.Text = "Chưa chọn ghế";

            lblGiaLoaiTitle.AutoSize = true;
            lblGiaLoaiTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblGiaLoaiTitle.Location = new Point(16, 204);
            lblGiaLoaiTitle.Text = "GIÁ THEO LOẠI GHẾ";

            lblGiaThuong.AutoSize = true;
            lblGiaThuong.Font = new Font("Segoe UI", 9.5F);
            lblGiaThuong.ForeColor = Color.FromArgb(119, 43, 191);
            lblGiaThuong.Location = new Point(16, 232);
            lblGiaThuong.Text = "Ghế thường: 0 VNĐ";

            lblGiaVIP.AutoSize = true;
            lblGiaVIP.Font = new Font("Segoe UI", 9.5F);
            lblGiaVIP.ForeColor = Color.FromArgb(220, 35, 55);
            lblGiaVIP.Location = new Point(16, 258);
            lblGiaVIP.Text = "Ghế VIP: 0 VNĐ";

            lblGiaDoi.AutoSize = true;
            lblGiaDoi.Font = new Font("Segoe UI", 9.5F);
            lblGiaDoi.ForeColor = Color.FromArgb(205, 30, 125);
            lblGiaDoi.Location = new Point(16, 284);
            lblGiaDoi.Text = "Ghế đôi (mỗi chỗ): 0 VNĐ";

            lblTongTienTitle.AutoSize = true;
            lblTongTienTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTongTienTitle.Location = new Point(16, 330);
            lblTongTienTitle.Text = "TỔNG TẠM TÍNH";

            lblTongTien.AutoSize = false;
            lblTongTien.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTongTien.ForeColor = Color.FromArgb(231, 26, 15);
            lblTongTien.Location = new Point(16, 357);
            lblTongTien.Size = new Size(310, 42);
            lblTongTien.Text = "0 VNĐ";

            btnXacNhan.BackColor = Color.FromArgb(231, 26, 15);
            btnXacNhan.FlatStyle = FlatStyle.Flat;
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXacNhan.ForeColor = Color.White;
            btnXacNhan.Location = new Point(16, 425);
            btnXacNhan.Size = new Size(310, 42);
            btnXacNhan.Text = "XÁC NHẬN GHẾ";
            btnXacNhan.UseVisualStyleBackColor = false;
            btnXacNhan.Click += btnXacNhan_Click;
            btnXacNhan.Enabled = false;
            btnXacNhan.Cursor = Cursors.Hand;

            btnHuy.BackColor = Color.White;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 218);
            btnHuy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnHuy.ForeColor = Color.FromArgb(70, 70, 80);
            btnHuy.Location = new Point(16, 478);
            btnHuy.Size = new Size(310, 36);
            btnHuy.Text = "HỦY";
            btnHuy.Click += btnHuy_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 248, 250);
            ClientSize = new Size(1220, 710);
            Controls.Add(pnlTongKet);
            Controls.Add(pnlLegend);
            Controls.Add(pnlSoDoGhe);
            Controls.Add(lblManHinh);
            Controls.Add(lblTrangThai);
            Controls.Add(cmbSuatChieu);
            Controls.Add(lblLabelSuatChieu);
            Controls.Add(lblThongTinPhim);
            Controls.Add(lblTenPhim);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1160, 750);
            Name = "frmDatVe";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đặt vé - CGV Cinemas";
            Load += frmDatVe_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlTongKet.ResumeLayout(false);
            pnlTongKet.PerformLayout();
            pnlLegend.ResumeLayout(false);
            pnlLegend.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
