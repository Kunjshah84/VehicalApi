using System.ComponentModel.DataAnnotations;

namespace VehicalApi.DTOs
{
    public class RefreshDto
    {
        [Required]
        public string AccessToken  { get; set; } = null!;        
        [Required]
        public string RefreshToken { get; set; } = null!;   
    }
}
