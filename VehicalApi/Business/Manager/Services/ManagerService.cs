
using VehicalApi.Business.Manager.Interfaces;
using VehicalApi.Controllers;
using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Dtos.Manager;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;

namespace VehicalApi.Business.Manager.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerDomainService _domain;

        public ManagerService(IManagerDomainService domain)
        {
            _domain = domain;
        }

        // public Task<Vehicle> CreateVehicleAsync(int userId,CreateVehicleDto dto)
        //     => _domain.CreateVehicleAsync(userId,dto);

        public Task<Vehicle> GetVehicleByIdAsync(int id)
            => _domain.GetVehicleByIdAsync(id);

        // public Task AddVehicleSpecificationAsync(
        //     int vehicleId,
        //     CreateVehicleSpecificationDto dto)
        //     => _domain.AddVehicleSpecificationAsync(vehicleId, dto);

        public Task<int> CreateVehicleWithSpecificationAsync(
            int managerUserId,
            CreateVehicleWithSpecDto dto)
        {
                        Console.WriteLine("Here in the bussiness");

            return _domain.CreateVehicleWithSpecificationAsync(
                managerUserId, dto);
        }

        public Task<List<ManagerVehicleDto>> GetVehiclesByManagerAsync(int managerUserId)
            => _domain.GetVehiclesByManagerAsync(managerUserId);

        public async Task<int> DeleteVehicleAsync(int managerUserId, int vehicleId)
        {
            await _domain.DeleteVehicleAsync(managerUserId, vehicleId);
            return vehicleId;
        }

        public async Task<string> UpdateVehicleAsync(int managerUserId, int vehicleId, UpdateVehicleWithSpecDto dto)
        {
            // Console.WriteLine("In the cotroller B service");
            return await _domain.UpdateVehicleAsync(managerUserId, vehicleId, dto);
        }

        public async Task AddVehicleImageAsync(int managerUserId, int vehicleId, List<SaveVehicleImageDto> images)
        {
            await _domain.AddVehicleImageAsync(
                managerUserId,
                vehicleId,
                images
            );
        }

        public async Task<List<VehicleImageResponseDto>> GetVehicleImagesAsync(int vehicleId)
        {
            return await _domain.GetVehicleImagesAsync(vehicleId);
        }
    }
}
