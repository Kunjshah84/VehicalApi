using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VehicalApi.Entity;

[Index("Email", Name = "UQ__Users__A9D1053468E7FA6E", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Role { get; set; } = null!;

    [Column("createdAt", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? RefreshToken { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RefreshTokenExpiry { get; set; }
    
    [StringLength(20)]
    [Unicode(false)]
    public string? Number { get; set; }  
    
    [InverseProperty("Manager")]
    public virtual ICollection<Showroom> Showrooms { get; set; } = new List<Showroom>();
}
