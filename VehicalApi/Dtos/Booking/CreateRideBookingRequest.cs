namespace VehicalApi.Dtos
{
    public class CreateRideBookingRequest
    {
        public int VehicleId { get; set; }
        public DateOnly BookingDate { get; set; }
        public int SlotHour { get; set; }
    }
}
