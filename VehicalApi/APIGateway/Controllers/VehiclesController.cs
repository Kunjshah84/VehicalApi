using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicalApi.Business.Vehicles.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleQueryService _vehicleQueryService;

    public VehiclesController(IVehicleQueryService vehicleQueryService)
    {
        _vehicleQueryService = vehicleQueryService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardVehicles(
        [FromQuery] string? search,
        [FromQuery] int? minPrice,
        [FromQuery] int? maxPrice,
        [FromQuery] string? fuelTypes,
        [FromQuery] string? bodyTypes,
        [FromQuery] double? minEngine,
        [FromQuery] double? maxEngine,
        [FromQuery] string? sortBy
    )
    {
        Console.WriteLine($"IsAuthenticated: {User.Identity?.IsAuthenticated}");

        var vehicles = await _vehicleQueryService.GetDashboardVehiclesAsync(
            search,
            minPrice,
            maxPrice,
            fuelTypes,
            bodyTypes,
            minEngine,
            maxEngine,
            sortBy
        );

        return Ok(vehicles);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetVehicleDetails(int id)
    {
        var vehicle = await _vehicleQueryService.GetVehicleDetailsAsync(id);
        return Ok(vehicle);
    }
}
