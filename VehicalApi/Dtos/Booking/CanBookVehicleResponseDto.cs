namespace VehicalApi.Dtos
{
    public class CanBookVehicleResponseDto
    {
        public bool CanBook { get; set; }

        public int? BookingId { get; set; }
        public int? UserId { get; set; }
        public int? VehicleId { get; set; }
        public int? ShowroomId { get; set; }

        public DateOnly? BookingDate { get; set; }
        public int? SlotHour { get; set; }

        public string? Status { get; set; }
        public DateTime? BookingCreatedAt { get; set; }
    }

}
