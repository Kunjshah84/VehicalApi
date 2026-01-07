namespace VehicalApi.Dtos
{
    public class PendingBookingDto
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; } 

        public string VehicleName { get; set; }         public DateOnly BookingDate { get; set; }
        public int SlotHour { get; set; }
        public string Status { get; set; }
        public DateTime BookingCreatedAt { get; set; }
    }
}
