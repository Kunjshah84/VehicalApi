using System.ComponentModel.DataAnnotations;

namespace VehicalApi.DTOs
{
    public class RegisterDto
    {
        [Required, MinLength(3)]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        [Required, StringLength(20)]
        public string Number { get; set; } = null!; 

        public string Role { get; set; } = "User";   
    }
}
