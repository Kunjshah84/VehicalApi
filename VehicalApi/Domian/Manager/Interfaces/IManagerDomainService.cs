using VehicalApi.Dtos.Manager;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Entity;

namespace VehicalApi.Domain.Manager.Interfaces
{
    public interface IManagerDomainService
    {
        Task<Vehicle> GetVehicleByIdAsync(int id);

        Task<int> CreateVehicleWithSpecificationAsync(
            int managerUserId,
            CreateVehicleWithSpecDto dto);

        Task<List<ManagerVehicleDto>> GetVehiclesByManagerAsync(
            int managerUserId);

        Task<int> DeleteVehicleAsync(int managerUserId, int vehicleId);

        Task<string > UpdateVehicleAsync(int managerUserId,int vehicleId,UpdateVehicleWithSpecDto dto);

        Task AddVehicleImageAsync(int managerUserId,int vehicleId,List<SaveVehicleImageDto> images);        

        Task<List<VehicleImageResponseDto>> GetVehicleImagesAsync(int vehicleId);
    }
}
