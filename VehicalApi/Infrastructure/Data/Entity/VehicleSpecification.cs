using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VehicalApi.Entity;

[Index("BodyType", Name = "IX_VehicleSpecifications_BodyType")]
[Index("Engine", Name = "IX_VehicleSpecifications_Engine")]
[Index("FuleType", Name = "IX_VehicleSpecifications_FuelType")]
[Index("VehicalId", Name = "IX_VehicleSpecifications_VehicleId")]
public partial class VehicleSpecification
{
    [Key]
    [Column("specificationId")]
    public int SpecificationId { get; set; }

    [Column("vehicalId")]
    public int VehicalId { get; set; }

    [Column("engine")]
    public int Engine { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string PowerOfvehical { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Torque { get; set; } = null!;

    [Column("fuleType")]
    [StringLength(20)]
    [Unicode(false)]
    public string FuleType { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Mileage { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string BodyType { get; set; } = null!;

    public int SeatingCapacity { get; set; }

    [ForeignKey("VehicalId")]
    [InverseProperty("VehicleSpecifications")]
    public virtual Vehicle Vehical { get; set; } = null!;
}
