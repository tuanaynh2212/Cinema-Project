using System.Text.Json.Serialization;

namespace cinema_project.DAL.DTO
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

        [JsonPropertyName("user_metadata")]
        public AuthUserMetadata? UserMetadata { get; set; }
    }

    public class AuthUserMetadata
    {
        [JsonPropertyName("full_name")]
        public string? HoTen { get; set; }

        [JsonPropertyName("phone")]
        public string? SoDienThoai { get; set; }

        [JsonPropertyName("birth_date")]
        public string? NgaySinh { get; set; }
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
        public string HoTen { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public DateTime? NgaySinh { get; set; }

        public bool LaAdmin => VaiTro == "admin";
    }

}