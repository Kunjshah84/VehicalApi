using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VehicalApi.Entity;

public partial class RideBooking
{
    [Key]
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int VehicleId { get; set; }

    public int ShowroomId { get; set; }

    public DateOnly BookingDate { get; set; }

    public int SlotHour { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    public DateTime BookingCreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
