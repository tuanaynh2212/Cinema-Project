using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using cinema_project.DAL.DTO;
using Microsoft.Extensions.Logging;

namespace cinema_project.DAL
{
    public interface IAuthDAL
    {
        Task DangKyAsync(string hoTen, string email, string matKhau);
        Task<AuthLoginResult> DangNhapAsync(string email, string matKhau);
        Task DoiMatKhauAsync(string matKhauHienTai, string matKhauMoi);
    }

    public class AuthDAL : IAuthDAL
    {
        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(20) };
        private readonly SupabaseService supabase;
        private static readonly ILoggerFactory _defaultLoggerFactory = LoggerFactory.Create(b =>
        {
            b.AddConsole();
            b.SetMinimumLevel(LogLevel.Debug);
        });

        private readonly ILogger<AuthDAL> _logger;

        public AuthDAL(SupabaseService? supabaseServices, ILogger<AuthDAL>? logger = null)
        {
            _logger = logger ?? _defaultLoggerFactory.CreateLogger<AuthDAL>();
            supabase = supabaseServices ?? throw new ArgumentNullException(nameof(supabaseServices));
        }

        public async Task DangKyAsync(string hoTen, string email, string matKhau)
        {
            using var _ = await supabase.GuiAsync(HttpMethod.Post, "auth/v1/signup",
                new { email = email.Trim(), password = matKhau, data = new { full_name = hoTen.Trim() } },
                "Đăng ký không thành công.");

            _logger.LogInformation("Đăng ký thành công: {Email}", email.Trim());
        
        }

        public async Task<AuthLoginResult> DangNhapAsync(string email, string matKhau)
        {
            using var response = await supabase.GuiAsync(HttpMethod.Post, "auth/v1/token?grant_type=password",
                new { email = email.Trim(), password = matKhau },
                "Email hoặc mật khẩu không chính xác.");

            AuthResponse? result;
            try
            {
                result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Phản hồi đăng nhập của {Email} không đúng định dạng JSON", email.Trim());
                throw new InvalidOperationException("Phản hồi từ máy chủ không hợp lệ.", ex);
            }

            if (result?.User?.Id == null || string.IsNullOrWhiteSpace(result.AccessToken))
            {
                _logger.LogWarning("Đăng nhập {Email}: Supabase không trả về phiên đăng nhập", email.Trim());
                throw new InvalidOperationException(
                    "Supabase chưa trả về phiên đăng nhập. Nếu vừa đăng ký, hãy xác minh email rồi thử lại.");
            }

            string vaiTro = await LayVaiTroAsync(result.User.Id, result.AccessToken);
            _logger.LogInformation("Đăng nhập thành công: {Email}, vai trò {VaiTro}", email.Trim(), vaiTro);
             PhienDangNhap.Luu(new AuthLoginResult
            {
                UserId = result.User.Id,
                Email = result.User.Email ?? email.Trim(),
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken ?? "",
                VaiTro = vaiTro
            });   

            return new AuthLoginResult
            {
                UserId = result.User.Id,
                Email = result.User.Email ?? email.Trim(),
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken ?? "",
                VaiTro = vaiTro
            };
             
        }

        public async Task DoiMatKhauAsync(string matKhauHienTai, string matKhauMoi)
        {
            AuthLoginResult? phien = PhienDangNhap.NguoiDung;
            if (phien == null || string.IsNullOrWhiteSpace(phien.Email))
                throw new InvalidOperationException("Bạn cần đăng nhập trước khi đổi mật khẩu.");

            if (string.IsNullOrWhiteSpace(matKhauHienTai) || string.IsNullOrWhiteSpace(matKhauMoi))
                throw new ArgumentException("Vui lòng nhập đầy đủ mật khẩu.");

            // Xác minh mật khẩu hiện tại bằng Supabase Auth trước khi đổi.
            using var verifyResponse = await supabase.GuiAsync(
                HttpMethod.Post,
                "auth/v1/token?grant_type=password",
                new { email = phien.Email, password = matKhauHienTai },
                "Mật khẩu hiện tại không chính xác.");

            AuthResponse? verified;
            try
            {
                verified = await verifyResponse.Content.ReadFromJsonAsync<AuthResponse>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Phản hồi xác minh mật khẩu không đúng định dạng cho {UserId}", phien.UserId);
                throw new InvalidOperationException("Không đọc được phản hồi xác minh tài khoản.", ex);
            }

            if (verified?.User?.Id == null ||
                !string.Equals(verified.User.Id, phien.UserId, StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(verified.AccessToken))
            {
                _logger.LogWarning("Không xác minh được danh tính khi đổi mật khẩu cho {UserId}", phien.UserId);
                throw new InvalidOperationException("Không xác minh được tài khoản. Vui lòng đăng nhập lại.");
            }

            // Supabase Auth cập nhật mật khẩu cho người dùng hiện tại qua endpoint /auth/v1/user.
            using var updateResponse = await supabase.GuiAsync(
                HttpMethod.Put,
                "auth/v1/user",
                new { password = matKhauMoi },
                "Không thể đổi mật khẩu. Vui lòng thử lại.",
                verified.AccessToken);

            // Giữ phiên đăng nhập hoạt động với token mới nhận khi xác minh mật khẩu.
            PhienDangNhap.Luu(new AuthLoginResult
            {
                UserId = phien.UserId,
                Email = phien.Email,
                AccessToken = verified.AccessToken,
                RefreshToken = verified.RefreshToken ?? phien.RefreshToken,
                VaiTro = phien.VaiTro
            });

            _logger.LogInformation("Người dùng {UserId} đã đổi mật khẩu thành công", phien.UserId);
        }

        private async Task<string> LayVaiTroAsync(string userId, string accessToken)
        {
            try
            {
                using var response = await supabase.GuiAsync(HttpMethod.Get, $"rest/v1/profiles?id=eq.{userId}&select=role",
                    null, "Không lấy được quyền người dùng.", accessToken);

                var rows = await response.Content.ReadFromJsonAsync<List<RoleRow>>();
                string? role = rows?.FirstOrDefault()?.Role;

                if (role == null)
                    _logger.LogWarning("Không có profile cho {UserId}, dùng vai trò mặc định", userId);

                return role ?? "user";
            }
            catch (InvalidOperationException)
            {
                return "user";
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Dữ liệu profile của {UserId} không đúng định dạng", userId);
                return "user";
            }
        }

    }
}