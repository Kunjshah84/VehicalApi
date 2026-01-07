using Moq;
using Xunit;
using VehicalApi.Domain.Manager.Services;
using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;
using VehicalApi.Exceptions;

namespace VehicalApi.Tests.Domain.Manager
{
    public class ManagerDomainServiceTests
    {
        private readonly Mock<IManagerRepository> _repositoryMock;
        private readonly ManagerDomainService _sut;

        public ManagerDomainServiceTests()
        {
            _repositoryMock = new Mock<IManagerRepository>();
            _sut = new ManagerDomainService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateVehicleAsync_WhenShowroomNotFound_ShouldThrowNotFoundException()
        {
            var dto = new CreateVehicleDto { ShowroomId = 1 };

            _repositoryMock
                .Setup(r => r.ShowroomExistsAsync(dto.ShowroomId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _sut.CreateVehicleAsync(dto)
            );
        }

        [Fact]
        public async Task CreateVehicleAsync_WhenValid_ShouldCreateVehicle()
        {
            var dto = new CreateVehicleDto { ShowroomId = 1 };
            var vehicle = new Vehicle();

            _repositoryMock
                .Setup(r => r.ShowroomExistsAsync(dto.ShowroomId))
                .ReturnsAsync(true);

            _repositoryMock
                .Setup(r => r.CreateVehicleAsync(dto))
                .ReturnsAsync(vehicle);

            var result = await _sut.CreateVehicleAsync(dto);

            Assert.Equal(vehicle, result);

            _repositoryMock.Verify(r => r.CreateVehicleAsync(dto), Times.Once);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_WhenVehicleNotFound_ShouldThrowNotFoundException()
        {
            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(1))
                .ReturnsAsync((Vehicle?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _sut.GetVehicleByIdAsync(1)
            );
        }

        [Fact]
        public async Task GetVehicleByIdAsync_WhenVehicleExists_ShouldReturnVehicle()
        {
            var vehicle = new Vehicle();

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(1))
                .ReturnsAsync(vehicle);

            var result = await _sut.GetVehicleByIdAsync(1);

            Assert.Equal(vehicle, result);
        }

        [Fact]
        public async Task AddVehicleSpecificationAsync_WhenVehicleNotFound_ShouldThrowNotFoundException()
        {
            var dto = new CreateVehicleSpecificationDto();

            _repositoryMock
                .Setup(r => r.VehicleExistsAsync(1))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _sut.AddVehicleSpecificationAsync(1, dto)
            );
        }

        [Fact]
        public async Task AddVehicleSpecificationAsync_WhenSpecificationAlreadyExists_ShouldThrowBadRequestException()
        {
            var dto = new CreateVehicleSpecificationDto();

            _repositoryMock
                .Setup(r => r.VehicleExistsAsync(1))
                .ReturnsAsync(true);

            _repositoryMock
                .Setup(r => r.VehicleSpecificationExistsAsync(1))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BadRequestException>(
                () => _sut.AddVehicleSpecificationAsync(1, dto)
            );
        }

        [Fact]
        public async Task AddVehicleSpecificationAsync_WhenValid_ShouldAddSpecification()
        {
            var dto = new CreateVehicleSpecificationDto();

            _repositoryMock
                .Setup(r => r.VehicleExistsAsync(1))
                .ReturnsAsync(true);

            _repositoryMock
                .Setup(r => r.VehicleSpecificationExistsAsync(1))
                .ReturnsAsync(false);

            await _sut.AddVehicleSpecificationAsync(1, dto);

            _repositoryMock.Verify(
                r => r.AddVehicleSpecificationAsync(1, dto),
                Times.Once
            );
        }

        [Fact]
        public async Task AddVehicleImageAsync_WhenVehicleNotFound_ShouldThrowNotFoundException()
        {
            var dto = new CreateVehicleImageDto();

            _repositoryMock
                .Setup(r => r.VehicleExistsAsync(1))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _sut.AddVehicleImageAsync(1, dto)
            );
        }

        [Fact]
        public async Task AddVehicleImageAsync_WhenValid_ShouldAddImage()
        {
            var dto = new CreateVehicleImageDto();

            _repositoryMock
                .Setup(r => r.VehicleExistsAsync(1))
                .ReturnsAsync(true);

            await _sut.AddVehicleImageAsync(1, dto);

            _repositoryMock.Verify(
                r => r.AddVehicleImageAsync(1, dto),
                Times.Once
            );
        }
    }
}
