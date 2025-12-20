using System.ComponentModel.DataAnnotations;

namespace VehicalApi.Dtos.VehicleImage
{
    public class CreateVehicleImageDto
    {
        [Required]
        [MaxLength(150)]
        public string ImageLocation { get; set; } = null!;

        [Required]
        public int SortOrder { get; set; }
    }
}
