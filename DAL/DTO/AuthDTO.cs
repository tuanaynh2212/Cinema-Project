using System.Text.Json.Serialization;

namespace cinema_project.DAL
{
    public class AuthResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("user")]
        public AuthUser? User { get; set; }
    }

    public class AuthUser
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class RoleRow
    {
        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }

    public class  AuthLoginResult
    {
        public string UserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public string VaiTro { get; set; } = "user";

        public bool LaAdmin => VaiTro == "admin";
    }

}