using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;

namespace VehicalApi.Business.Manager.Interfaces
{
    public interface IManagerService
    {
        Task<Vehicle> CreateVehicleAsync(CreateVehicleDto dto);
        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task AddVehicleSpecificationAsync(int vehicleId, CreateVehicleSpecificationDto dto);
        Task AddVehicleImageAsync(int vehicleId, CreateVehicleImageDto dto);
    }
}
