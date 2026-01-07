using VehicalApi.Dto;

namespace VehicalApi.Business.Vehicles.Interfaces
{
    public interface IVehicleQueryService
    {
        Task<List<DashboardVehicleDto>> GetDashboardVehiclesAsync(
            string? search,
            int? minPrice,
            int? maxPrice,
            string? fuelTypes,
            string? bodyTypes,
            double? minEngine,
            double? maxEngine,
            string? sortBy
        );

        Task<VehicleDetailsDto> GetVehicleDetailsAsync(int id);
    }
}
