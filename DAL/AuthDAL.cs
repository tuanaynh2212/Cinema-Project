using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using cinema_project.DAL.DTO;
using cinema_project.Logging;
using Microsoft.Extensions.Logging;

namespace cinema_project.DAL
{
    public class AuthDAL
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
            _logger = logger ?? AppLogging.CreateLogger<AuthDAL>();
        }

        public virtual async Task DangKyAsync(string hoTen, string email, string matKhau)
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

        public virtual async Task<AuthLoginResult> DangNhapAsync(string email, string matKhau)
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
            if (result?.User == null)
            {
                throw new InvalidOperationException(
                    "Email hoặc mật khẩu không chính xác, hoặc tài khoản chưa xác minh email.");
            }

            if (string.IsNullOrWhiteSpace(result.User.Id) ||
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
                VaiTro = vaiTro,
                HoTen = result.User.UserMetadata?.HoTen ?? "",
                SoDienThoai = result.User.UserMetadata?.SoDienThoai ?? "",
                NgaySinh = DateTime.TryParse(
                    result.User.UserMetadata?.NgaySinh,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime ngaySinh)
                        ? ngaySinh
                        : null
            };

            PhienDangNhap.Luu(ketQua);
            _logger.LogInformation(
                "Đăng nhập thành công: {Email}, vai trò {VaiTro}",
                ketQua.Email,
                vaiTro);
            return ketQua;
        }

        public virtual async Task ChangePasswordAsync(
            string email,
            string oldPassword,
            string newPassword)
        {
            using var loginRequest = TaoRequest(
                HttpMethod.Post,
                "auth/v1/token?grant_type=password",
                new { email = email.Trim(), password = oldPassword });
            using HttpResponseMessage loginResponse = await GuiYeuCauAsync(loginRequest, "xác thực đổi mật khẩu");
            string loginBody = await loginResponse.Content.ReadAsStringAsync();

            AuthResponse? login = JsonSerializer.Deserialize<AuthResponse>(loginBody, _json);
            if (string.IsNullOrWhiteSpace(login?.AccessToken))
                throw new InvalidOperationException("Mật khẩu hiện tại không đúng.");

            using var updateRequest = TaoRequest(
                HttpMethod.Put,
                "auth/v1/user",
                new { password = newPassword },
                login.AccessToken);
            using HttpResponseMessage updateResponse = await GuiYeuCauAsync(updateRequest, "đổi mật khẩu");

            _logger.LogInformation("Đã gửi yêu cầu đổi mật khẩu: {Email}", email.Trim());
        }

        public virtual async Task UpdateProfileAsync(
            string accessToken,
            string hoTen,
            string soDienThoai,
            DateTime? ngaySinh)
        {
            using var request = TaoRequest(
                HttpMethod.Put,
                "auth/v1/user",
                new
                {
                    data = new
                    {
                        full_name = hoTen.Trim(),
                        phone = soDienThoai.Trim(),
                        birth_date = ngaySinh?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    }
                },
                accessToken);
            using HttpResponseMessage response = await GuiYeuCauAsync(request, "cập nhật hồ sơ");
        }

        private async Task<string> LayVaiTroAsync(string userId, string accessToken)
        {
            using var request = _supabase.CreateRequest(
                HttpMethod.Get,
                $"profiles?select=role&id=eq.{Uri.EscapeDataString(userId)}",
                accessToken);
            using HttpResponseMessage response = await GuiYeuCauAsync(request, "tải vai trò tài khoản");
            string noiDung = await response.Content.ReadAsStringAsync();

            List<RoleRow>? rows = JsonSerializer.Deserialize<List<RoleRow>>(noiDung, _json);
            return string.Equals(rows?.Count > 0 ? rows[0].Role : null, "admin", StringComparison.OrdinalIgnoreCase)
                ? "admin"
                : "user";
        }

        private async Task<string> KiemTraPhanHoiAsync(
            HttpResponseMessage response,
            string loiMacDinh,
            string thaoTac)
        {
            string noiDung = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Supabase từ chối yêu cầu {ThaoTac}: HTTP {Status}. {NoiDung}",
                    thaoTac,
                    (int)response.StatusCode,
                    noiDung);
                throw new InvalidOperationException(
                    $"{loiMacDinh} (HTTP {(int)response.StatusCode}). {noiDung}");
            }

            return noiDung;
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
            var stopwatch = Stopwatch.StartNew();
            _logger.LogDebug(
                "Bắt đầu {ThaoTac}: {Method} {Path}",
                thaoTac,
                request.Method.Method,
                request.RequestUri?.AbsolutePath);

            try
            {
                HttpResponseMessage response = await _http.SendAsync(request);
                _logger.LogDebug(
                    "Hoàn tất {ThaoTac}: HTTP {Status} trong {ElapsedMilliseconds} ms",
                    thaoTac,
                    (int)response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    ex,
                    "Không thể gửi yêu cầu Supabase khi {ThaoTac} sau {ElapsedMilliseconds} ms",
                    thaoTac,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(
                    ex,
                    "Yêu cầu Supabase hết thời gian chờ khi {ThaoTac} sau {ElapsedMilliseconds} ms",
                    thaoTac,
                    stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}