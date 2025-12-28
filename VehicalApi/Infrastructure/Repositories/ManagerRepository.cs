using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;
using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;

namespace VehicalApi.Infrastructure.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly MyDbContext _context;

        public ManagerRepository(MyDbContext context)
        {
            _context = context;
        }

        public Task<bool> ShowroomExistsAsync(int showroomId)
            => _context.Showrooms.AnyAsync(s => s.ShowroomId == showroomId);

        public Task<bool> VehicleExistsAsync(int vehicleId)
            => _context.Vehicles.AnyAsync(v => v.VehicalId == vehicleId);

        public Task<bool> VehicleSpecificationExistsAsync(int vehicleId)
            => _context.VehicleSpecifications.AnyAsync(s => s.VehicalId == vehicleId);

        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleDto dto)
        {
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
            return vehicle;
        }

        public Task<Vehicle?> GetVehicleByIdAsync(int id)
            => _context.Vehicles.FirstOrDefaultAsync(v => v.VehicalId == id);

        public async Task AddVehicleSpecificationAsync(
            int vehicleId,
            CreateVehicleSpecificationDto dto)
        {
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
        }

        public async Task AddVehicleImageAsync(
            int vehicleId,
            CreateVehicleImageDto dto)
        {
            var image = new VehicleImage
            {
                VehicleId = vehicleId,
                ImageLocation = dto.ImageLocation,
                SortOrder = dto.SortOrder
            };

            _context.VehicleImages.Add(image);
            await _context.SaveChangesAsync();
        }
    }
}
