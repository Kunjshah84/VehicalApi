using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;
using VehicalApi.Exceptions;

namespace VehicalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    // [Authorize]
    public class ManagerController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ManagerController(MyDbContext context)
        {
            _context = context;
        }


        [HttpPost("vehicles")]
        public async Task<IActionResult> CreateVehicle(CreateVehicleDto dto)
        {
            var showroomExists = await _context.Showrooms
                .AnyAsync(s => s.ShowroomId == dto.ShowroomId);

            // if (!showroomExists)
            //     return NotFound("Showroom not found");

            if (!showroomExists)
                throw new NotFoundException("Showroom not found");

            var vehicle = new Vehicle
            {
                ShowroomId = dto.ShowroomId,
                VehicleName = dto.VehicleName,
                Model = dto.Model,
                YearOfProduction = dto.YearOfProduction,
                BasePrice = dto.BasePrice,
                StockCount = dto.StockCount,
                ShortDescription = dto.ShortDescription
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetVehicleById),
                new { id = vehicle.VehicalId },
                vehicle
            );
        }

        [HttpGet("vehicles/{id}")]
        [Authorize]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicalId == id);

            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            return Ok(vehicle); 
        }


        [HttpPost("vehicles/{vehicleId}/specifications")]
        public async Task<IActionResult> AddVehicleSpecification(int vehicleId,CreateVehicleSpecificationDto dto)
        {
            // var vehicleExists = await _context.Vehicles
            //     .AnyAsync(v => v.VehicalId == vehicleId);

            if (!await _context.Vehicles.AnyAsync(v => v.VehicalId == vehicleId))
                throw new NotFoundException("Vehicle not found");

            if (await _context.VehicleSpecifications.AnyAsync(s => s.VehicalId == vehicleId))
                throw new BadRequestException("Specifications already exist for this vehicle");

            var specification = new VehicleSpecification
            {
                VehicalId = vehicleId,
                Engine = dto.Engine,
                PowerOfvehical = dto.PowerOfVehical,
                Torque = dto.Torque,
                FuleType = dto.FuelType,
                Mileage = dto.Mileage,
                BodyType = dto.BodyType,
                SeatingCapacity = dto.SeatingCapacity
            };

            _context.VehicleSpecifications.Add(specification);
            await _context.SaveChangesAsync();

            return Ok("Vehicle specifications added successfully");
        }


        [HttpPost("vehicles/{vehicleId}/images")]
        public async Task<IActionResult> AddVehicleImage(int vehicleId,CreateVehicleImageDto dto)
        {
            var vehicleExists = await _context.Vehicles
                .AnyAsync(v => v.VehicalId == vehicleId);

            if (!vehicleExists)
                throw new NotFoundException("Vehicle not found");

            var image = new VehicleImage
            {
                VehicleId = vehicleId,
                ImageLocation = dto.ImageLocation,
                SortOrder = dto.SortOrder
            };

            _context.VehicleImages.Add(image);
            await _context.SaveChangesAsync();

            return Ok("Vehicle image added successfully");
        }

    }
}
