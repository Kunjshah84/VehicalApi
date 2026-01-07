namespace VehicalApi.Dtos
{
    public class UserBookingDto
    {
        public int BookingId { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; } = null!;
        public string? VehiclePrimaryImage { get; set; }

        public DateOnly BookingDate { get; set; }
        public int SlotHour { get; set; }
        public string Status { get; set; } = null!;
        public DateTime BookingCreatedAt { get; set; }
    }
}
