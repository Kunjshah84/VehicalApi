namespace VehicalApi.Dto;

public class DashboardVehicleDto
{
    public int VehicalId { get; set; }
    public string? VehicleName { get; set; }
    public string? Model { get; set; }
    public decimal BasePrice { get; set; }

    public int StockCount { get; set; }
    public string? ShortDescription { get; set; }

    public string? FuleType { get; set; }
    public string? BodyType { get; set; }
    public double? Engine { get; set; }

    public string? Thumbnail { get; set; }
}
