using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cinema_project.DAL
{
    public sealed class AuthLoginResult
    {
        public string UserId { get; init; } = "";
        public string Email { get; init; } = "";
        public string AccessToken { get; init; } = "";
        public string RefreshToken { get; init; } = "";
    }

    // Chỉ xử lý Supabase Auth đăng ký/đăng nhập, không làm phân quyền.
    public sealed class AuthDAL
    {
        private static readonly HttpClient _http = new()
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        private static readonly JsonSerializerOptions _json = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task DangKyAsync(string hoTen, string email, string matKhau)
        {
            var body = new
            {
                email = email.Trim(),
                password = matKhau,
                data = new { full_name = hoTen.Trim() }
            };

            using var request = TaoRequest("signup");
            request.Content = new StringContent(
                JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _http.SendAsync(request);
            string noiDung = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    DocLoi(noiDung, "Đăng ký không thành công."));
        }

        public async Task<AuthLoginResult> DangNhapAsync(string email, string matKhau)
        {
            var body = new
            {
                email = email.Trim(),
                password = matKhau
            };

            using var request = TaoRequest("token?grant_type=password");
            request.Content = new StringContent(
                JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _http.SendAsync(request);
            string noiDung = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(
                    DocLoi(noiDung, "Email hoặc mật khẩu không chính xác."));

            AuthResponse? result = JsonSerializer.Deserialize<AuthResponse>(noiDung, _json);

            if (result?.User == null || string.IsNullOrWhiteSpace(result.AccessToken))
                throw new InvalidOperationException(
                    "Supabase chưa trả về phiên đăng nhập. Nếu vừa đăng ký, hãy xác minh email rồi thử lại.");

            return new AuthLoginResult
            {
                UserId = result.User.Id ?? "",
                Email = result.User.Email ?? email.Trim(),
                AccessToken = result.AccessToken ?? "",
                RefreshToken = result.RefreshToken ?? ""
            };
        }

        private static HttpRequestMessage TaoRequest(string path)
        {
            if (string.IsNullOrWhiteSpace(SupabaseService.SupabaseUrl) ||
                string.IsNullOrWhiteSpace(SupabaseService.SupabaseKey))
            {
                throw new InvalidOperationException(
                    "Bạn cần cấu hình SupabaseUrl và SupabaseKey trong SupabaseService.");
            }

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{SupabaseService.SupabaseUrl.TrimEnd('/')}/auth/v1/{path}");

            request.Headers.TryAddWithoutValidation("apikey", SupabaseService.SupabaseKey);
            return request;
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
                        return value.GetString()!;
                }
            }
            catch
            {
                // Nếu response không phải JSON thì dùng thông báo mặc định.
            }

            return string.IsNullOrWhiteSpace(json) ? macDinh : $"{macDinh}\n{json}";
        }

        private sealed class AuthResponse
        {
            [JsonPropertyName("access_token")]
            public string? AccessToken { get; set; }

            [JsonPropertyName("refresh_token")]
            public string? RefreshToken { get; set; }

            [JsonPropertyName("user")]
            public AuthUser? User { get; set; }
        }

        private sealed class AuthUser
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; }

            [JsonPropertyName("email")]
            public string? Email { get; set; }
        }
    }
}
