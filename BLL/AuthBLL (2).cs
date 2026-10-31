using cinema_project.DAL;
using cinema_project.DAL.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace cinema_project.BLL
{
    public class AuthBLL
    {
        private readonly IAuthDAL _authDAL;
        private readonly ILogger<AuthBLL> _logger;

        public AuthBLL(SupabaseService supabaseService, ILoggerFactory? loggerFactory = null)
            : this(new AuthDAL(supabaseService, loggerFactory?.CreateLogger<AuthDAL>()), loggerFactory)
        {
        }

        public AuthBLL(IAuthDAL authDAL, ILoggerFactory? loggerFactory = null)
        {
            loggerFactory ??= NullLoggerFactory.Instance;
            _logger = loggerFactory.CreateLogger<AuthBLL>();
            _authDAL = authDAL ?? throw new ArgumentNullException(nameof(authDAL));
        }

        public async Task DangKyAsync(string hoTen, string email, string matKhau)
        {
            _logger.LogInformation("Đăng ký: {Email}", email);
            try
            {
                await _authDAL.DangKyAsync(hoTen, email, matKhau);
                _logger.LogInformation("Đăng ký thành công: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đăng ký thất bại: {Email}", email);
                throw;
            }
        }

        public async Task<AuthLoginResult> DangNhapAsync(string email, string matKhau)
        {
            _logger.LogInformation("Đăng nhập: {Email}", email);
            try
            {
                var result = await _authDAL.DangNhapAsync(email, matKhau);
                _logger.LogInformation("Đăng nhập thành công: {Email}", email);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Đăng nhập thất bại: {Email}", email);
                throw;
            }
        }
        public async Task DoiMatKhauAsync(string matKhauHienTai, string matKhauMoi)
        {
            _logger.LogInformation("Yêu cầu đổi mật khẩu cho tài khoản đang đăng nhập");
            try
            {
                await _authDAL.DoiMatKhauAsync(matKhauHienTai, matKhauMoi);
                _logger.LogInformation("Đổi mật khẩu thành công");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Đổi mật khẩu thất bại");
                throw;
            }
        }

    }
}