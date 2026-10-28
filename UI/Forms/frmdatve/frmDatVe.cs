#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using cinema_project.DAL;

namespace cinema_project.UI.Forms.frmDatVe
{
    public partial class frmDatVe : Form
    {
        private readonly Phim _phim;
        private readonly SupabaseService _supabase = new();
        private readonly GheSuatChieuDAL _gheDAL = new();
        private readonly List<SuatChieu> _cacSuat = new();
        private readonly HashSet<string> _gheDangChon = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _gheDaDat = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Button> _nutGhe = new(StringComparer.OrdinalIgnoreCase);
        private SuatChieu? _suatDangChon;
        private int _lanTaiGhe;
        private bool _dangTaiGhe;
        private bool _loiTaiGhe;

        private static readonly Color MauNenBanDo = Color.FromArgb(37, 37, 43);
        private static readonly Color MauThuong = Color.FromArgb(119, 43, 191); // tím
        private static readonly Color MauVIP = Color.FromArgb(239, 35, 55);     // đỏ
        private static readonly Color MauGheDoi = Color.FromArgb(226, 38, 145); // hồng
        private static readonly Color MauDaDat = Color.FromArgb(83, 86, 92);     // xám
        private static readonly Color MauDangChon = Color.FromArgb(0, 166, 153); // xanh ngọc

        // Thay các hệ số này để điều chỉnh giá theo từng loại ghế.
        private const decimal HeSoGheThuong = 1.00m;
        private const decimal HeSoGheVIP = 1.30m;
        private const decimal HeSoGheDoi = 1.50m;

        public IReadOnlyList<string> GheDaChon => _gheDangChon
            .OrderBy(x => x[0]).ThenBy(x => int.Parse(x.Substring(1))).ToList();
        public SuatChieu? SuatChieuDaChon => _suatDangChon;
        public int SoNguoiDat => SoNguoiCanDat;
        public decimal GiaTamTinh => _gheDangChon.Sum(LayGiaGhe);

        private int SoNguoiCanDat => cmbSoNguoi.SelectedIndex == 1 ? 4 : 1;
        private decimal GiaVeCoBan => (_suatDangChon?.GiaVe ?? 0) > 0
            ? _suatDangChon!.GiaVe
            : _phim.GiaVe;

        public frmDatVe(Phim phim)
        {
            _phim = phim ?? throw new ArgumentNullException(nameof(phim));
            InitializeComponent();
            if (cmbSoNguoi.SelectedIndex < 0) cmbSoNguoi.SelectedIndex = 0;
            cmbSoNguoi.SelectedIndexChanged += cmbSoNguoi_SelectedIndexChanged;
            HienThiThongTinPhim();
            TaoSoDoGhe();
            CapNhatTongTien();
            Load += frmDatVe_Load;
        }

        private void HienThiThongTinPhim()
        {
            lblTenPhim.Text = _phim.Ten;
            lblThongTinPhim.Text = $"Độ tuổi: {_phim.DoTuoi}   •   Thời lượng: {_phim.ThoiLuong}   •   Giá vé cơ bản: {_phim.GiaVe:N0} VNĐ";
            Text = $"Đặt vé - {_phim.Ten}";
        }

        private async void frmDatVe_Load(object? sender, EventArgs e)
        {
            await TaiSuatChieuAsync();
        }

