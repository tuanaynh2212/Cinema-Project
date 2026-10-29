#nullable enable
using System;
using System.Windows.Forms;
using cinema_project.DAL;
using FormLichChieu = cinema_project.UI.Forms.frmLichChieu.frmLichChieu;

namespace cinema_project.UI.Forms.frmAdmin
{
    public sealed partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
            _lblEmailAdmin.Text = PhienDangNhap.NguoiDung?.Email ?? "Quản trị viên";

            _btnSidebarLichChieu.Click += BtnMoLichChieu_Click;
            _btnSidebarBaoCao.Click += BtnMoBaoCao_Click;
            _btnLichChieu.Click += BtnMoLichChieu_Click;
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
            using var form = new FormLichChieu();
            form.ShowDialog(this);
        }

        private void BtnMoBaoCao_Click(object? sender, EventArgs e)
        {
            using var form = new frmThongKeBaoCao();
            form.ShowDialog(this);
        }
    }
}
