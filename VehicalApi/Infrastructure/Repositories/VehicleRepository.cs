using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;
using VehicalApi.Domain.Vehicles.Interfaces;
using VehicalApi.Dto;
using VehicalApi.Exceptions;

namespace VehicalApi.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly MyDbContext _context;

        public VehicleRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<DashboardVehicleDto>> GetDashboardVehiclesAsync(
            string? search,
            int? minPrice,
            int? maxPrice,
            string? fuelTypes,
            string? bodyTypes,
            double? minEngine,
            double? maxEngine,
            string? sortBy
        )
        {
            var vehicles = new List<DashboardVehicleDto>();

            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "GetVehiclesForDashboard";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Search", search ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@MinPrice", minPrice ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@MaxPrice", maxPrice ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@FuelTypes", fuelTypes ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@BodyTypes", bodyTypes ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@MinEngine", minEngine ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@MaxEngine", maxEngine ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@SortBy", sortBy ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Page", 1));
            command.Parameters.Add(new SqlParameter("@PageSize", 10));

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                vehicles.Add(new DashboardVehicleDto
                {
                    VehicalId = Convert.ToInt32(reader["vehicalId"]),
                    VehicleName = reader["VehicleName"]?.ToString(),
                    Model = reader["Model"]?.ToString(),
                    BasePrice = Convert.ToDecimal(reader["BasePrice"]),
                    StockCount = Convert.ToInt32(reader["StockCount"]),
                    ShortDescription = reader["ShortDescription"]?.ToString(),
                    FuleType = reader["fuleType"]?.ToString(),
                    BodyType = reader["BodyType"]?.ToString(),
                    Engine = reader["engine"] == DBNull.Value
                        ? null
                        : Convert.ToDouble(reader["engine"]),
                    Thumbnail = reader["Thumbnail"]?.ToString()
                });
            }

            return vehicles;
        }

        public async Task<VehicleDetailsDto> GetVehicleDetailsAsync(int id)
        {
            var vehicle = await _context.Vehicles
                .AsNoTracking()
                .Where(v => v.VehicalId == id)
                .Select(v => new VehicleDetailsDto
                {
                    VehicalId = v.VehicalId,
                    VehicleName = v.VehicleName,
                    Model = v.Model,
                    YearOfProduction = v.YearOfProduction,
                    
                    AgeInShowroom = v.AgeInShowroom, 
                    BasePrice = v.BasePrice,
                    
                    StockCount = v.StockCount,
                    ShortDescription = v.ShortDescription,

                    Images = v.VehicleImages
                        .OrderBy(i => i.SortOrder)
                        .Select(i => new VehicleImageDto
                        {
                            ImageLocation = i.ImageLocation,
                            SortOrder = i.SortOrder
                        })
                        .ToList(),

                    Specifications = v.VehicleSpecifications
                        .Select(s => new VehicleSpecificationDto
                        {
                            Engine = s.Engine,
                            PowerOfvehical = s.PowerOfvehical,
                            Torque = s.Torque,
                            FuleType = s.FuleType,
                            Mileage = s.Mileage,
                            BodyType = s.BodyType,
                            SeatingCapacity = s.SeatingCapacity
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            return vehicle;
        }
    }
}