        private async Task TaiSuatChieuAsync()
        {
            cmbSuatChieu.Enabled = false;
            btnXacNhan.Enabled = false;
            lblTrangThai.Text = "Đang tải suất chiếu...";

            try
            {
                List<SuatChieu> tatCa = await _supabase.LayTatCaSuatChieuAsync();
                DateTime now = DateTime.Now;
                _cacSuat.Clear();
                _cacSuat.AddRange(tatCa.Where(s =>
                    LaPhimCanDat(s) &&
                    !string.Equals(s.TrangThai, "Đã hủy", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(s.TrangThai, "Hoàn tất", StringComparison.OrdinalIgnoreCase) &&
                    (s.NgayChieu.Date > now.Date ||
                     (s.NgayChieu.Date == now.Date && s.GioBatDau >= now.TimeOfDay)))
                    .OrderBy(s => s.NgayChieu).ThenBy(s => s.GioBatDau));

                var dsHienThi = _cacSuat.Select(s => new SuatChieuLuaChon(s)).ToList();
                cmbSuatChieu.DisplayMember = nameof(SuatChieuLuaChon.NoiDung);
                cmbSuatChieu.ValueMember = nameof(SuatChieuLuaChon.MaSuatChieu);
                cmbSuatChieu.DataSource = dsHienThi;
                cmbSuatChieu.Enabled = dsHienThi.Count > 0;

                if (dsHienThi.Count > 0)
                {
                    if (cmbSuatChieu.SelectedIndex < 0) cmbSuatChieu.SelectedIndex = 0;
                    lblTrangThai.Text = "Chọn suất chiếu, rồi chọn 1 hoặc 4 ghế. Màu ghế biểu thị loại giá.";
                }
                else
                {
                    _suatDangChon = null;
                    _gheDangChon.Clear();
                    TaoSoDoGhe();
                    CapNhatTongTien();
                    lblSuatDaChon.Text = "Chưa có suất chiếu";
                    lblTrangThai.Text = "Chưa có suất chiếu phù hợp cho phim này. Kiểm tra mục Lịch chiếu.";
                }
            }
            catch (Exception ex)
            {
                _suatDangChon = null;
                _gheDangChon.Clear();
                TaoSoDoGhe();
                CapNhatTongTien();
                lblTrangThai.Text = "Không tải được suất chiếu.";
                MessageBox.Show("Không tải được suất chiếu từ Supabase.\n\n" + ex.Message,
                    "Lỗi tải suất chiếu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool LaPhimCanDat(SuatChieu s)
        {
            int maPhim = _phim.MaPhim;
            bool trungMa = maPhim > 0 &&
                (s.MaPhim == maPhim || (s.PhimThongTin?.MaPhim ?? 0) == maPhim);
            bool trungTen = !string.IsNullOrWhiteSpace(s.TenPhim) &&
                string.Equals(s.TenPhim.Trim(), _phim.Ten.Trim(), StringComparison.OrdinalIgnoreCase);
            return trungMa || trungTen;
        }

        private async void cmbSuatChieu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbSuatChieu.SelectedItem is not SuatChieuLuaChon luaChon)
                return;

            _suatDangChon = luaChon.Suat;
            _gheDangChon.Clear();
            _gheDaDat.Clear();
            _loiTaiGhe = false;
            _dangTaiGhe = true;
            int lanTai = ++_lanTaiGhe;

            lblSuatDaChon.Text = $"{_suatDangChon.NgayChieu:dd/MM/yyyy} • {_suatDangChon.GioBatDau:hh\\:mm}\n{_suatDangChon.TenPhong} • {_suatDangChon.DinhDang}";
            lblTrangThai.Text = "Đang tải ghế đã đặt của suất chiếu...";
            TaoSoDoGhe();
            CapNhatTongTien();

            await TaiGheDaDatAsync(_suatDangChon.MaSuatChieu, lanTai);
        }

        private async Task TaiGheDaDatAsync(int maSuatChieu, int lanTai)
        {
            try
            {
                List<string> danhSachGhe = await _gheDAL.LayGheDaDatAsync(maSuatChieu);
                if (lanTai != _lanTaiGhe || _suatDangChon?.MaSuatChieu != maSuatChieu)
                    return;

                _gheDaDat.Clear();
                foreach (string maGhe in danhSachGhe)
                {
                    if (!string.IsNullOrWhiteSpace(maGhe))
                        _gheDaDat.Add(maGhe.Trim().ToUpperInvariant());
                }

                _loiTaiGhe = false;
                lblTrangThai.Text = $"Đã tải sơ đồ ghế. Có {_gheDaDat.Count} ghế đã đặt trong suất chiếu này.";
            }
            catch (Exception ex)
            {
                if (lanTai != _lanTaiGhe)
                    return;

                _loiTaiGhe = true;
                _gheDangChon.Clear();
                lblTrangThai.Text = "Không tải được trạng thái ghế. Vui lòng kiểm tra kết nối rồi chọn lại suất chiếu.";
                MessageBox.Show(
                    "Không tải được ghế đã đặt từ Supabase. Để tránh đặt trùng ghế, chức năng xác nhận đang bị khóa.\n\n" + ex.Message,
                    "Lỗi tải sơ đồ ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (lanTai == _lanTaiGhe && !IsDisposed)
                {
                    _dangTaiGhe = false;
                    foreach (var pair in _nutGhe)
                        CapNhatMauNutGhe(pair.Value, pair.Key);
                    CapNhatTongTien();
                }
            }
        }

        private void cmbSoNguoi_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_gheDangChon.Count > 0)
            {
                _gheDangChon.Clear();
                lblTrangThai.Text = "Đã đổi số người; vui lòng chọn lại ghế.";
            }
            TaoSoDoGhe();
            CapNhatTongTien();
        }

        private void TaoSoDoGhe()
        {
            pnlSoDoGhe.SuspendLayout();
            pnlSoDoGhe.Controls.Clear();
            _nutGhe.Clear();

            string[] cacHang = { "A", "B", "C", "D", "E", "F", "G", "H" };
            int[] soGheMoiHang = { 14, 15, 15, 13, 14, 14, 15, 12 };

            var bang = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = cacHang.Length,
                Padding = new Padding(4, 2, 4, 2),
                BackColor = MauNenBanDo,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
                Margin = Padding.Empty
            };
            for (int r = 0; r < cacHang.Length; r++)
                bang.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / cacHang.Length));

