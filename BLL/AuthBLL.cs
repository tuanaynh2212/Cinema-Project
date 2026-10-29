using cinema_project.DAL;
using Microsoft.Extensions.Logging;

namespace cinema_project.BLL
{
    public class AuthBLL
    {
        private readonly AuthDAL _authDAL;

        public AuthBLL(SupabaseService supabaseService, ILogger<AuthDAL>? logger = null)
        {
            _authDAL = new AuthDAL(supabaseService, logger);
        }


        public Task DangKyAsync(string hoTen, string email, string matKhau) =>
            _authDAL.DangKyAsync(hoTen, email, matKhau);

        public Task<AuthLoginResult> DangNhapAsync(string email, string matKhau) =>
            _authDAL.DangNhapAsync(email, matKhau);
    }
}