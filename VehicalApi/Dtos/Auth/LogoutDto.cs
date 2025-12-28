using System.ComponentModel.DataAnnotations;

namespace VehicalApi.DTOs
{
    public class LogoutDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
    }
}
