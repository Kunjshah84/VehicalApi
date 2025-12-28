using VehicalApi.Business.Manager.Interfaces;
using VehicalApi.Domain.Manager.Interfaces;
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

        public Task<Vehicle> CreateVehicleAsync(CreateVehicleDto dto)
            => _domain.CreateVehicleAsync(dto);

        public Task<Vehicle> GetVehicleByIdAsync(int id)
            => _domain.GetVehicleByIdAsync(id);

        public Task AddVehicleSpecificationAsync(
            int vehicleId,
            CreateVehicleSpecificationDto dto)
            => _domain.AddVehicleSpecificationAsync(vehicleId, dto);

        public Task AddVehicleImageAsync(
            int vehicleId,
            CreateVehicleImageDto dto)
            => _domain.AddVehicleImageAsync(vehicleId, dto);
    }
}
