using Moq;
using Xunit;
using VehicalApi.Domain.Vehicles.Services;
using VehicalApi.Domain.Vehicles.Interfaces;
using VehicalApi.Dto;

namespace VehicalApi.Tests.Domain.Vehicles
{
    public class VehicleDomainServiceTests
    {
        private readonly Mock<IVehicleRepository> _repositoryMock;
        private readonly VehicleDomainService _sut;

        public VehicleDomainServiceTests()
        {
            _repositoryMock = new Mock<IVehicleRepository>();
            _sut = new VehicleDomainService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetDashboardVehiclesAsync_ShouldReturnVehiclesFromRepository()
        {
            var expected = new List<DashboardVehicleDto>
            {
                new DashboardVehicleDto(),
                new DashboardVehicleDto()
            };

            _repositoryMock
                .Setup(r => r.GetDashboardVehiclesAsync(
                    It.IsAny<string?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<double?>(),
                    It.IsAny<double?>(),
                    It.IsAny<string?>()
                ))
                .ReturnsAsync(expected);

            var result = await _sut.GetDashboardVehiclesAsync(
                "car",
                100000,
                500000,
                "Petrol",
                "SUV",
                1.0,
                3.0,
                "price"
            );

            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);

            _repositoryMock.Verify(r => r.GetDashboardVehiclesAsync(
                "car",
                100000,
                500000,
                "Petrol",
                "SUV",
                1.0,
                3.0,
                "price"
            ), Times.Once);
        }

        [Fact]
        public async Task GetVehicleDetailsAsync_WhenCalled_ShouldReturnVehicleDetailsFromRepository()
        {
            var expected = new VehicleDetailsDto();

            _repositoryMock
                .Setup(r => r.GetVehicleDetailsAsync(1))
                .ReturnsAsync(expected);

            var result = await _sut.GetVehicleDetailsAsync(1);

            Assert.NotNull(result);
            Assert.Equal(expected, result);

            _repositoryMock.Verify(r => r.GetVehicleDetailsAsync(1), Times.Once);
        }
    }
}
