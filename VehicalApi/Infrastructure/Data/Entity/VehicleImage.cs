using System;
using System.Collections.Generic;

namespace VehicalApi.Entity;

public partial class VehicleImage
{
    public int ImageId { get; set; }

    public int VehicleId { get; set; }

    public string ImageLocation { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual Vehicle Vehicle { get; set; } = null!;
}
