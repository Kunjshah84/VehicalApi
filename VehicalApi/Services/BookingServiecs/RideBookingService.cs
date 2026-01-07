using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;
using VehicalApi.Dtos;
using VehicalApi.Entity;

namespace VehicalApi.Services.BookingServiecs

{
    public class RideBookingService
    {
        private readonly MyDbContext _context;

        public RideBookingService(MyDbContext context)
        {
            _context = context;
        }
        public async Task AutoAcceptExpiredBookingsAsync()
        {
            Console.WriteLine("service chalu che");

            var nowIst = DateTime.Now;

            var pendingBookings = await _context.RideBookings
                .Where(b =>
                    b.Status == "PENDING" &&
                    nowIst >= b.BookingCreatedAt.AddHours(2)
                )
                .ToListAsync();
            var expiredPendingBookings = pendingBookings
                .Where(b =>
                {
                    var slotTime = new DateTime(
                        b.BookingDate.Year,
                        b.BookingDate.Month,
                        b.BookingDate.Day,
                        b.SlotHour,
                        0,
                        0
                    );
                    return slotTime > nowIst;
                })
                .ToList();

            if (!expiredPendingBookings.Any())
            {
                Console.WriteLine("return happning here");
                return;
            }

            foreach (var booking in expiredPendingBookings)
            {
                booking.Status = "ACCEPTED";
                booking.UpdatedAt = nowIst;
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("completed the function of the auto except");
        }



        public async Task<CanBookVehicleResponseDto> CanUserBookVehicleAsync(
            int userId,
            int vehicleId
        )
        {
            var nowIst = DateTime.Now;

            var activeBookings = await _context.RideBookings
                .Where(b =>
                    b.UserId == userId &&
                    b.VehicleId == vehicleId &&
                    (b.Status == "PENDING" || b.Status == "ACCEPTED")
                )
                .OrderByDescending(b => b.BookingCreatedAt)
                .ToListAsync();

            var blockingBooking = activeBookings.FirstOrDefault(b =>
            {
                var slotTime = new DateTime(
                    b.BookingDate.Year,
                    b.BookingDate.Month,
                    b.BookingDate.Day,
                    b.SlotHour,
                    0,
                    0
                );

                return slotTime > nowIst;
            });

            if (blockingBooking == null)
            {
                return new CanBookVehicleResponseDto
                {
                    CanBook = true
                };
            }

            return new CanBookVehicleResponseDto
            {
                CanBook = false,
                BookingId = blockingBooking.BookingId,
                UserId = blockingBooking.UserId,
                VehicleId = blockingBooking.VehicleId,
                ShowroomId = blockingBooking.ShowroomId,
                BookingDate = blockingBooking.BookingDate,
                SlotHour = blockingBooking.SlotHour,
                Status = blockingBooking.Status,
                BookingCreatedAt = blockingBooking.BookingCreatedAt
            };
        }


        public async Task<List<int>> GetAvailableSlotsAsync(
            int vehicleId,
            DateOnly bookingDate
        )
        {
            var nowIst = DateTime.Now;
            var today = DateOnly.FromDateTime(nowIst);

            var allSlots = Enumerable.Range(9, 9).ToList();

            var bookedSlots = await _context.RideBookings
                .Where(b =>
                    b.VehicleId == vehicleId &&
                    b.BookingDate == bookingDate &&
                    (b.Status == "PENDING" || b.Status == "ACCEPTED")
                )
                .Select(b => b.SlotHour)
                .ToListAsync();

            var availableSlots = new List<int>();

            foreach (var slot in allSlots)
            {
                if (bookedSlots.Contains(slot))
                    continue;

                if (bookingDate == today)
                {
                    var slotDateTime = new DateTime(
                        bookingDate.Year,
                        bookingDate.Month,
                        bookingDate.Day,
                        slot,
                        0,
                        0
                    );

                    if (slotDateTime < nowIst.AddHours(2))
                        continue;
                }

                availableSlots.Add(slot);
            }

            return availableSlots;
        }

        public async Task<int> CreateBookingAsync(
            int userId,
            int vehicleId,
            int showroomId,
            DateOnly bookingDate,
            int slotHour
        )
        {
            var nowIst = DateTime.Now;

            await AutoAcceptExpiredBookingsAsync();

            var availableSlots = await GetAvailableSlotsAsync(vehicleId, bookingDate);
            if (!availableSlots.Contains(slotHour))
                throw new InvalidOperationException("Selected slot is not available.");

            var booking = new RideBooking
            {
                UserId = userId,
                VehicleId = vehicleId,
                ShowroomId = showroomId,
                BookingDate = bookingDate,
                SlotHour = slotHour,
                Status = "PENDING",
                BookingCreatedAt = nowIst
            };  

            _context.RideBookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking.BookingId;
        }

        // This is rh side of the admin pannel
        public async Task<List<PendingBookingDto>> GetAdminPendingBookingsAsync(int showroomId)
        {
            var nowIst = DateTime.Now;
            var nowDate = DateOnly.FromDateTime(nowIst);
            var nowHour = nowIst.Hour;

            await AutoAcceptExpiredBookingsAsync();

            return await (from b in _context.RideBookings
                        join u in _context.Users on b.UserId equals u.UserId
                        join v in _context.Vehicles on b.VehicleId equals v.VehicalId
                        where b.ShowroomId == showroomId &&
                                b.Status == "PENDING" &&
                                (b.BookingDate > nowDate || (b.BookingDate == nowDate && b.SlotHour > nowHour)) &&
                                nowIst < b.BookingCreatedAt.AddHours(2)
                        orderby b.BookingCreatedAt
                        select new PendingBookingDto
                        {
                            BookingId = b.BookingId,
                            CustomerName = u.FullName, 
                            VehicleName = v.VehicleName, 
                            BookingDate = b.BookingDate,
                            SlotHour = b.SlotHour,
                            Status = b.Status,
                            BookingCreatedAt = b.BookingCreatedAt
                        }).ToListAsync();
        }

        public async Task RejectBookingAsync(int bookingId)
        {
            var booking = await _context.RideBookings
            .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.Status == "Pending");

            if (booking == null)
                throw new InvalidOperationException("Booking not found. || the booking is already accepted");

            _context.RideBookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetShowroomIdByVehicleIdAsync(int vehicleId)
        {
            var showroomId = await _context.Vehicles
                .Where(v => v.VehicalId == vehicleId)
                .Select(v => v.ShowroomId)
                .FirstOrDefaultAsync();

            if (showroomId == 0)
                throw new InvalidOperationException("Showroom not found for this vehicle.");

            return showroomId;
        }

        public async Task<int?> GetShowroomIdByManagerIdAsync(int managerUserId)
        {
            var showroom = await _context.Showrooms
                .FirstOrDefaultAsync(s => s.ManagerId == managerUserId);
            return showroom?.ShowroomId; 
        }

        public async Task AcceptBookingAsync(int bookingId)
        {
            var booking = await _context.RideBookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.Status == "Pending");
            if (booking == null)
            {
                throw new InvalidOperationException("Booking not found or is no longer in pending status.");
            }
            booking.Status = "Accepted";
            await _context.SaveChangesAsync();
        }

