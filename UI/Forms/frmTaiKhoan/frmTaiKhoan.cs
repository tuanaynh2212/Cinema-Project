using System.Drawing;
using System.Windows.Forms;
using cinema_project.DAL;
using FormVeCuaBan = cinema_project.UI.Forms.frmVeCuaBan.frmVeCuaBan;

namespace cinema_project.UI.Forms.frmTaiKhoan
{
    public partial class frmTaiKhoan : Form
    {
        public frmTaiKhoan()
        {
            InitializeComponent();
            string email = PhienDangNhap.NguoiDung?.Email ?? "minhanh@example.com";
            _lblEmailTaiKhoan.Text = email;
            _txtEmail.Text = email;
            _btnMoDanhSachVe.Click += (s, e) => new FormVeCuaBan().ShowDialog(this);
            _btnDangXuat.Click += BtnDangXuat_Click;
            ChuyenTrang(0);
        }

        private void BtnDangXuat_Click(object? sender, System.EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                this,
                "Bạn có chắc muốn đăng xuất khỏi tài khoản này không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            PhienDangNhap.DangXuat();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ChuyenTrang(int index)
        {
            for (int i = 0; i < _cacTrang.Length; i++)
            {
                bool dangChon = i == index;
                _cacTrang[i].Visible = dangChon;
                _nutDieuHuong[i].BackColor = dangChon ? Color.FromArgb(255, 242, 242) : Color.White;
                _nutDieuHuong[i].ForeColor = dangChon ? MauDo : MauPhu;
                _nutDieuHuong[i].FlatAppearance.BorderColor = dangChon ? MauDo : Color.White;
            }
            _cacTrang[index].BringToFront();
        }
    }
}