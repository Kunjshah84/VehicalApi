namespace VehicalApi.DTOs.Auth
{
    public class AuthUserResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Number { get; set; }
        public string Role { get; set; } = null!;
    }
}
