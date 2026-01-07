using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicalApi.Dtos;
using VehicalApi.Entity;
using VehicalApi.Services.BookingServiecs;

namespace VehicalApi.Controllers
{
    [ApiController]
    [Route("api/ride-bookings")]
    [Authorize]
    public class RideBookingController : ControllerBase
    {
        private readonly RideBookingService _rideBookingService;

        public RideBookingController(RideBookingService rideBookingService)
        {
            _rideBookingService = rideBookingService;
        }
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId not found in token");

            return int.Parse(userIdClaim.Value);
        }

        [HttpGet("can-book")]
        [Authorize]
        public async Task<IActionResult> CanBook(
            [FromQuery] int vehicleId
        )
        {
            var userId = GetUserIdFromToken();

            await _rideBookingService.AutoAcceptExpiredBookingsAsync();

            var result = await _rideBookingService
                .CanUserBookVehicleAsync(userId, vehicleId);

            return Ok(result);
        }


        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] int vehicleId,
            [FromQuery] DateOnly date
        )
        {
            await _rideBookingService.AutoAcceptExpiredBookingsAsync();

            var slots = await _rideBookingService
                .GetAvailableSlotsAsync(vehicleId, date);

            return Ok(slots);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            [FromBody] CreateRideBookingRequest request
        )
        {
            try
            {
                var userId = GetUserIdFromToken();

                int ShowroomId=await _rideBookingService.GetShowroomIdByVehicleIdAsync(request.VehicleId);

                var bookingId = await _rideBookingService.CreateBookingAsync(
                    userId,
                    request.VehicleId,
                    ShowroomId,
                    request.BookingDate,
                    request.SlotHour
                );

                return Ok(new
                {
                    bookingId,
                    status = "PENDING"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-upcoming-bookings")]
        [Authorize]
        public async Task<IActionResult> GetMyUpcomingBookings()
        {
            var userId = GetUserIdFromToken();
            
            var bookings = await _rideBookingService
                .GetUpcomingBookingsForUserAsync(userId);

            return Ok(bookings);
        }
    }
}
