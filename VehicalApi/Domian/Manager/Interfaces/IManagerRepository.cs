using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;

namespace VehicalApi.Domain.Manager.Interfaces
{
    public interface IManagerRepository
    {
        Task<bool> ShowroomExistsAsync(int showroomId);
        Task<bool> VehicleExistsAsync(int vehicleId);
        Task<bool> VehicleSpecificationExistsAsync(int vehicleId);

        Task<Vehicle> CreateVehicleAsync(CreateVehicleDto dto);
        Task<Vehicle?> GetVehicleByIdAsync(int id);
        Task AddVehicleSpecificationAsync(int vehicleId, CreateVehicleSpecificationDto dto);
        Task AddVehicleImageAsync(int vehicleId, CreateVehicleImageDto dto);
    }
}
