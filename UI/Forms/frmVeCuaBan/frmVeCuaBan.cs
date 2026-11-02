#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using cinema_project.DAL;
using cinema_project.DAL.DTO;

namespace cinema_project.UI.Forms.frmVeCuaBan
{
    public sealed class frmVeCuaBan : Form
    {
        private readonly VeDaDatDAL _dal = new VeDaDatDAL();
        private readonly Label _lblStatus = new Label();
        private readonly DataGridView _grid = new DataGridView();
        private readonly Button _btnReload = new Button();
        private readonly Label _lblCount = new Label();

        public frmVeCuaBan()
        {
            TaoGiaoDien();
            Load += async (_, _) => await TaiDuLieuAsync();
        }

        private void TaoGiaoDien()
        {
            Text = "CGV Cinemas - Vé của bạn";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1040, 680);
            MinimumSize = new Size(820, 540);
            BackColor = Color.FromArgb(247, 246, 248);
            Font = new Font("Segoe UI", 9.5F);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(24),
                BackColor = BackColor,
                Margin = Padding.Empty
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));

            Panel header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 12),
                Padding = new Padding(18, 8, 18, 8)
            };

            Label logo = new Label
            {
                AutoSize = true,
                Text = "CGV CINEMAS",
                ForeColor = Color.FromArgb(215, 25, 32),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Location = new Point(18, 10)
            };
            header.Controls.Add(logo);

            Label title = new Label
            {
                AutoSize = true,
                Text = "Vé đã đặt",
                ForeColor = Color.FromArgb(39, 37, 43),
                Font = new Font("Segoe UI", 17F, FontStyle.Bold),
                Location = new Point(18, 31)
            };
            header.Controls.Add(title);

            Label note = new Label
            {
                AutoSize = false,
                Text = "Danh sách chỉ hiển thị các ghế đã đặt thuộc tài khoản đang đăng nhập.",
                ForeColor = Color.FromArgb(85, 81, 90),
                BackColor = Color.White,
                Padding = new Padding(16, 8, 16, 8),
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 12),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.RowHeadersVisible = false;
            _grid.BackgroundColor = Color.White;
            _grid.BorderStyle = BorderStyle.None;
            _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _grid.EnableHeadersVisualStyles = false;
            _grid.GridColor = Color.FromArgb(231, 228, 233);
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(246, 245, 247);
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(39, 37, 43);
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _grid.ColumnHeadersHeight = 42;
            _grid.RowTemplate.Height = 40;
            _grid.DefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            _grid.Columns.Add("MaSuat", "Mã suất chiếu");
            _grid.Columns.Add("MaGhe", "Ghế");
            _grid.Columns.Add("LoaiGhe", "Loại ghế");
            _grid.Columns.Add("GiaGhe", "Giá ghế (VNĐ)");
            _grid.Columns.Add("TrangThai", "Trạng thái");
            _grid.Columns.Add("NgayDat", "Ngày đặt");

            _lblCount.AutoSize = true;
            _lblCount.Dock = DockStyle.Fill;
            _lblCount.TextAlign = ContentAlignment.MiddleCenter;
            _lblCount.Text = "Tổng số ghế: 0";

            _lblStatus.AutoSize = false;
            _lblStatus.Dock = DockStyle.Fill;
            _lblStatus.ForeColor = Color.FromArgb(117, 112, 121);
            _lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            _btnReload.Text = "↻ Tải lại";
            _btnReload.Size = new Size(132, 40);
            _btnReload.Anchor = AnchorStyles.Right;
            _btnReload.BackColor = Color.FromArgb(215, 25, 32);
            _btnReload.ForeColor = Color.White;
            _btnReload.FlatStyle = FlatStyle.Flat;
            _btnReload.FlatAppearance.BorderSize = 0;
            _btnReload.Click += async (_, _) => await TaiDuLieuAsync();

            var footer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = Padding.Empty
            };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 142F));
            footer.Controls.Add(_lblStatus, 0, 0);
            footer.Controls.Add(_lblCount, 1, 0);
            footer.Controls.Add(_btnReload, 2, 0);

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(note, 0, 1);
            layout.Controls.Add(_grid, 0, 2);
            layout.Controls.Add(footer, 0, 3);
            Controls.Add(layout);

            cinema_project.UI.CinemaFormTheme.Apply(this);
        }

        private async Task TaiDuLieuAsync()
        {
            _btnReload.Enabled = false;
            _lblStatus.Text = "Đang tải dữ liệu từ Supabase...";
            try
            {
                List<VeDaDatItem> danhSach = await _dal.LayTatCaGheDaDatAsync();
                _grid.Rows.Clear();
                foreach (VeDaDatItem item in danhSach)
                {
                    string loaiGhe = LayLoaiGhe(item.MaGhe);
                    _grid.Rows.Add(
                        item.MaSuatChieu,
                        item.MaGhe,
                        loaiGhe,
                        item.GiaGhe.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")),
                        item.TrangThai == "da_dat" ? "Đã đặt" : item.TrangThai,
                        item.NgayDat.ToLocalTime().ToString("dd/MM/yyyy HH:mm"));
                }
                _lblCount.Text = "Tổng số ghế đã đặt: " + danhSach.Count;
                _lblStatus.Text = danhSach.Count == 0
                    ? "Chưa có ghế nào được xác nhận đặt."
                    : "Dữ liệu vừa được tải từ Supabase.";
            }
            catch (Exception ex)
            {
                _lblStatus.Text = "Không tải được dữ liệu.";
                MessageBox.Show(ex.Message, "Lỗi tải vé", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed) _btnReload.Enabled = true;
            }
        }

        private static string LayLoaiGhe(string maGhe)
        {
            if (maGhe.StartsWith("H", StringComparison.OrdinalIgnoreCase)) return "Ghế đôi";
            if (maGhe.StartsWith("D", StringComparison.OrdinalIgnoreCase) ||
                maGhe.StartsWith("E", StringComparison.OrdinalIgnoreCase) ||
                maGhe.StartsWith("F", StringComparison.OrdinalIgnoreCase) ||
                maGhe.StartsWith("G", StringComparison.OrdinalIgnoreCase)) return "VIP";
            return "Thường";
        }
    }
}
