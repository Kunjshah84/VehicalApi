using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VehicalApi.Entity;

[Table("Vehicle")]
[Index("AgeInShowroom", Name = "IX_Vehicle_AgeInShowroom")]
[Index("BasePrice", Name = "IX_Vehicle_BasePrice")]
[Index("VehicleName", Name = "IX_Vehicle_VehicleName")]
public partial class Vehicle
{
    [Key]
    public int VehicalId { get; set; }

    public int ShowroomId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string VehicleName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Model { get; set; } = null!;

    public int YearOfProduction { get; set; }

    public int BasePrice { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AgeInShowroom { get; set; }

    [Column("stockCount")]
    public int StockCount { get; set; }

    [Column("shortDescription")]
    [StringLength(200)]
    [Unicode(false)]
    public string ShortDescription { get; set; } = null!;

    [ForeignKey("ShowroomId")]
    [InverseProperty("Vehicles")]
    public virtual Showroom Showroom { get; set; } = null!;

    [InverseProperty("Vehicle")]
    public virtual ICollection<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();

    [InverseProperty("Vehical")]
    public virtual ICollection<VehicleSpecification> VehicleSpecifications { get; set; } = new List<VehicleSpecification>();
}
