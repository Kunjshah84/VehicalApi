using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicalApi.Services.BookingServiecs;

namespace VehicalApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Manager")]
    public class AdminRideBookingController : ControllerBase
    {
        private readonly RideBookingService _rideBookingService;

        public AdminRideBookingController(RideBookingService rideBookingService)
        {
            _rideBookingService = rideBookingService;
        }

        [HttpGet("pending")]
        [Authorize]
        public async Task<IActionResult> GetPendingBookings()
        {
            var managerUserId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var showroomId = await _rideBookingService.GetShowroomIdByManagerIdAsync(managerUserId);
            if (showroomId == null)
                return NotFound("No showroom found assigned to this manager.");

            await _rideBookingService.AutoAcceptExpiredBookingsAsync();

            var bookings = await _rideBookingService
                .GetAdminPendingBookingsAsync(showroomId.Value);

            return Ok(bookings);
        }
        [HttpDelete("reject/{bookingId}")]
        public async Task<IActionResult> RejectBooking(int bookingId)
        {

            await _rideBookingService.AutoAcceptExpiredBookingsAsync();
            await _rideBookingService.RejectBookingAsync(bookingId);
            return Ok(new { message = "Booking rejected successfully." });
        }
        [HttpPut("accept/{bookingId}")]
        public async Task<IActionResult> AcceptBooking(int bookingId)
        {
            await _rideBookingService.AutoAcceptExpiredBookingsAsync();
            await _rideBookingService.AcceptBookingAsync(bookingId);
            return Ok(new { message = "You accepted the request successfully!" });
        }
    }
}

