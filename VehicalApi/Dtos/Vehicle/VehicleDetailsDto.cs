public class VehicleDetailsDto
{
    public int VehicalId { get; set; }
    public string VehicleName { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int YearOfProduction { get; set; }
    public DateTime AgeInShowroom { get; set; }
    public decimal BasePrice { get; set; }
    public int StockCount { get; set; }
    public string ShortDescription { get; set; } = null!;

    public List<VehicleImageDto> Images { get; set; } = new();
    public List<VehicleSpecificationDto> Specifications { get; set; } = new();
}
