using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cinema_project.DAL;

namespace cinema_project.UI
{
    public partial class frmThongKeBaoCao : Form
    {
        private readonly BaoCaoDoanhThuDAL _baoCaoDAL = new BaoCaoDoanhThuDAL();
        private List<BaoCaoDoanhThu> _duLieu = new List<BaoCaoDoanhThu>();

        public frmThongKeBaoCao()
        {
            InitializeComponent();
            dtpTuNgay.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpDenNgay.Value = DateTime.Today;
            Load += frmThongKeBaoCao_Load;
        }

        private async void frmThongKeBaoCao_Load(object sender, EventArgs e)
        {
            await TaiDuLieuAsync();
        }

        private async Task TaiDuLieuAsync()
        {
            btnThongKe.Enabled = false;
            btnXuat.Enabled = false;
            lblTrangThai.Text = "Đang tải dữ liệu từ Supabase...";

            try
            {
                _duLieu = await _baoCaoDAL.LayDoanhThuDaThanhToanAsync();
                ThongKe();
                lblTrangThai.Text = "Đã tải " + _duLieu.Count +
                    " giao dịch đã thanh toán từ Supabase.";
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "Không tải được dữ liệu.";
                MessageBox.Show(
                    "Không thể tải báo cáo doanh thu từ Supabase.\n\n" + ex.Message,
                    "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnThongKe.Enabled = true;
                btnXuat.Enabled = true;
            }
        }

        private List<BaoCaoDoanhThu> LayDuLieuTheoNgay()
        {
            return _duLieu
                .Where(x => x.NgayMua.Date >= dtpTuNgay.Value.Date &&
                            x.NgayMua.Date <= dtpDenNgay.Value.Date)
                .OrderBy(x => x.NgayMua)
                .ToList();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            ThongKe();
        }

        private void ThongKe()
        {
            if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
                return;
            }

            List<BaoCaoDoanhThu> ds = LayDuLieuTheoNgay();
            dgvBaoCao.Rows.Clear();

            foreach (BaoCaoDoanhThu item in ds)
            {
                dgvBaoCao.Rows.Add(
                    item.NgayMua.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    item.TenPhim,
                    item.SoVe,
                    item.DonGia.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")),
                    item.DoanhThu.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")));
            }

            lblTongDoanhThu.Text = "Tổng doanh thu: " +
                ds.Sum(x => x.DoanhThu).ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
            lblTongVe.Text = "Tổng số vé: " + ds.Sum(x => x.SoVe);
            lblTrangThai.Text = ds.Count == 0
                ? "Không có giao dịch đã thanh toán trong khoảng ngày đã chọn."
                : "Đã thống kê " + ds.Count + " giao dịch đã thanh toán.";
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            List<BaoCaoDoanhThu> ds = LayDuLieuTheoNgay();
            if (ds.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo.");
                return;
            }

            using (SaveFileDialog hopThoai = new SaveFileDialog())
            {
                hopThoai.Filter = "File CSV (*.csv)|*.csv";
                hopThoai.FileName = "BaoCaoDoanhThu.csv";

                if (hopThoai.ShowDialog() != DialogResult.OK)
                    return;

                StringBuilder noiDung = new StringBuilder();
                noiDung.AppendLine("Ngay mua,Ten phim,So ve,Don gia,Doanh thu");

                foreach (BaoCaoDoanhThu item in ds)
                {
                    noiDung.Append(item.NgayMua.ToLocalTime().ToString("dd/MM/yyyy HH:mm"))
                        .Append(",\"").Append((item.TenPhim ?? "").Replace("\"", "\"\""))
                        .Append("\",").Append(item.SoVe)
                        .Append(",").Append(item.DonGia.ToString(CultureInfo.InvariantCulture))
                        .Append(",").Append(item.DoanhThu.ToString(CultureInfo.InvariantCulture))
                        .AppendLine();
                }

                File.WriteAllText(hopThoai.FileName, "\uFEFF" + noiDung.ToString(), Encoding.UTF8);
                MessageBox.Show("Xuất báo cáo CSV thành công!");
            }
        }
    }
}
