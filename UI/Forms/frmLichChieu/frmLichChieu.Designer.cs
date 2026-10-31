#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmLichChieu
{
    partial class frmLichChieu
    {
        private System.ComponentModel.IContainer components = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            SuspendLayout();

            // Form properties
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 780);
            MinimumSize = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular, GraphicsUnit.Point);
            BackColor = Color.FromArgb(245, 246, 250);
            ForeColor = Color.FromArgb(26, 26, 34);
            Text = "CGV Cinemas – Quản Lý Lịch Chiếu & Suất Chiếu";

            // 1. HEADER PANEL
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(24, 0, 24, 0)
            };
            pnlHeader.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            };

            lblHeaderTitle = new Label
            {
                Text = "QUẢN LÝ LỊCH CHIẾU & SUẤT CHIẾU",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 26, 15),
                Location = new Point(24, 12),
                AutoSize = true
            };

            lblHeaderSubtitle = new Label
            {
                Text = "Sắp xếp phòng chiếu, quản lý khung giờ và giá vé theo từng bộ phim",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(107, 107, 118),
                Location = new Point(25, 40),
                AutoSize = true
            };

            btnDong = new Button
            {
                Text = "✕ Đóng",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(107, 107, 118),
                BackColor = Color.FromArgb(245, 246, 250),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(90, 38),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(ClientSize.Width - 114, 16),
                Cursor = Cursors.Hand
            };
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 232, 240);
            btnDong.Click += (s, e) => Close();

            pnlHeader.Controls.AddRange(new Control[] { lblHeaderTitle, lblHeaderSubtitle, btnDong });

            // 2. FILTER BAR PANEL
            pnlFilter = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 24, 12)
            };
            pnlFilter.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(235, 238, 242), 1);
                e.Graphics.DrawLine(pen, 0, pnlFilter.Height - 1, pnlFilter.Width, pnlFilter.Height - 1);
            };

            // Ngày chiếu filter
            lblLocNgay = new Label { Text = "Ngày chiếu:", AutoSize = true, Location = new Point(24, 22), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(70, 70, 80) };
            dtpLocNgay = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Location = new Point(102, 18),
                Size = new Size(115, 26),
                Font = new Font("Segoe UI", 9.5f)
            };
            dtpLocNgay.ValueChanged += LocSuatChieu_Changed;

            chkTatCaNgay = new CheckBox
            {
                Text = "Tất cả ngày",
                Location = new Point(223, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                Checked = false
            };
            chkTatCaNgay.CheckedChanged += (s, e) =>
            {
                dtpLocNgay.Enabled = !chkTatCaNgay.Checked;
                LocSuatChieu_Changed(s, e);
            };

            // Phim filter
            lblLocPhim = new Label { Text = "Phim:", AutoSize = true, Location = new Point(320, 22), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(70, 70, 80) };
            cmbLocPhim = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(365, 18),
                Size = new Size(170, 26),
                Font = new Font("Segoe UI", 9.5f)
            };
            cmbLocPhim.SelectedIndexChanged += LocSuatChieu_Changed;

            // Phòng filter
            lblLocPhong = new Label { Text = "Phòng:", AutoSize = true, Location = new Point(545, 22), Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(70, 70, 80) };
            cmbLocPhong = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(598, 18),
                Size = new Size(140, 26),
                Font = new Font("Segoe UI", 9.5f)
            };
            cmbLocPhong.SelectedIndexChanged += LocSuatChieu_Changed;

            // Tìm kiếm nhanh
            txtTimKiem = new TextBox
            {
                PlaceholderText = "🔍 Tìm phim, phòng, định dạng...",
                Location = new Point(750, 18),
                Size = new Size(200, 26),
                Font = new Font("Segoe UI", 9.5f)
            };
            txtTimKiem.TextChanged += LocSuatChieu_Changed;

            // Nút đặt lại bộ lọc
            btnDatLaiBoLoc = new Button
            {
                Text = "🔄 Đặt lại",
                Location = new Point(960, 16),
                Size = new Size(85, 29),
                Font = new Font("Segoe UI", 9f, FontStyle.Regular),
                BackColor = Color.FromArgb(240, 242, 245),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDatLaiBoLoc.FlatAppearance.BorderSize = 0;
            btnDatLaiBoLoc.Click += BtnDatLaiBoLoc_Click;

            pnlFilter.Controls.AddRange(new Control[]
            {
                lblLocNgay, dtpLocNgay, chkTatCaNgay,
                lblLocPhim, cmbLocPhim,
                lblLocPhong, cmbLocPhong,
                txtTimKiem, btnDatLaiBoLoc
            });

            // 3. MAIN CONTENT CONTAINER (SPLIT: LEFT TABLE, RIGHT DETAIL)
            pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 16, 24, 16),
                BackColor = Color.FromArgb(245, 246, 250)
            };

            // DETAIL PANEL (RIGHT)
            pnlDetail = new Panel
            {
                Dock = DockStyle.Right,
                Width = 420,
                BackColor = Color.White,
                Padding = new Padding(20, 20, 20, 20)
            };
            pnlDetail.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlDetail.Width - 1, pnlDetail.Height - 1);
            };

            InitDetailControls();

            // TABLE PANEL (LEFT / FILL)
            pnlTable = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 16, 0),
                BackColor = Color.Transparent
            };

            InitTableControls();

            pnlBody.Controls.AddRange(new Control[] { pnlTable, pnlDetail });

            // ADD PANELS TO FORM
            Controls.AddRange(new Control[] { pnlBody, pnlFilter, pnlHeader });

            Load += FrmLichChieu_Load;
            Resize += (s, e) => btnDong.Location = new Point(ClientSize.Width - 114, 16);

            ResumeLayout(false);
        }

        private void InitTableControls()
        {
            pnlTableHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.Transparent
            };

            lblTableTitle = new Label
            {
                Text = "📋 Danh sách suất chiếu",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 26, 34),
                Location = new Point(0, 6),
                AutoSize = true
            };

            lblSoLuongSuat = new Label
            {
                Text = "(0 suất chiếu)",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(107, 107, 118),
                Location = new Point(190, 8),
                AutoSize = true
            };

            pnlTableHeader.Controls.AddRange(new Control[] { lblTableTitle, lblSoLuongSuat });

            // DataGridView setup
            dgvSuatChieu = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(240, 242, 245),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoGenerateColumns = false,
                EnableHeadersVisualStyles = false
            };

            // Style headers
            dgvSuatChieu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 248);
            dgvSuatChieu.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 72);
            dgvSuatChieu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvSuatChieu.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSuatChieu.ColumnHeadersHeight = 40;
            dgvSuatChieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Style rows
            dgvSuatChieu.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            dgvSuatChieu.DefaultCellStyle.ForeColor = Color.FromArgb(30, 30, 40);
            dgvSuatChieu.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 235, 235);
            dgvSuatChieu.DefaultCellStyle.SelectionForeColor = Color.FromArgb(231, 26, 15);
            dgvSuatChieu.RowTemplate.Height = 40;
            dgvSuatChieu.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 253, 255);

            // Columns
            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSuatChieu",
                HeaderText = "Mã SC",
                Width = 65,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaPhong",
                HeaderText = "Mã Phòng",
                Width = 85,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenPhim",
                HeaderText = "Tên Phim",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenPhong",
                HeaderText = "Phòng Chiếu",
                Width = 145,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayChieuFormatted",
                HeaderText = "Ngày Chiếu",
                Width = 100,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "KhungGio",
                HeaderText = "Khung Giờ",
                Width = 115,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DinhDang",
                HeaderText = "Định Dạng",
                Width = 105,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaVeFormatted",
                HeaderText = "Giá Vé",
                Width = 105,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvSuatChieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng Thái",
                Width = 105,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvSuatChieu.SelectionChanged += DgvSuatChieu_SelectionChanged;
            dgvSuatChieu.CellFormatting += DgvSuatChieu_CellFormatting;

            pnlTable.Controls.AddRange(new Control[] { dgvSuatChieu, pnlTableHeader });
        }

        private void InitDetailControls()
        {
            lblDetailTitle = new Label
            {
                Text = "THÔNG TIN SUẤT CHIẾU",
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 26, 15),
                Location = new Point(20, 16),
                AutoSize = true
            };

            lblDetailSub = new Label
            {
                Text = "Chọn dòng để sửa hoặc nhập mới bên dưới",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(120, 120, 130),
                Location = new Point(21, 38),
                AutoSize = true
            };

            int y = 72;
            int labelWidth = 100;
            int inputLeft = 120;
            int inputWidth = 270;
            int rowSpacing = 42;

            // Mã suất chiếu
            lblMaSC = new Label { Text = "Mã suất:", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            txtMaSC = new TextBox { Location = new Point(inputLeft, y), Width = inputWidth, ReadOnly = true, BackColor = Color.FromArgb(245, 245, 248), Font = new Font("Segoe UI", 9.5f) };

            // Phim
            y += rowSpacing;
            lblPhim = new Label { Text = "Phim (*):", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            cmbPhim = new ComboBox { Location = new Point(inputLeft, y), Width = inputWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };
            cmbPhim.SelectedIndexChanged += CmbPhim_SelectedIndexChanged;

            // Phòng chiếu
            y += rowSpacing;
            lblPhong = new Label { Text = "Phòng (*):", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            cmbPhong = new ComboBox { Location = new Point(inputLeft, y), Width = inputWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };

            // Ngày chiếu
            y += rowSpacing;
            lblNgay = new Label { Text = "Ngày chiếu:", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            dtpNgay = new DateTimePicker { Location = new Point(inputLeft, y), Width = inputWidth, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Font = new Font("Segoe UI", 9.5f) };

            // Giờ bắt đầu
            y += rowSpacing;
            lblGioBD = new Label { Text = "Giờ bắt đầu:", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            dtpGioBD = new DateTimePicker { Location = new Point(inputLeft, y), Width = 125, Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Font = new Font("Segoe UI", 9.5f) };
            dtpGioBD.ValueChanged += DtpGioBD_ValueChanged;

            lblGioKT = new Label { Text = "Kết thúc:", Location = new Point(255, y + 4), Width = 60, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            dtpGioKT = new DateTimePicker { Location = new Point(320, y), Width = 70, Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Font = new Font("Segoe UI", 9.5f) };

            // Định dạng
            y += rowSpacing;
            lblDinhDang = new Label { Text = "Định dạng:", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            cmbDinhDang = new ComboBox { Location = new Point(inputLeft, y), Width = inputWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };

            // Giá vé
            y += rowSpacing;
            lblGiaVe = new Label { Text = "Giá vé (VNĐ):", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            nudGiaVe = new NumericUpDown
            {
                Location = new Point(inputLeft, y),
                Width = inputWidth,
                Minimum = 0,
                Maximum = 1000000,
                Increment = 5000,
                Value = 90000,
                ThousandsSeparator = true,
                Font = new Font("Segoe UI", 9.5f)
            };

            // Trạng thái
            y += rowSpacing;
            lblTrangThai = new Label { Text = "Trạng thái:", Location = new Point(20, y + 4), Width = labelWidth, ForeColor = Color.FromArgb(60, 60, 70), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            cmbTrangThai = new ComboBox { Location = new Point(inputLeft, y), Width = inputWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };

            // Nút bấm thao tác
            y += 55;
            btnThem = new Button
            {
                Text = "➕  Thêm Suất",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 26, 15),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, y),
                Size = new Size(180, 42),
                Cursor = Cursors.Hand
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(196, 21, 11);
            btnThem.Click += BtnThem_Click;

            btnCapNhat = new Button
            {
                Text = "💾  Cập Nhật",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(37, 99, 235),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(210, y),
                Size = new Size(180, 42),
                Cursor = Cursors.Hand
            };
            btnCapNhat.FlatAppearance.BorderSize = 0;
            btnCapNhat.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
            btnCapNhat.Click += BtnCapNhat_Click;

            y += 50;
            btnXoa = new Button
            {
                Text = "🗑️  Xóa Suất",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(239, 68, 68),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, y),
                Size = new Size(180, 40),
                Cursor = Cursors.Hand
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            btnXoa.Click += BtnXoa_Click;

            btnLamMoi = new Button
            {
                Text = "🔄  Làm Mới Form",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                BackColor = Color.FromArgb(243, 244, 246),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(210, y),
                Size = new Size(180, 40),
                Cursor = Cursors.Hand
            };
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatAppearance.MouseOverBackColor = Color.FromArgb(229, 231, 235);
            btnLamMoi.Click += BtnLamMoi_Click;

            pnlDetail.Controls.AddRange(new Control[]
            {
                lblDetailTitle, lblDetailSub,
                lblMaSC, txtMaSC,
                lblPhim, cmbPhim,
                lblPhong, cmbPhong,
                lblNgay, dtpNgay,
                lblGioBD, dtpGioBD, lblGioKT, dtpGioKT,
                lblDinhDang, cmbDinhDang,
                lblGiaVe, nudGiaVe,
                lblTrangThai, cmbTrangThai,
                btnThem, btnCapNhat, btnXoa, btnLamMoi
            });
        }

        // Controls
        private Panel pnlHeader = null!, pnlFilter = null!, pnlBody = null!, pnlTable = null!, pnlDetail = null!, pnlTableHeader = null!;
        private Label lblHeaderTitle = null!, lblHeaderSubtitle = null!, lblLocNgay = null!, lblLocPhim = null!, lblLocPhong = null!;
        private Label lblTableTitle = null!, lblSoLuongSuat = null!, lblDetailTitle = null!, lblDetailSub = null!;
        private Label lblMaSC = null!, lblPhim = null!, lblPhong = null!, lblNgay = null!, lblGioBD = null!, lblGioKT = null!, lblDinhDang = null!, lblGiaVe = null!, lblTrangThai = null!;
        private Button btnDong = null!, btnDatLaiBoLoc = null!, btnThem = null!, btnCapNhat = null!, btnXoa = null!, btnLamMoi = null!;
        private DateTimePicker dtpLocNgay = null!, dtpNgay = null!, dtpGioBD = null!, dtpGioKT = null!;
        private CheckBox chkTatCaNgay = null!;
        private ComboBox cmbLocPhim = null!, cmbLocPhong = null!, cmbPhim = null!, cmbPhong = null!, cmbDinhDang = null!, cmbTrangThai = null!;
        private TextBox txtTimKiem = null!, txtMaSC = null!;
        private NumericUpDown nudGiaVe = null!;
        private DataGridView dgvSuatChieu = null!;
    }
}
