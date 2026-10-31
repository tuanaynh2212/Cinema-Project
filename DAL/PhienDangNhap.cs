using cinema_project.DAL.DTO;

namespace cinema_project.DAL
{
    public static class PhienDangNhap
    {
        public static AuthLoginResult? NguoiDung { get; private set; }

        public static bool DaDangNhap => NguoiDung != null;

        public static void Luu(AuthLoginResult kq) => NguoiDung = kq;

        public static void DangXuat() => NguoiDung = null;
    }
}
