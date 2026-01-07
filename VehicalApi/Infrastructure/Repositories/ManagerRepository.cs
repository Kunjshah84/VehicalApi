

using Microsoft.EntityFrameworkCore;
using VehicalApi.Data;
using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Dtos.Manager;
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

        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleDto dto)
        {
            Console.WriteLine("In the repo");
            var vehicle = new Vehicle
            {
                ShowroomId = dto.ShowroomId,
                VehicleName = dto.VehicleName,
                Model = dto.Model,
                YearOfProduction = dto.YearOfProduction,
                BasePrice = dto.BasePrice,
                StockCount = dto.StockCount,
                ShortDescription = dto.ShortDescription,
                AgeInShowroom = dto.AgeInShowroom  
            };
            Console.WriteLine("Here fails");

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }


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

        public async Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
        {
            return await _context.Vehicles
                .Include(v => v.VehicleSpecifications)
                .FirstOrDefaultAsync(v => v.VehicalId == vehicleId);
        }


        public Task<bool> VehicleExistsAsync(int vehicleId)
            => _context.Vehicles.AnyAsync(v => v.VehicalId == vehicleId);


        public async Task<int?> GetShowroomIdByManagerAsync(int managerUserId)
        {
            return await _context.Showrooms
                .Where(s => s.ManagerId == managerUserId)
                .Select(s => (int?)s.ShowroomId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ManagerVehicleDto>> GetVehiclesByShowroomAsync(int showroomId)
        {
            return await _context.Vehicles
                .Where(v => v.ShowroomId == showroomId)
                .Select(v => new ManagerVehicleDto
                {
                    VehicalId = v.VehicalId,
                    VehicleName = v.VehicleName,
                    AgeInShowroom = v.AgeInShowroom,
                    StockCount = v.StockCount,

                    PrimaryImageUrl = v.VehicleImages
                        .OrderBy(img => img.SortOrder)
                        .Select(img => img.ImageLocation)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            _context.VehicleImages.RemoveRange(vehicle.VehicleImages);
            _context.VehicleSpecifications.RemoveRange(vehicle.VehicleSpecifications);
            _context.Vehicles.Remove(vehicle);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            Console.WriteLine("In the repo");
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleImage>> GetVehicleImagesAsync(int vehicleId)
        {
            return await _context.VehicleImages
                .Where(img => img.VehicleId == vehicleId)
                .OrderBy(img => img.SortOrder)
                .ToListAsync();
        }

        public async Task<VehicleImage?> GetVehicleImageByIdAsync(int imageId)
        {
            return await _context.VehicleImages
                .FirstOrDefaultAsync(img => img.ImageId == imageId);
        }

        public async Task DeleteVehicleImageAsync(int imageId)
        {
            var image = await _context.VehicleImages
                .FirstOrDefaultAsync(img => img.ImageId == imageId);

            if (image == null)
                return;

            _context.VehicleImages.Remove(image);
            await _context.SaveChangesAsync();
        }


        public async Task AddVehicleImageAsync(
            int vehicleId,
            string imageLocation,
            int sortOrder
        )
        {
            var image = new VehicleImage
            {
                VehicleId = vehicleId,
                ImageLocation = imageLocation,
                SortOrder = sortOrder
            };

            _context.VehicleImages.Add(image);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateVehicleImageSortOrderAsync(
            int imageId,
            int sortOrder
        )
        {
            var image = await _context.VehicleImages
                .FirstOrDefaultAsync(img => img.ImageId == imageId);

            if (image == null) return;

            image.SortOrder = sortOrder;
            await _context.SaveChangesAsync();
        }
    }
}