        // In order to get the bookings that is for the perticular user
        public async Task<List<UserBookingDto>> GetUpcomingBookingsForUserAsync(int userId)
        {
            Console.WriteLine("Req reaches here");
            TimeZoneInfo istZone;
            try
            {
                istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
            catch
            {
                istZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }

            var istNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);

            var rawBookings = await (
                from booking in _context.RideBookings
                join vehicle in _context.Vehicles
                    on booking.VehicleId equals vehicle.VehicalId
                join image in _context.VehicleImages
                    on vehicle.VehicalId equals image.VehicleId into imageGroup
                from primaryImage in imageGroup
                    .OrderBy(i => i.SortOrder)
                    .Take(1)
                    .DefaultIfEmpty()

                where booking.UserId == userId

                select new UserBookingDto
                {
                    BookingId = booking.BookingId,
                    VehicleId = vehicle.VehicalId,
                    VehicleName = vehicle.VehicleName,
                    VehiclePrimaryImage = primaryImage != null
                        ? primaryImage.ImageLocation
                        : null,

                    BookingDate = booking.BookingDate,
                    SlotHour = booking.SlotHour,
                    Status = booking.Status,
                    BookingCreatedAt = booking.BookingCreatedAt
                }
            ).ToListAsync();
            return rawBookings
                .Where(b =>
                {
                    var bookingDateTime = b.BookingDate.ToDateTime(
                        TimeOnly.FromTimeSpan(TimeSpan.FromHours(b.SlotHour))
                    );
                    return bookingDateTime > istNow;
                })
                .OrderBy(b => b.BookingDate)
                .ThenBy(b => b.SlotHour)
                .ToList();
        }
    }
}
