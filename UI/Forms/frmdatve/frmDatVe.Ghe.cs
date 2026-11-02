#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cinema_project.UI.Forms.frmDatVe
{
    // Logic tạo và quản lý sơ đồ ghế được tách khỏi luồng đặt vé chính.
    public partial class frmDatVe
    {
        private readonly HashSet<string> _gheDangChon = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _gheDaDat = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Button> _nutGhe = new(StringComparer.OrdinalIgnoreCase);
        private bool _dangTaiGhe;
        private bool _loiTaiGhe;

        private static readonly string[] CacHangGhe = { "A", "B", "C", "D", "E", "F", "G", "H" };
        private static readonly int[] SoGheMoiHang = { 14, 15, 15, 13, 14, 14, 15, 12 };
        private static readonly Color MauNenBanDo = Color.FromArgb(250, 249, 251);
        private static readonly Color MauThuong = Color.FromArgb(119, 43, 191);
        private static readonly Color MauVIP = Color.FromArgb(239, 35, 55);
        private static readonly Color MauGheDoi = Color.FromArgb(226, 38, 145);
        private static readonly Color MauDaDat = Color.FromArgb(83, 86, 92);
        private static readonly Color MauDangChon = Color.FromArgb(0, 166, 153);

        private const decimal HeSoGheThuong = 1.00m;
        private const decimal HeSoGheVIP = 1.30m;
        private const decimal HeSoGheDoi = 1.50m;

        private static List<string> SapXepMaGhe(IEnumerable<string> ghe) =>
            ghe.OrderBy(x => x[0]).ThenBy(x => int.Parse(x.Substring(1))).ToList();

        private void TaoSoDoGhe()
        {
            pnlSoDoGhe.SuspendLayout();
            foreach (Control control in pnlSoDoGhe.Controls.Cast<Control>().ToArray())
                control.Dispose();
            pnlSoDoGhe.Controls.Clear();
            _nutGhe.Clear();

            var bang = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = CacHangGhe.Length,
                Padding = new Padding(4, 2, 4, 2),
                BackColor = MauNenBanDo,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
                Margin = Padding.Empty
            };
            for (int i = 0; i < CacHangGhe.Length; i++)
                bang.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / CacHangGhe.Length));

            for (int i = 0; i < CacHangGhe.Length; i++)
            {
                string hang = CacHangGhe[i];
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

                for (int soGhe = SoGheMoiHang[i]; soGhe >= 1; soGhe--)
                {
                    string maGhe = hang + soGhe.ToString("00");
                    int lePhai = hang == "H" && soGhe % 2 == 1 ? 12 : 2;
                    var nut = new Button
                    {
                        Name = "btnGhe" + maGhe,
                        Text = maGhe,
                        Tag = maGhe,
                        Size = new Size(35, 30),
                        Margin = new Padding(2, 2, lePhai, 2),
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 8.2F, FontStyle.Bold),
                        Cursor = Cursors.Hand,
                        UseVisualStyleBackColor = false
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
                    if (!flow.IsDisposed && !panelHang.IsDisposed)
                    {
                        flow.Left = Math.Max(0, (panelHang.ClientSize.Width - flow.Width) / 2);
                        flow.Top = Math.Max(0, (panelHang.ClientSize.Height - flow.Height) / 2);
                    }
                }
                panelHang.Resize += (_, _) => CanGiua();
                flow.SizeChanged += (_, _) => CanGiua();
                bang.Controls.Add(panelHang, 0, i);
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
            nut.AccessibleDescription = daDat ? $"Ghế {maGhe} - đã đặt" : $"Ghế {maGhe} - {LoaiGhe(maGhe)} - {LayGiaGhe(maGhe):N0} VNĐ";
        }

        private static string LoaiGhe(string maGhe)
        {
            if (maGhe.StartsWith("H", StringComparison.OrdinalIgnoreCase)) return "Ghế đôi";
            return char.ToUpperInvariant(maGhe[0]) is >= 'D' and <= 'G' ? "Ghế VIP" : "Ghế thường";
        }

        private static Color MauTheoLoaiGhe(string maGhe) => LoaiGhe(maGhe) switch
        {
            "Ghế VIP" => MauVIP,
            "Ghế đôi" => MauGheDoi,
            _ => MauThuong
        };

        private decimal LayGiaGhe(string maGhe) => Math.Round(GiaVeCoBan * (LoaiGhe(maGhe) switch
        {
            "Ghế VIP" => HeSoGheVIP,
            "Ghế đôi" => HeSoGheDoi,
            _ => HeSoGheThuong
        }), 0);

        public void CapNhatGheDaDat(IEnumerable<string>? danhSachGheDaDat)
        {
            _gheDaDat.Clear();
            foreach (string ghe in danhSachGheDaDat ?? Array.Empty<string>())
                if (!string.IsNullOrWhiteSpace(ghe)) _gheDaDat.Add(ghe.Trim().ToUpperInvariant());

            foreach (var ghe in _nutGhe) CapNhatMauNutGhe(ghe.Value, ghe.Key);
            CapNhatTongTien();
        }

        private void Ghe_Click(object? sender, EventArgs e)
        {
            if (sender is not Button nut || nut.Tag is not string maGhe || _suatDangChon == null) return;
            if (_gheDaDat.Contains(maGhe))
            {
                MessageBox.Show("Ghế này đã được đặt.", "Chọn ghế", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!_gheDangChon.Add(maGhe)) _gheDangChon.Remove(maGhe);

            CapNhatMauNutGhe(nut, maGhe);
            CapNhatTongTien();
        }
    }
}
