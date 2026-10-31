#nullable enable
using System;
using System.Windows.Forms;
using cinema_project.DAL;
using FormLichChieu = cinema_project.UI.Forms.frmLichChieu.frmLichChieu;
using FormVeCuaBan = cinema_project.UI.Forms.frmVeCuaBan.frmVeCuaBan;

namespace cinema_project.UI.Forms.frmAdmin
{
    public sealed partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
            _lblEmailAdmin.Text = PhienDangNhap.NguoiDung?.Email ?? "Quản trị viên";

            _btnSidebarLichChieu.Click += BtnMoLichChieu_Click;
            _btnLichChieu.Click += BtnMoLichChieu_Click;
            _btnSidebarVe.Click += BtnMoVe_Click;
            _btnVe.Click += BtnMoVe_Click;
            _btnSidebarBaoCao.Click += BtnMoBaoCao_Click;
            _btnBaoCao.Click += BtnMoBaoCao_Click;
            Shown += FrmAdmin_Shown;
        }

        private void FrmAdmin_Shown(object? sender, EventArgs e)
        {
            if (PhienDangNhap.NguoiDung?.LaAdmin == true)
                return;

            MessageBox.Show(
                this,
                "Chỉ tài khoản quản trị viên mới có thể mở khu vực này.",
                "Không có quyền truy cập",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            Close();
        }


        private void BtnMoLichChieu_Click(object? sender, EventArgs e)
        {
            if (PhienDangNhap.NguoiDung?.LaAdmin != true)
            {
                MessageBox.Show(
                    this,
                    "Chỉ tài khoản quản trị viên mới được quản lý lịch chiếu.",
                    "Không có quyền truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _lblTieuDeTrang.Text = "Quản lý lịch chiếu";
            using var form = new FormLichChieu();
            form.ShowDialog(this);
        }

        private void BtnMoVe_Click(object? sender, EventArgs e)
        {
            if (PhienDangNhap.NguoiDung?.LaAdmin != true)
            {
                MessageBox.Show(
                    this,
                    "Chỉ tài khoản quản trị viên mới được mở chức năng vé trong khu vực quản trị.",
                    "Không có quyền truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _lblTieuDeTrang.Text = "Vé đã đặt";
            using var form = new FormVeCuaBan();
            form.ShowDialog(this);
        }

        private void BtnMoBaoCao_Click(object? sender, EventArgs e)
        {
            if (PhienDangNhap.NguoiDung?.LaAdmin != true)
            {
                MessageBox.Show(
                    this,
                    "Bạn không có quyền xem thống kê doanh thu.",
                    "Không có quyền truy cập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _lblTieuDeTrang.Text = "Thống kê & báo cáo";
            using var form = new frmThongKeBaoCao();
            form.ShowDialog(this);
        }
    }
}
