using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;
using VehicalApi.Exceptions;

namespace VehicalApi.Domain.Manager.Services
{
    public class ManagerDomainService : IManagerDomainService
    {
        private readonly IManagerRepository _repository;

        public ManagerDomainService(IManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleDto dto)
        {
            if (!await _repository.ShowroomExistsAsync(dto.ShowroomId))
                throw new NotFoundException("Showroom not found");

            return await _repository.CreateVehicleAsync(dto);
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _repository.GetVehicleByIdAsync(id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            return vehicle;
        }

        public async Task AddVehicleSpecificationAsync(
            int vehicleId,
            CreateVehicleSpecificationDto dto)
        {
            if (!await _repository.VehicleExistsAsync(vehicleId))
                throw new NotFoundException("Vehicle not found");

            if (await _repository.VehicleSpecificationExistsAsync(vehicleId))
                throw new BadRequestException("Specifications already exist for this vehicle");

            await _repository.AddVehicleSpecificationAsync(vehicleId, dto);
        }

        public async Task AddVehicleImageAsync(
            int vehicleId,
            CreateVehicleImageDto dto)
        {
            if (!await _repository.VehicleExistsAsync(vehicleId))
                throw new NotFoundException("Vehicle not found");

            await _repository.AddVehicleImageAsync(vehicleId, dto);
        }
    }
}
