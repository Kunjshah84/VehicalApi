using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VehicalApi.Entity;

[Index("ContactNumber", Name = "UQ__Showroom__570665C6F41D4DD4", IsUnique = true)]
[Index("ShowroomName", Name = "UQ__Showroom__C46B526F0EFC64D4", IsUnique = true)]
public partial class Showroom
{
    [Key]
    public int ShowroomId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ShowroomName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string ShowroomLocation { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string ContactNumber { get; set; } = null!;

    public int ManagerId { get; set; }

    [ForeignKey("ManagerId")]
    [InverseProperty("Showrooms")]
    public virtual User Manager { get; set; } = null!;

    [InverseProperty("Showroom")]
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
