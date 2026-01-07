using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicalApi.Business.Manager.Interfaces;
using VehicalApi.Dtos.Manager;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;

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

        [HttpPost("vehicleadd")]
        public async Task<IActionResult> CreateVehicle(
            CreateVehicleWithSpecDto dto)
        {
            Console.WriteLine("Here in the cotroller");
            if (!ModelState.IsValid)
            {
                return BadRequest("Bhai kaik to seen che");
            }

            var managerUserId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var vehicle = await _managerService
                .CreateVehicleWithSpecificationAsync(managerUserId, dto);

            return Ok(new
            {
                vehicleId = vehicle,
                message = "Vehicle created successfully"
            });
        }

        [HttpGet("vehicles/{id}")]
        [Authorize]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _managerService.GetVehicleByIdAsync(id);
            return Ok(vehicle);
        }

        // [HttpPost("vehicles/{vehicleId}/specifications")]
        // public async Task<IActionResult> AddVehicleSpecification(
        //     int vehicleId,
        //     CreateVehicleSpecificationDto dto)
        // {
        //     await _managerService.AddVehicleSpecificationAsync(vehicleId, dto);
        //     return Ok("Vehicle specifications added successfully");
        // }

        [HttpPost("vehicles/{vehicleId}/images")]
        public async Task<IActionResult> SaveVehicleImages(
            int vehicleId,
            SaveVehicleImagesRequestDto dto
        )
        {
            var managerUserId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

           await _managerService.AddVehicleImageAsync(managerUserId,vehicleId,dto.Images);

            return Ok(new
            {
                message = "Vehicle images saved successfully",
            });
        }



        [HttpGet("vehicles")]
        public async Task<IActionResult> GetMyShowroomVehicles()
        {
            var managerUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var vehicles = await _managerService.GetVehiclesByManagerAsync(managerUserId);
            return Ok(vehicles);
        }

        [HttpDelete("dltvehicle/{vehicleId}")] 
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            var managerUserId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            await _managerService.DeleteVehicleAsync(managerUserId, vehicleId);

            return Ok(new
            {
                message = $"Vehicle deleted successfully with vehicle ID {vehicleId}"
            });
        }

        [HttpPut("editvehicle/{vehicleId}")]
        public async Task<IActionResult> EditVehicle(
            int vehicleId,
            UpdateVehicleWithSpecDto dto)
        {
            var managerUserId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            Console.WriteLine("In the cotroller");

            await _managerService.UpdateVehicleAsync(managerUserId,vehicleId,dto);

            return Ok(new
            {
                message = $"Vehicle updated successfully with vehicle ID {vehicleId}"
            });
        }

        [HttpGet]
            [Route("vehicles/getImg/{vehicleId}/images")]
        public async Task<IActionResult> GetVehicleImages(int vehicleId)
        {
            var managerId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );
            var images = await _managerService.GetVehicleImagesAsync(vehicleId);
            return Ok(new { message = "Vehicle image added successfully" , images });
        }
    }
}
