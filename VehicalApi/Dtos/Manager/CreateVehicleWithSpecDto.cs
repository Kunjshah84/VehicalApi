using VehicalApi.Dtos.VehicleSpecification;

namespace VehicalApi.Dtos.Vehicle
{
    public class CreateVehicleWithSpecDto
    {
        public string VehicleName { get; set; } = null!;
        public string Model { get; set; } = null!;

        public int YearOfProduction { get; set; }
        public DateTime AgeInShowroom { get; set; }
        public int BasePrice { get; set; }
        public int StockCount { get; set; }
        public string ShortDescription { get; set; } = null!;

        public CreateVehicleSpecificationDto Specification { get; set; } = null!;
    }
}
