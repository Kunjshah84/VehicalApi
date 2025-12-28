namespace VehicalApi.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public AuthUserResponseDto User { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;
        public DateTimeOffset RefreshTokenExpiry { get; set; }
    }
}
