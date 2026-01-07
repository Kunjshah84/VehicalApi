namespace VehicalApi.Dtos.Manager
{
    public class UpdateVehicleWithSpecDto
    {
        public string VehicleName { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int YearOfProduction { get; set; }
        public int BasePrice { get; set; }
        public int StockCount { get; set; }
        public string ShortDescription { get; set; } = null!;

        public int Engine { get; set; }
        public string PowerOfVehical { get; set; } = null!;
        public string Torque { get; set; } = null!;
        public string FuelType { get; set; } = null!;
        public string Mileage { get; set; } = null!;
        public string BodyType { get; set; } = null!;
        public int SeatingCapacity { get; set; }
    }
}
