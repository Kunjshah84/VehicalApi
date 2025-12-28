using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicalApi.Business.Manager.Interfaces;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;

namespace VehicalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpPost("vehicles")]
        public async Task<IActionResult> CreateVehicle(CreateVehicleDto dto)
        {
            // Console.WriteLine("1");

            var vehicle = await _managerService.CreateVehicleAsync(dto);

            Console.WriteLine("1");
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
            var vehicle = await _managerService.GetVehicleByIdAsync(id);
            return Ok(vehicle);
        }

        [HttpPost("vehicles/{vehicleId}/specifications")]
        public async Task<IActionResult> AddVehicleSpecification(
            int vehicleId,
            CreateVehicleSpecificationDto dto)
        {
            await _managerService.AddVehicleSpecificationAsync(vehicleId, dto);
            return Ok("Vehicle specifications added successfully");
        }

        [HttpPost("vehicles/{vehicleId}/images")]
        public async Task<IActionResult> AddVehicleImage(
            int vehicleId,
            CreateVehicleImageDto dto)
        {
            await _managerService.AddVehicleImageAsync(vehicleId, dto);
            return Ok("Vehicle image added successfully");
        }
    }
}
