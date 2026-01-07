using VehicalApi.Business.Vehicles.Interfaces;
using VehicalApi.Domain.Vehicles.Interfaces;
using VehicalApi.Dto;

namespace VehicalApi.Business.Vehicles.Services
{
    public class VehicleQueryService : IVehicleQueryService
    {
        private readonly IVehicleDomainService _domainService;

        public VehicleQueryService(IVehicleDomainService domainService)
        {
            _domainService = domainService;
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
            return _domainService.GetDashboardVehiclesAsync(
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
            return await _domainService.GetVehicleDetailsAsync(id);
        }
    }
}
