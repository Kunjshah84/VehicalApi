using VehicalApi.Controllers;
using VehicalApi.Dtos.Manager;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;

namespace VehicalApi.Business.Manager.Interfaces
{
    public interface IManagerService
    {
        Task<int> CreateVehicleWithSpecificationAsync(
            int managerUserId,
            CreateVehicleWithSpecDto dto);

        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task<List<ManagerVehicleDto>> GetVehiclesByManagerAsync(int managerUserId);

        Task<int> DeleteVehicleAsync(int managerUserId, int vehicleId);

        Task<string> UpdateVehicleAsync(int managerUserId, int vehicleId, UpdateVehicleWithSpecDto dto);

        Task AddVehicleImageAsync(int managerUserId, int vehicleId, List<SaveVehicleImageDto> images);

        Task<List<VehicleImageResponseDto>> GetVehicleImagesAsync(int vehicleId);
    }
}
