using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace cinema_project.DAL
{
    public interface IAuthDAL
    {
        Task DangKyAsync(string hoTen, string email, string matKhau);
        Task<AuthLoginResult> DangNhapAsync(string email, string matKhau);
        Task ChangePasswordAsync(string email, string oldPassword, string newPassword);
    }

    public sealed class AuthDAL : IAuthDAL
    {
        private static readonly HttpClient _http = new()
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        private static readonly JsonSerializerOptions _json = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly SupabaseService _supabase;
        private readonly ILogger<AuthDAL> _logger;

        public AuthDAL(
            SupabaseService? supabaseServices = null,
            ILogger<AuthDAL>? logger = null)
        {
            _supabase = supabaseServices ?? new SupabaseService();
            _logger = logger ?? NullLogger<AuthDAL>.Instance;
        }

        public async Task DangKyAsync(string hoTen, string email, string matKhau)
        {
            var body = new
            {
                email = email.Trim(),
                password = matKhau,
                data = new { full_name = hoTen.Trim() }
            };

            using var request = TaoRequest(HttpMethod.Post, "auth/v1/signup", body);
            using HttpResponseMessage response = await GuiYeuCauAsync(request, "đăng ký");
            await KiemTraPhanHoiAsync(response, "Đăng ký không thành công.", "đăng ký");

            _logger.LogInformation("Đăng ký thành công: {Email}", email.Trim());
        }

        public async Task<AuthLoginResult> DangNhapAsync(string email, string matKhau)
        {
            var body = new
            {
                email = email.Trim(),
                password = matKhau
            };

            using var request = TaoRequest(
                HttpMethod.Post,
                "auth/v1/token?grant_type=password",
                body);
            using HttpResponseMessage response = await GuiYeuCauAsync(request, "đăng nhập");
            string noiDung = await KiemTraPhanHoiAsync(
                response,
                "Email hoặc mật khẩu không chính xác.",
                "đăng nhập");

            AuthResponse? result = JsonSerializer.Deserialize<AuthResponse>(noiDung, _json);
            if (result?.User == null ||
                string.IsNullOrWhiteSpace(result.User.Id) ||
                string.IsNullOrWhiteSpace(result.AccessToken))
            {
                throw new InvalidOperationException(
                    "Supabase chưa trả về phiên đăng nhập. Nếu vừa đăng ký, hãy xác minh email rồi thử lại.");
            }

            string vaiTro = await LayVaiTroAsync(result.User.Id, result.AccessToken);
            var ketQua = new AuthLoginResult
            {
                UserId = result.User.Id,
                Email = result.User.Email ?? email.Trim(),
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken ?? "",
                VaiTro = vaiTro
            };

            PhienDangNhap.Luu(ketQua);
            _logger.LogInformation(
                "Đăng nhập thành công: {Email}, vai trò {VaiTro}",
                ketQua.Email,
                vaiTro);
            return ketQua;
        }

        public async Task ChangePasswordAsync(
            string email,
            string oldPassword,
            string newPassword)
        {
            using var loginRequest = TaoRequest(
                HttpMethod.Post,
                "auth/v1/token?grant_type=password",
                new { email = email.Trim(), password = oldPassword });
            using HttpResponseMessage loginResponse = await GuiYeuCauAsync(loginRequest, "xác thực đổi mật khẩu");
            string loginBody = await KiemTraPhanHoiAsync(
                loginResponse,
                "Mật khẩu hiện tại không đúng.",
                "xác thực đổi mật khẩu");

            AuthResponse? login = JsonSerializer.Deserialize<AuthResponse>(loginBody, _json);
            if (string.IsNullOrWhiteSpace(login?.AccessToken))
                throw new InvalidOperationException("Supabase chưa trả về phiên xác thực để đổi mật khẩu.");

            using var updateRequest = TaoRequest(
                HttpMethod.Put,
                "auth/v1/user",
                new { password = newPassword },
                login.AccessToken);
            using HttpResponseMessage updateResponse = await GuiYeuCauAsync(updateRequest, "đổi mật khẩu");
            await KiemTraPhanHoiAsync(
                updateResponse,
                "Đổi mật khẩu không thành công.",
                "đổi mật khẩu");

            _logger.LogInformation("Đổi mật khẩu thành công: {Email}", email.Trim());
        }

        private async Task<string> LayVaiTroAsync(string userId, string accessToken)
        {
            using var request = _supabase.CreateRequest(
                HttpMethod.Get,
                $"profiles?select=role&id=eq.{Uri.EscapeDataString(userId)}",
                accessToken);
            using HttpResponseMessage response = await GuiYeuCauAsync(request, "tải vai trò tài khoản");
            string noiDung = await KiemTraPhanHoiAsync(
                response,
                "Không thể tải vai trò tài khoản.",
                "tải vai trò tài khoản");

            List<RoleRow>? rows = JsonSerializer.Deserialize<List<RoleRow>>(noiDung, _json);
            return string.Equals(rows?.Count > 0 ? rows[0].Role : null, "admin", StringComparison.OrdinalIgnoreCase)
                ? "admin"
                : "user";
        }

        private static HttpRequestMessage TaoRequest(
            HttpMethod method,
            string path,
            object? body = null,
            string? accessToken = null)
        {
            if (string.IsNullOrWhiteSpace(SupabaseService.SupabaseUrl) ||
                string.IsNullOrWhiteSpace(SupabaseService.SupabaseKey))
            {
                throw new InvalidOperationException(
                    "Bạn cần cấu hình SupabaseUrl và SupabaseKey trong SupabaseService.");
            }

            var request = new HttpRequestMessage(
                method,
                $"{SupabaseService.SupabaseUrl.TrimEnd('/')}/{path.TrimStart('/')}");

            request.Headers.TryAddWithoutValidation("apikey", SupabaseService.SupabaseKey);
            if (!string.IsNullOrWhiteSpace(accessToken))
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            if (body != null)
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json");

            return request;
        }

        private async Task<HttpResponseMessage> GuiYeuCauAsync(
            HttpRequestMessage request,
            string thaoTac)
        {
            try
            {
                return await _http.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gửi yêu cầu Supabase khi {ThaoTac}", thaoTac);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Yêu cầu Supabase hết thời gian chờ khi {ThaoTac}", thaoTac);
                throw;
            }
        }

        private async Task<string> KiemTraPhanHoiAsync(
            HttpResponseMessage response,
            string loiMacDinh,
            string thaoTac)
        {
            string noiDung = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                string loi = DocLoi(noiDung, loiMacDinh);
                _logger.LogWarning(
                    "Supabase từ chối yêu cầu {ThaoTac}: HTTP {Status}. {Loi}",
                    thaoTac,
                    (int)response.StatusCode,
                    loi);
                throw new InvalidOperationException(loi);
            }

            return noiDung;
        }

        private static string DocLoi(string json, string macDinh)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(json);
                foreach (string key in new[] { "msg", "message", "error_description", "error" })
                {
                    if (doc.RootElement.TryGetProperty(key, out JsonElement value) &&
                        value.ValueKind == JsonValueKind.String &&
                        !string.IsNullOrWhiteSpace(value.GetString()))
                    {
                        return value.GetString()!;
                    }
                }
            }
            catch (JsonException)
            {
            }

            return string.IsNullOrWhiteSpace(json) ? macDinh : $"{macDinh}\n{json}";
        }
    }
}
