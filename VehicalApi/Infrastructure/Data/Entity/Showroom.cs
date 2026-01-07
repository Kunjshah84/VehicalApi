using System;
using System.Collections.Generic;

namespace VehicalApi.Entity;

public partial class Showroom
{
    public int ShowroomId { get; set; }

    public string ShowroomName { get; set; } = null!;

    public string ShowroomLocation { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public int ManagerId { get; set; }

    public virtual User Manager { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
