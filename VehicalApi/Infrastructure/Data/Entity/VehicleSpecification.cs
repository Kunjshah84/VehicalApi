using System;
using System.Collections.Generic;

namespace VehicalApi.Entity;

public partial class VehicleSpecification
{
    public int SpecificationId { get; set; }

    public int VehicalId { get; set; }

    public int Engine { get; set; }

    public string PowerOfvehical { get; set; } = null!;

    public string Torque { get; set; } = null!;

    public string FuleType { get; set; } = null!;

    public string Mileage { get; set; } = null!;

    public string BodyType { get; set; } = null!;

    public int SeatingCapacity { get; set; }

    public virtual Vehicle Vehical { get; set; } = null!;
}
