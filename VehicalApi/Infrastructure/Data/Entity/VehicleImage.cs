using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VehicalApi.Entity;

[Table("VehicleImage")]
public partial class VehicleImage
{
    [Key]
    public int ImageId { get; set; }

    public int VehicleId { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string ImageLocation { get; set; } = null!;

    public int SortOrder { get; set; }

    [ForeignKey("VehicleId")]
    [InverseProperty("VehicleImages")]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
