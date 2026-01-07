using System;

namespace VehicalApi.Dtos.Manager;

public class ManagerVehicleDto
{
    public int VehicalId { get; set; }
    public required string VehicleName { get; set; }
    public DateTime AgeInShowroom { get; set; }
    public int StockCount { get; set; }

    public string? PrimaryImageUrl { get; set; }

}