using System.ComponentModel.DataAnnotations;

namespace VehicalApi.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
