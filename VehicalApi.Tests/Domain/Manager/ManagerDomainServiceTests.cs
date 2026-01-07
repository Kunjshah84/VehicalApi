using Xunit;
using Moq;
using VehicalApi.Domain.Manager.Services;
using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Entity;
using VehicalApi.Exceptions;
using VehicalApi.Dtos.Manager;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Entity;
using VehicalApi.Dtos.VehicleSpecification;

namespace VehicalApi.Tests.Domain.Auth
{
    public class ManagerDomainServiceTests
    {
        private readonly Mock<IManagerRepository> _repositoryMock;
        private readonly ManagerDomainService _service;

        public ManagerDomainServiceTests()
        {
            _repositoryMock = new Mock<IManagerRepository>();
            _service = new ManagerDomainService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_WhenVehicleExists_ReturnsVehicle()
        {
            var vehicleId = 1;
            var vehicle = new Vehicle
            {
                VehicalId = vehicleId
            };

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);

            var result = await _service.GetVehicleByIdAsync(vehicleId);

            Assert.NotNull(result);
            Assert.Equal(vehicleId, result.VehicalId);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_WhenVehicleNotFound_ThrowsNotFoundException()
        {
            var vehicleId = 99;

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync((Vehicle)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetVehicleByIdAsync(vehicleId)
            );
        }
        [Fact]
        public async Task CreateVehicleWithSpecificationAsync_WhenValidManager_CreatesVehicleAndReturnsId()
        {
            var managerUserId = 10;
            var showroomId = 5;
            var createdVehicleId = 100;

            var dto = new CreateVehicleWithSpecDto
            {
                VehicleName = "Test Vehicle",
                Model = "X1",
                YearOfProduction = 2024,
                BasePrice = 500000,
                StockCount = 3,
                ShortDescription = "Test desc",
                Specification = new CreateVehicleSpecificationDto
                {
                    Engine = 12,
                    PowerOfVehical = "200HP",
                    Torque = "300Nm",
                    FuelType = "Petrol",
                    Mileage = "15",
                    BodyType = "SUV",
                    SeatingCapacity = 5
                }
            };

            _repositoryMock
            .Setup(r => r.GetShowroomIdByManagerAsync(managerUserId))
            .ReturnsAsync(showroomId);

            _repositoryMock
            .Setup(r => r.CreateVehicleAsync(It.IsAny<CreateVehicleDto>()))
            .ReturnsAsync(new Vehicle
            {
                VehicalId = createdVehicleId
            });

            _repositoryMock
            .Setup(r => r.AddVehicleSpecificationAsync(
                createdVehicleId,
                It.IsAny<CreateVehicleSpecificationDto>()
            ))
            .Returns(Task.CompletedTask);

            var result = await _service.CreateVehicleWithSpecificationAsync(managerUserId, dto);
            Assert.Equal(createdVehicleId, result);


            _repositoryMock.Verify(
                r => r.GetShowroomIdByManagerAsync(managerUserId),
                Times.Once
            );

            _repositoryMock.Verify(
                r => r.CreateVehicleAsync(It.IsAny<CreateVehicleDto>()),
                Times.Once
            );

            _repositoryMock.Verify(
                r => r.AddVehicleSpecificationAsync(
                    createdVehicleId,
                    It.IsAny<CreateVehicleSpecificationDto>()
                ),
                Times.Once
            );
        }


        [Fact]
        public async Task CreateVehicleWithSpecificationAsync_WhenShowroomNotFound_ThrowsNotFoundException()
        {
            var managerUserId = 10;
            var dto = new CreateVehicleWithSpecDto();

            _repositoryMock
                .Setup(r => r.GetShowroomIdByManagerAsync(managerUserId))
                .ReturnsAsync((int?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.CreateVehicleWithSpecificationAsync(managerUserId, dto)
            );
        }


        [Fact]
        public async Task DeleteVehicleAsync_WhenAuthorized_DeletesVehicle()
        {
            var managerUserId = 10;
            var showroomId = 5;
            var vehicleId = 20;

            var vehicle = new Vehicle
            {
                VehicalId = vehicleId,
                ShowroomId = showroomId
            };

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);

            _repositoryMock
                .Setup(r => r.GetShowroomIdByManagerAsync(managerUserId))
                .ReturnsAsync(showroomId);

            _repositoryMock
                .Setup(r => r.DeleteVehicleAsync(vehicle))
                .Returns(Task.CompletedTask);

            var result = await _service.DeleteVehicleAsync(managerUserId, vehicleId);

            Assert.Equal(vehicleId, result);

            _repositoryMock.Verify(
                r => r.DeleteVehicleAsync(vehicle),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteVehicleAsync_WhenUnauthorized_ThrowsUnauthorizedException()
        {
            var managerUserId = 10;
            var vehicleId = 20;

            var vehicle = new Vehicle
            {
                VehicalId = vehicleId,
                ShowroomId = 99
            };

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);

            _repositoryMock
                .Setup(r => r.GetShowroomIdByManagerAsync(managerUserId))
                .ReturnsAsync(5);

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                _service.DeleteVehicleAsync(managerUserId, vehicleId)
            );
        }

        [Fact]
        public async Task DeleteVehicleAsync_ShouldThrowNotFoundException_WhenVehicleDoesNotExist()
        {
            // Arrange
            var managerUserId = 10;
            var vehicleId = 99;

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync((Vehicle)null);

            // Act
            var action = async () =>
                await _service.DeleteVehicleAsync(managerUserId, vehicleId);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(action);

            _repositoryMock.Verify(
                r => r.DeleteVehicleAsync(It.IsAny<Vehicle>()),
                Times.Never
            );
        }

        [Fact]
        public async Task UpdateVehicleAsync_WhenValidManagerAndSpecExists_UpdatesVehicle()
        {
            // Arrange
            var managerUserId = 10;
            var showroomId = 5;
            var vehicleId = 20;

            var vehicle = new Vehicle
            {
                VehicalId = vehicleId,
                ShowroomId = showroomId,
                VehicleSpecifications = new List<VehicleSpecification>
                {
                    new VehicleSpecification()
                }
            };

            var dto = new UpdateVehicleWithSpecDto
            {
                VehicleName = "Updated Name",
                Model = "Updated Model",
                YearOfProduction = 2025,
                BasePrice = 700000,
                StockCount = 4,
                ShortDescription = "Updated Desc",
                Engine = 1200,
                PowerOfVehical = "250HP",
                Torque = "350Nm",
                FuelType = "Diesel",
                Mileage = "18",
                BodyType = "Sedan",
                SeatingCapacity = 5
            };

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);

            _repositoryMock
                .Setup(r => r.GetShowroomIdByManagerAsync(managerUserId))
                .ReturnsAsync(showroomId);

            _repositoryMock
                .Setup(r => r.UpdateVehicleAsync(vehicle))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateVehicleAsync(
                managerUserId,
                vehicleId,
                dto
            );

            // Assert
            Assert.Equal("Vehicle Edited with vehicle Id", result);

            Assert.Equal(dto.VehicleName, vehicle.VehicleName);
            Assert.Equal(dto.Model, vehicle.Model);
            Assert.Equal(dto.BasePrice, vehicle.BasePrice);
            Assert.Equal(dto.StockCount, vehicle.StockCount);

            var spec = vehicle.VehicleSpecifications.First();
            Assert.Equal(dto.Engine, spec.Engine);
            Assert.Equal(dto.PowerOfVehical, spec.PowerOfvehical);
            Assert.Equal(dto.FuelType, spec.FuleType);

            _repositoryMock.Verify(
                r => r.UpdateVehicleAsync(vehicle),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateVehicleAsync_WhenManagerNotOwner_ThrowsUnauthorizedAccessException()
        {
            var managerUserId = 10;
            var vehicleId = 20;

            var vehicle = new Vehicle
            {
                VehicalId = vehicleId,
                ShowroomId = 99
            };

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);

            _repositoryMock
                .Setup(r => r.GetShowroomIdByManagerAsync(managerUserId))
                .ReturnsAsync(5);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.UpdateVehicleAsync(
                    managerUserId,
                    vehicleId,
                    new UpdateVehicleWithSpecDto()
                )
            );
        }


        [Fact]
        public async Task UpdateVehicleAsync_WhenSpecificationMissing_ThrowsNotFoundException()
        {
            var managerUserId = 10;
            var showroomId = 5;
            var vehicleId = 20;

            var vehicle = new Vehicle
            {
                VehicalId = vehicleId,
                ShowroomId = showroomId,
                VehicleSpecifications = new List<VehicleSpecification>()
            };

            _repositoryMock
                .Setup(r => r.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);

            _repositoryMock
                .Setup(r => r.GetShowroomIdByManagerAsync(managerUserId))
                .ReturnsAsync(showroomId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.UpdateVehicleAsync(
                    managerUserId,
                    vehicleId,
                    new UpdateVehicleWithSpecDto()
                )
            );
        }

    }
}