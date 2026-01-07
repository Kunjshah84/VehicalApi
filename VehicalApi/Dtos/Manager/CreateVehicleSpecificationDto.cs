using System.ComponentModel.DataAnnotations;

namespace VehicalApi.Dtos.VehicleSpecification
{
    public class CreateVehicleSpecificationDto
    {
        [Required]
        public int Engine { get; set; } 

        [Required]
        [MaxLength(100)]
        public string PowerOfVehical { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Torque { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string FuelType { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Mileage { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string BodyType { get; set; } = null!;

        [Required]
        public int SeatingCapacity { get; set; }
    }
}
