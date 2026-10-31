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