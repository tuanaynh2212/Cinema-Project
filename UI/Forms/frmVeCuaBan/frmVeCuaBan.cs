#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using cinema_project.DAL;

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
            Size = new Size(950, 600);
            MinimumSize = new Size(780, 480);
            BackColor = Color.FromArgb(248, 248, 250);
            Font = new Font("Segoe UI", 10F);

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 105,
                BackColor = Color.FromArgb(28, 28, 35)
            };
            Controls.Add(header);

            Label logo = new Label
            {
                AutoSize = true,
                Text = "CGV CINEMAS",
                ForeColor = Color.FromArgb(231, 26, 15),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(24, 12)
            };
            header.Controls.Add(logo);

            Label title = new Label
            {
                AutoSize = true,
                Text = "VÉ / GHẾ ĐÃ ĐẶT",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Location = new Point(24, 39)
            };
            header.Controls.Add(title);

            Label note = new Label
            {
                AutoSize = false,
                Text = "Danh sách chỉ hiển thị các ghế đã đặt thuộc tài khoản đang đăng nhập.",
                ForeColor = Color.FromArgb(120, 65, 0),
                BackColor = Color.FromArgb(255, 244, 214),
                Padding = new Padding(10, 8, 10, 8),
                Location = new Point(22, 120),
                Size = new Size(880, 45),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(note);

            _grid.Location = new Point(22, 180);
            _grid.Size = new Size(880, 310);
            _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.RowHeadersVisible = false;
            _grid.BackgroundColor = Color.White;
            _grid.BorderStyle = BorderStyle.None;
            _grid.EnableHeadersVisualStyles = false;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 26, 15);
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _grid.Columns.Add("MaSuat", "Mã suất chiếu");
            _grid.Columns.Add("MaGhe", "Ghế");
            _grid.Columns.Add("LoaiGhe", "Loại ghế");
            _grid.Columns.Add("GiaGhe", "Giá ghế (VNĐ)");
            _grid.Columns.Add("TrangThai", "Trạng thái");
            _grid.Columns.Add("NgayDat", "Ngày đặt");
            Controls.Add(_grid);

            _lblCount.AutoSize = true;
            _lblCount.Location = new Point(24, 510);
            _lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _lblCount.Text = "Tổng số ghế: 0";
            Controls.Add(_lblCount);

            _lblStatus.AutoSize = false;
            _lblStatus.Location = new Point(24, 535);
            _lblStatus.Size = new Size(620, 25);
            _lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            _lblStatus.ForeColor = Color.DimGray;
            Controls.Add(_lblStatus);

            _btnReload.Text = "↻ Tải lại";
            _btnReload.Location = new Point(770, 510);
            _btnReload.Size = new Size(132, 42);
            _btnReload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _btnReload.BackColor = Color.FromArgb(231, 26, 15);
            _btnReload.ForeColor = Color.White;
            _btnReload.FlatStyle = FlatStyle.Flat;
            _btnReload.FlatAppearance.BorderSize = 0;
            _btnReload.Click += async (_, _) => await TaiDuLieuAsync();
            Controls.Add(_btnReload);
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
