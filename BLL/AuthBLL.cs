using cinema_project.DAL;
using cinema_project.DAL.DTO;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.BLL
{
    public class AuthBLL
    {
        private readonly IAuthDAL _authDAL;
        private readonly ILogger<AuthBLL> _logger;

        public AuthBLL(SupabaseService supabaseService, ILoggerFactory? loggerFactory = null)
            : this(
                new AuthDAL(
                    supabaseService,
                    loggerFactory?.CreateLogger<AuthDAL>() ?? AppLogging.CreateLogger<AuthDAL>()),
                loggerFactory)
        {
        }

        public AuthBLL(IAuthDAL authDAL, ILoggerFactory? loggerFactory = null)
        {
            _logger = loggerFactory?.CreateLogger<AuthBLL>() ?? AppLogging.CreateLogger<AuthBLL>();
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
        public async Task ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            email = email?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email không được để trống.");
            if (string.IsNullOrEmpty(oldPassword))
                throw new ArgumentException("Vui lòng nhập mật khẩu hiện tại.");
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentException("Vui lòng nhập mật khẩu mới.");
            if (newPassword.Length < 6)
                throw new ArgumentException("Mật khẩu mới phải có ít nhất 6 ký tự.");
            if (newPassword == oldPassword)
                throw new ArgumentException("Mật khẩu mới phải khác mật khẩu hiện tại.");

            _logger.LogInformation("Đổi mật khẩu: {Email}", email);
            try
            {
                await _authDAL.ChangePasswordAsync(email, oldPassword, newPassword);
                _logger.LogInformation("Đổi mật khẩu thành công: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Đổi mật khẩu thất bại: {Email}", email);
                throw;
            }
        }
    }
}