using VehicalApi.Dtos.Manager;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;

namespace VehicalApi.Domain.Manager.Interfaces
{
    public interface IManagerRepository
    {
        Task<Vehicle> CreateVehicleAsync(CreateVehicleDto dto);
        Task AddVehicleSpecificationAsync(int vehicleId, CreateVehicleSpecificationDto dto);

        Task<Vehicle?> GetVehicleByIdAsync(int id);

        Task<int?> GetShowroomIdByManagerAsync(int managerUserId);
        Task<List<ManagerVehicleDto>> GetVehiclesByShowroomAsync(int showroomId);

        Task<bool> VehicleExistsAsync(int vehicleId);

        Task DeleteVehicleAsync(Vehicle vehicle);

        Task UpdateVehicleAsync(Vehicle vehicle);


        Task<List<VehicleImage>> GetVehicleImagesAsync(int vehicleId);

        Task<VehicleImage?> GetVehicleImageByIdAsync(int imageId);

        Task AddVehicleImageAsync(
            int vehicleId,
            string imageLocation,
            int sortOrder
        );


        Task DeleteVehicleImageAsync(int imageId);

        Task UpdateVehicleImageSortOrderAsync(
            int imageId,
            int sortOrder
        );

    }
}
