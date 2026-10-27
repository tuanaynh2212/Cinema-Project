namespace cinema_project.UI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.lblTimKiem = new System.Windows.Forms.Label();
            this.cboLoaiPhim = new System.Windows.Forms.ComboBox();
            this.lblLoc = new System.Windows.Forms.Label();
            this.spltMain = new System.Windows.Forms.SplitContainer();
            this.grpDanhSach = new System.Windows.Forms.GroupBox();
            this.dgvPhim = new System.Windows.Forms.DataGridView();
            this.grpChiTiet = new System.Windows.Forms.GroupBox();
            this.btnDatVe = new System.Windows.Forms.Button();
            this.lblGiaVe = new System.Windows.Forms.Label();
            this.lblDoTuoi = new System.Windows.Forms.Label();
            this.lblNgayChieu = new System.Windows.Forms.Label();
            this.lblThoiLuong = new System.Windows.Forms.Label();
            this.lblTheLoai = new System.Windows.Forms.Label();
            this.lblTenPhim = new System.Windows.Forms.Label();
            this.picPoster = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusTongSo = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlHeader.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).BeginInit();
            this.spltMain.Panel1.SuspendLayout();
            this.spltMain.Panel2.SuspendLayout();
            this.spltMain.SuspendLayout();
            this.grpDanhSach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhim)).BeginInit();
            this.grpChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPoster)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1008, 65);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(434, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🎬 HỆ THỐNG ĐẶT VÉ PHIM CGV CINEMA";
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(248)))));
            this.pnlFilter.Controls.Add(this.btnLamMoi);
            this.pnlFilter.Controls.Add(this.btnTimKiem);
            this.pnlFilter.Controls.Add(this.txtTimKiem);
            this.pnlFilter.Controls.Add(this.lblTimKiem);
            this.pnlFilter.Controls.Add(this.cboLoaiPhim);
            this.pnlFilter.Controls.Add(this.lblLoc);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 65);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1008, 55);
            this.pnlFilter.TabIndex = 1;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.White;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLamMoi.Location = new System.Drawing.Point(685, 12);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(85, 29);
            this.btnLamMoi.TabIndex = 5;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.White;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTimKiem.Location = new System.Drawing.Point(595, 12);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(80, 29);
            this.btnTimKiem.TabIndex = 4;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTimKiem.Location = new System.Drawing.Point(395, 14);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Nhập tên phim...";
            this.txtTimKiem.Size = new System.Drawing.Size(190, 25);
            this.txtTimKiem.TabIndex = 3;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            // 
            // lblTimKiem
            // 
            this.lblTimKiem.AutoSize = true;
            this.lblTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTimKiem.Location = new System.Drawing.Point(325, 17);
            this.lblTimKiem.Name = "lblTimKiem";
            this.lblTimKiem.Size = new System.Drawing.Size(68, 17);
            this.lblTimKiem.TabIndex = 2;
            this.lblTimKiem.Text = "Tìm kiếm:";
            // 
            // cboLoaiPhim
            // 
            this.cboLoaiPhim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiPhim.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboLoaiPhim.FormattingEnabled = true;
            this.cboLoaiPhim.Items.AddRange(new object[] {
            "Tất cả",
            "Đang chiếu",
            "Đặc biệt",
            "Sắp chiếu"});
            this.cboLoaiPhim.Location = new System.Drawing.Point(125, 14);
            this.cboLoaiPhim.Name = "cboLoaiPhim";
            this.cboLoaiPhim.Size = new System.Drawing.Size(165, 25);
            this.cboLoaiPhim.TabIndex = 1;
            this.cboLoaiPhim.SelectedIndexChanged += new System.EventHandler(this.cboLoaiPhim_SelectedIndexChanged);
            // 
            // lblLoc
            // 
            this.lblLoc.AutoSize = true;
            this.lblLoc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLoc.Location = new System.Drawing.Point(18, 17);
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.Size = new System.Drawing.Size(103, 17);
            this.lblLoc.TabIndex = 0;
            this.lblLoc.Text = "Phân loại phim:";
            // 
            // spltMain
            // 
            this.spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltMain.Location = new System.Drawing.Point(0, 120);
            this.spltMain.Name = "spltMain";
            // 
            // spltMain.Panel1
            // 
            this.spltMain.Panel1.Controls.Add(this.grpDanhSach);
            this.spltMain.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // 
            // spltMain.Panel2
            // 
            this.spltMain.Panel2.Controls.Add(this.grpChiTiet);
            this.spltMain.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.spltMain.Size = new System.Drawing.Size(1008, 579);
            this.spltMain.SplitterDistance = 630;
            this.spltMain.TabIndex = 2;
            // 
            // grpDanhSach
            // 
            this.grpDanhSach.Controls.Add(this.dgvPhim);
            this.grpDanhSach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDanhSach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpDanhSach.Location = new System.Drawing.Point(10, 10);
            this.grpDanhSach.Name = "grpDanhSach";
            this.grpDanhSach.Padding = new System.Windows.Forms.Padding(8);
            this.grpDanhSach.Size = new System.Drawing.Size(610, 559);
            this.grpDanhSach.TabIndex = 0;
            this.grpDanhSach.TabStop = false;
            this.grpDanhSach.Text = "DANH SÁCH PHIM";
            // 
            // dgvPhim
            // 
            this.dgvPhim.AllowUserToAddRows = false;
            this.dgvPhim.AllowUserToDeleteRows = false;
            this.dgvPhim.AllowUserToResizeRows = false;
            this.dgvPhim.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhim.BackgroundColor = System.Drawing.Color.White;
            this.dgvPhim.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPhim.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPhim.ColumnHeadersHeight = 35;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhim.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPhim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhim.EnableHeadersVisualStyles = false;
            this.dgvPhim.Location = new System.Drawing.Point(8, 26);
            this.dgvPhim.MultiSelect = false;
            this.dgvPhim.Name = "dgvPhim";
            this.dgvPhim.ReadOnly = true;
            this.dgvPhim.RowHeadersVisible = false;
            this.dgvPhim.RowTemplate.Height = 32;
            this.dgvPhim.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhim.Size = new System.Drawing.Size(594, 525);
            this.dgvPhim.TabIndex = 0;
            this.dgvPhim.SelectionChanged += new System.EventHandler(this.dgvPhim_SelectionChanged);
            // 
            // grpChiTiet
            // 
            this.grpChiTiet.Controls.Add(this.btnDatVe);
            this.grpChiTiet.Controls.Add(this.lblGiaVe);
            this.grpChiTiet.Controls.Add(this.lblDoTuoi);
            this.grpChiTiet.Controls.Add(this.lblNgayChieu);
            this.grpChiTiet.Controls.Add(this.lblThoiLuong);
            this.grpChiTiet.Controls.Add(this.lblTheLoai);
            this.grpChiTiet.Controls.Add(this.lblTenPhim);
            this.grpChiTiet.Controls.Add(this.picPoster);
            this.grpChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpChiTiet.Location = new System.Drawing.Point(10, 10);
            this.grpChiTiet.Name = "grpChiTiet";
            this.grpChiTiet.Padding = new System.Windows.Forms.Padding(12);
            this.grpChiTiet.Size = new System.Drawing.Size(354, 559);
            this.grpChiTiet.TabIndex = 0;
            this.grpChiTiet.TabStop = false;
            this.grpChiTiet.Text = "THÔNG TIN CHI TIẾT (TMDB)";
            // 
            // btnDatVe
            // 
            this.btnDatVe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDatVe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnDatVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDatVe.FlatAppearance.BorderSize = 0;
            this.btnDatVe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatVe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDatVe.ForeColor = System.Drawing.Color.White;
            this.btnDatVe.Location = new System.Drawing.Point(15, 495);
            this.btnDatVe.Name = "btnDatVe";
            this.btnDatVe.Size = new System.Drawing.Size(324, 45);
            this.btnDatVe.TabIndex = 7;
            this.btnDatVe.Text = "🎟️ ĐẶT VÉ NGAY";
            this.btnDatVe.UseVisualStyleBackColor = false;
            this.btnDatVe.Click += new System.EventHandler(this.btnDatVe_Click);
            // 
            // lblGiaVe
            // 
            this.lblGiaVe.AutoSize = true;
            this.lblGiaVe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblGiaVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblGiaVe.Location = new System.Drawing.Point(15, 450);
            this.lblGiaVe.Name = "lblGiaVe";
            this.lblGiaVe.Size = new System.Drawing.Size(125, 20);
            this.lblGiaVe.TabIndex = 6;
            this.lblGiaVe.Text = "Giá vé: 90.000 đ";
            // 
            // lblDoTuoi
            // 
            this.lblDoTuoi.AutoSize = true;
            this.lblDoTuoi.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDoTuoi.Location = new System.Drawing.Point(15, 418);
            this.lblDoTuoi.Name = "lblDoTuoi";
            this.lblDoTuoi.Size = new System.Drawing.Size(83, 17);
            this.lblDoTuoi.TabIndex = 5;
            this.lblDoTuoi.Text = "Độ tuổi: T18";
            // 
            // lblNgayChieu
            // 
            this.lblNgayChieu.AutoSize = true;
            this.lblNgayChieu.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNgayChieu.Location = new System.Drawing.Point(15, 386);
            this.lblNgayChieu.Name = "lblNgayChieu";
            this.lblNgayChieu.Size = new System.Drawing.Size(143, 17);
            this.lblNgayChieu.TabIndex = 4;
            this.lblNgayChieu.Text = "Khởi chiếu: 10/02/2026";
            // 
            // lblThoiLuong
            // 
            this.lblThoiLuong.AutoSize = true;
            this.lblThoiLuong.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblThoiLuong.Location = new System.Drawing.Point(15, 354);
            this.lblThoiLuong.Name = "lblThoiLuong";
            this.lblThoiLuong.Size = new System.Drawing.Size(130, 17);
            this.lblThoiLuong.TabIndex = 3;
            this.lblThoiLuong.Text = "Thời lượng: 120 phút";
            // 
            // lblTheLoai
            // 
            this.lblTheLoai.AutoSize = true;
            this.lblTheLoai.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTheLoai.Location = new System.Drawing.Point(15, 322);
            this.lblTheLoai.Name = "lblTheLoai";
            this.lblTheLoai.Size = new System.Drawing.Size(161, 17);
            this.lblTheLoai.TabIndex = 2;
            this.lblTheLoai.Text = "Thể loại: Tâm lý, Tình cảm";
            // 
            // lblTenPhim
            // 
            this.lblTenPhim.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTenPhim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblTenPhim.Location = new System.Drawing.Point(15, 270);
            this.lblTenPhim.Name = "lblTenPhim";
            this.lblTenPhim.Size = new System.Drawing.Size(324, 45);
            this.lblTenPhim.TabIndex = 1;
            this.lblTenPhim.Text = "Tên phim";
            // 
            // picPoster
            // 
            this.picPoster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(245)))));
            this.picPoster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPoster.Dock = System.Windows.Forms.DockStyle.Top;
            this.picPoster.Location = new System.Drawing.Point(12, 30);
            this.picPoster.Name = "picPoster";
            this.picPoster.Size = new System.Drawing.Size(330, 230);
            this.picPoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPoster.TabIndex = 0;
            this.picPoster.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusTongSo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 699);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusTongSo
            // 
            this.lblStatusTongSo.Name = "lblStatusTongSo";
            this.lblStatusTongSo.Size = new System.Drawing.Size(100, 17);
            this.lblStatusTongSo.Text = "Tổng số phim: 0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1008, 721);
            this.Controls.Add(this.spltMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Đặt Vé Phim CGV (TMDB API)";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.spltMain.Panel1.ResumeLayout(false);
            this.spltMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).EndInit();
            this.spltMain.ResumeLayout(false);
            this.grpDanhSach.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhim)).EndInit();
            this.grpChiTiet.ResumeLayout(false);
            this.grpChiTiet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPoster)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.ComboBox cboLoaiPhim;
        private System.Windows.Forms.Label lblLoc;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.SplitContainer spltMain;
        private System.Windows.Forms.GroupBox grpDanhSach;
        private System.Windows.Forms.DataGridView dgvPhim;
        private System.Windows.Forms.GroupBox grpChiTiet;
        private System.Windows.Forms.PictureBox picPoster;
        private System.Windows.Forms.Label lblTenPhim;
        private System.Windows.Forms.Label lblTheLoai;
        private System.Windows.Forms.Label lblThoiLuong;
        private System.Windows.Forms.Label lblNgayChieu;
        private System.Windows.Forms.Label lblDoTuoi;
        private System.Windows.Forms.Label lblGiaVe;
        private System.Windows.Forms.Button btnDatVe;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusTongSo;
    }
}
