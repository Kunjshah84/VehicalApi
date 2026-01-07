using System;
using System.Collections.Generic;

namespace VehicalApi.Entity;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public string Number { get; set; } = null!;


    public virtual ICollection<Showroom> Showrooms { get; set; } = new List<Showroom>();
}
