using System.ComponentModel.DataAnnotations;

namespace VehicalApi.Dtos.Vehicle
{
    public class CreateVehicleDto
    {
        [Required]
        public int ShowroomId { get; set; }

        [Required]
        [MaxLength(50)]
        public string VehicleName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = null!;

        [Required]
        public int YearOfProduction { get; set; }

        [Required]
        public int BasePrice { get; set; }

        [Required]
        public int StockCount { get; set; }

        [Required]
        [MaxLength(200)]
        public string ShortDescription { get; set; } = null!;
    }
}
