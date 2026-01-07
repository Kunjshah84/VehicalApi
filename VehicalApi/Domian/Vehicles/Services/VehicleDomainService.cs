using VehicalApi.Domain.Vehicles.Interfaces;
using VehicalApi.Dto;

namespace VehicalApi.Domain.Vehicles.Services
{
    public class VehicleDomainService : IVehicleDomainService
    {
        private readonly IVehicleRepository _repository;

        public VehicleDomainService(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public Task<List<DashboardVehicleDto>> GetDashboardVehiclesAsync(
            string? search,
            int? minPrice,
            int? maxPrice,
            string? fuelTypes,
            string? bodyTypes,
            double? minEngine,
            double? maxEngine,
            string? sortBy
        )
        {
            return _repository.GetDashboardVehiclesAsync(
                search,
                minPrice,
                maxPrice,
                fuelTypes,
                bodyTypes,
                minEngine,
                maxEngine,
                sortBy
            );
        }

        public async Task<VehicleDetailsDto> GetVehicleDetailsAsync(int id)
        {
            return await _repository.GetVehicleDetailsAsync(id);
        }
    }
}
