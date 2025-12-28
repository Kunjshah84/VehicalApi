using VehicalApi.Dto;

namespace VehicalApi.Domain.Vehicles.Interfaces
{
    public interface IVehicleRepository
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

        Task<object> GetVehicleDetailsAsync(int id);
    }
}
