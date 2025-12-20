using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;
using VehicalApi.Exceptions;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly MyDbContext _context;

    public VehiclesController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardVehicles()
    {
        var vehicles = await _context.Vehicles
            .Include(v => v.VehicleImages)
            .AsNoTracking()
            .Select(v => new
            {
                v.VehicalId,
                v.VehicleName,
                v.Model,
                v.YearOfProduction,
                v.BasePrice,
                v.StockCount,
                v.ShortDescription,

                Images = v.VehicleImages
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new
                    {
                        i.ImageLocation,
                        i.SortOrder
                    })
            })
            .ToListAsync();

        return Ok(vehicles);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetVehicleDetails(int id)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.VehicleImages)
            .Include(v => v.VehicleSpecifications)
            .AsNoTracking()
            .Where(v => v.VehicalId == id)
            .Select(v => new
            {
                v.VehicalId,
                v.VehicleName,
                v.Model,
                v.YearOfProduction,
                v.BasePrice,
                v.StockCount,
                v.ShortDescription,

                Images = v.VehicleImages
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new
                    {
                        i.ImageLocation,
                        i.SortOrder
                    }),

                Specifications = v.VehicleSpecifications.Select(s => new
                {
                    s.Engine,
                    s.PowerOfvehical,
                    s.Torque,
                    s.FuleType,
                    s.Mileage,
                    s.BodyType,
                    s.SeatingCapacity
                })
            })
            .FirstOrDefaultAsync();

        if (vehicle == null)
            throw new NotFoundException("not found");

        return Ok(vehicle);
    }

}
