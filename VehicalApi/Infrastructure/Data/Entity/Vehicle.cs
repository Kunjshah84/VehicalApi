using System;
using System.Collections.Generic;

namespace VehicalApi.Entity;

public partial class Vehicle
{
    public int VehicalId { get; set; }

    public int ShowroomId { get; set; }

    public string VehicleName { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int YearOfProduction { get; set; }

    public int BasePrice { get; set; }

    public DateTime AgeInShowroom { get; set; }

    public int StockCount { get; set; }

    public string ShortDescription { get; set; } = null!;

    public virtual Showroom Showroom { get; set; } = null!;

    public virtual ICollection<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();

    public virtual ICollection<VehicleSpecification> VehicleSpecifications { get; set; } = new List<VehicleSpecification>();
}