            for (int r = 0; r < cacHang.Length; r++)
            {
                string hang = cacHang[r];
                var panelHang = new Panel { Dock = DockStyle.Fill, BackColor = MauNenBanDo, Margin = Padding.Empty };
                var flow = new FlowLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight,
                    BackColor = MauNenBanDo,
                    Margin = Padding.Empty,
                    Padding = Padding.Empty
                };

                for (int n = soGheMoiHang[r]; n >= 1; n--)
                {
                    string maGhe = hang + n.ToString("00");
                    bool laHangDoi = hang == "H";
                    int lePhai = laHangDoi && n % 2 == 1 ? 12 : 2; // khoảng cách tách từng sofa đôi
                    Button nut = new Button
                    {
                        Name = "btnGhe" + maGhe,
                        Text = maGhe,
                        Tag = maGhe,
                        Size = new Size(35, 30),
                        Margin = new Padding(2, 2, lePhai, 2),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 8.2F, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        UseVisualStyleBackColor = false,
                        AccessibleDescription = $"Ghế {maGhe} - {LoaiGhe(maGhe)} - {LayGiaGhe(maGhe):N0} VNĐ"
                    };
                    nut.FlatAppearance.BorderSize = 1;
                    nut.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 220);
                    nut.FlatAppearance.MouseOverBackColor = Color.FromArgb(94, 94, 105);
                    nut.Click += Ghe_Click;
                    _nutGhe[maGhe] = nut;
                    CapNhatMauNutGhe(nut, maGhe);
                    flow.Controls.Add(nut);
                }

                panelHang.Controls.Add(flow);
                void CanGiua()
                {
                    if (flow.IsDisposed || panelHang.IsDisposed) return;
                    flow.Left = Math.Max(0, (panelHang.ClientSize.Width - flow.Width) / 2);
                    flow.Top = Math.Max(0, (panelHang.ClientSize.Height - flow.Height) / 2);
                }
                panelHang.Resize += (s, e) => CanGiua();
                flow.SizeChanged += (s, e) => CanGiua();
                bang.Controls.Add(panelHang, 0, r);
            }

            pnlSoDoGhe.Controls.Add(bang);
            pnlSoDoGhe.ResumeLayout();
        }

        private void CapNhatMauNutGhe(Button nut, string maGhe)
        {
            bool daDat = _gheDaDat.Contains(maGhe);
            bool dangChon = _gheDangChon.Contains(maGhe);
            nut.Enabled = _suatDangChon != null && !_dangTaiGhe && !_loiTaiGhe && !daDat;
            nut.BackColor = daDat ? MauDaDat : dangChon ? MauDangChon : MauTheoLoaiGhe(maGhe);
            nut.ForeColor = Color.White;
            nut.FlatAppearance.BorderColor = dangChon ? Color.White : Color.FromArgb(210, 210, 220);
            nut.FlatAppearance.BorderSize = dangChon ? 2 : 1;
            nut.AccessibleDescription = daDat
                ? $"Ghế {maGhe} - đã đặt"
                : $"Ghế {maGhe} - {LoaiGhe(maGhe)} - {LayGiaGhe(maGhe):N0} VNĐ";
        }

        private static string LoaiGhe(string maGhe)
        {
            if (maGhe.StartsWith("H", StringComparison.OrdinalIgnoreCase)) return "Ghế đôi";
            char hang = char.ToUpperInvariant(maGhe[0]);
            return hang >= 'D' && hang <= 'G' ? "Ghế VIP" : "Ghế thường";
        }

        private static Color MauTheoLoaiGhe(string maGhe)
        {
            string loai = LoaiGhe(maGhe);
            return loai switch
            {
                "Ghế VIP" => MauVIP,
                "Ghế đôi" => MauGheDoi,
                _ => MauThuong
            };
        }

        private decimal LayGiaGhe(string maGhe)
        {
            decimal coBan = GiaVeCoBan;
            return LoaiGhe(maGhe) switch
            {
                "Ghế VIP" => Math.Round(coBan * HeSoGheVIP, 0),
                "Ghế đôi" => Math.Round(coBan * HeSoGheDoi, 0),
                _ => Math.Round(coBan * HeSoGheThuong, 0)
            };
        }

        // Có thể gọi phương thức này khi sau này DAL tải ghế đã đặt theo suất chiếu.
        public void CapNhatGheDaDat(IEnumerable<string> danhSachGheDaDat)
        {
            _gheDaDat.Clear();
            foreach (string maGhe in danhSachGheDaDat ?? Array.Empty<string>())
                if (!string.IsNullOrWhiteSpace(maGhe)) _gheDaDat.Add(maGhe.Trim().ToUpperInvariant());

            foreach (var pair in _nutGhe) CapNhatMauNutGhe(pair.Value, pair.Key);
            CapNhatTongTien();
        }

        private void Ghe_Click(object? sender, EventArgs e)
        {
            if (sender is not Button nut || nut.Tag is not string maGhe || _suatDangChon == null)
                return;
            if (_gheDaDat.Contains(maGhe))
            {
                MessageBox.Show("Ghế này đã được đặt.", "Chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_gheDangChon.Contains(maGhe))
            {
                _gheDangChon.Remove(maGhe);
            }
            else
            {
                if (_gheDangChon.Count >= SoNguoiCanDat)
                {
                    MessageBox.Show($"Bạn đã chọn đủ {SoNguoiCanDat} ghế. Hãy bỏ chọn ghế trước khi chọn ghế khác.",
                        "Chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                _gheDangChon.Add(maGhe);
            }

            CapNhatMauNutGhe(nut, maGhe);
            CapNhatTongTien();
        }

        private void CapNhatTongTien()
        {
            decimal giaCoBan = GiaVeCoBan;
            lblSoGhe.Text = $"Số ghế đã chọn: {_gheDangChon.Count}/{SoNguoiCanDat}";
            lblDanhSachGhe.Text = _gheDangChon.Count == 0
                ? "Chưa chọn ghế"
                : string.Join(", ", _gheDangChon.OrderBy(x => x[0]).ThenBy(x => int.Parse(x.Substring(1))));

            lblGiaThuong.Text = $"Ghế thường: {(giaCoBan * HeSoGheThuong):N0} VNĐ";
            lblGiaVIP.Text = $"Ghế VIP: {(giaCoBan * HeSoGheVIP):N0} VNĐ";
            lblGiaDoi.Text = $"Ghế đôi (mỗi chỗ): {(giaCoBan * HeSoGheDoi):N0} VNĐ";
            lblTongTien.Text = $"{GiaTamTinh:N0} VNĐ";
            lblSoNguoiHuongDan.Text = SoNguoiCanDat == 1
                ? "Chọn đúng 1 ghế để tiếp tục."
                : "Chọn đủ 4 ghế để tiếp tục.";
            btnXacNhan.Enabled = _suatDangChon != null && _gheDangChon.Count == SoNguoiCanDat;
        }

        private async void btnXacNhan_Click(object? sender, EventArgs e)
        {
            if (_suatDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn suất chiếu.", "Đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_dangTaiGhe || _loiTaiGhe)
            {
                MessageBox.Show("Chưa tải được trạng thái ghế. Vui lòng chọn lại suất chiếu hoặc kiểm tra kết nối Supabase.",
                    "Đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_gheDangChon.Count != SoNguoiCanDat)
            {
                MessageBox.Show($"Vui lòng chọn đủ {SoNguoiCanDat} ghế theo số người đặt.",
                    "Đặt vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int maSuatChieu = _suatDangChon.MaSuatChieu;
            List<string> gheDaXacNhan = GheDaChon.ToList();
            List<GheSuatChieuDat> danhSachLuu = gheDaXacNhan.Select(maGhe => new GheSuatChieuDat
            {
                MaSuatChieu = maSuatChieu,
                MaGhe = maGhe,
                GiaGhe = LayGiaGhe(maGhe),
                TrangThai = "da_dat"
            }).ToList();

            btnXacNhan.Enabled = false;
            lblTrangThai.Text = "Đang lưu ghế lên Supabase...";

            try
            {
                await _gheDAL.DatGheAsync(danhSachLuu);

                foreach (string maGhe in gheDaXacNhan)
                    _gheDaDat.Add(maGhe);

                lblTrangThai.Text = "Đã lưu ghế thành công.";
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _gheDangChon.Clear();
                int lanTai = ++_lanTaiGhe;
                _dangTaiGhe = true;
                _loiTaiGhe = false;
                TaoSoDoGhe();
                CapNhatTongTien();
                await TaiGheDaDatAsync(maSuatChieu, lanTai);
                MessageBox.Show(
                    "Không thể xác nhận ghế. Có thể ghế vừa được người khác đặt hoặc Supabase từ chối ghi dữ liệu.\n\n" + ex.Message,
                    "Không thể đặt ghế", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (!IsDisposed)
                    CapNhatTongTien();
            }
        }

        private void btnHuy_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private sealed class SuatChieuLuaChon
        {
            public SuatChieu Suat { get; }
            public int MaSuatChieu => Suat.MaSuatChieu;
            public string NoiDung => $"{Suat.NgayChieu:dd/MM/yyyy} | {Suat.GioBatDau:hh\\:mm} | {Suat.TenPhong} | {Suat.DinhDang} | {Suat.GiaVe:N0} VNĐ";
            public SuatChieuLuaChon(SuatChieu suat) => Suat = suat;
        }
    }
}
