using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleSpecification;
using VehicalApi.Entity;
using VehicalApi.Infrastructure.Repositories;
using Xunit;

namespace VehicalApi.Tests.Repositories
{
    public class ManagerRepositoryTests
    {
        [Fact]
        public async Task CreateVehicleAsync_ShouldCreateVehicle_WhenValidDtoPassed()
        {
            var context = DbContextFactory.Create("CreateVehicle_TestDb");

            var repository = new ManagerRepository(context);

            var dto = new CreateVehicleDto
            {
                ShowroomId = 1,
                VehicleName = "BMW X5",
                Model = "X5",
                YearOfProduction = 2024,
                BasePrice = 8000000,
                StockCount = 3,
                ShortDescription = "Luxury SUV"
            };

            var result = await repository.CreateVehicleAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("BMW X5", result.VehicleName);
            Assert.Equal("X5", result.Model);
            Assert.Equal(1, result.ShowroomId);
            Assert.Equal(2024, result.YearOfProduction);
            Assert.Equal(8000000, result.BasePrice);
            Assert.Equal(3, result.StockCount);
            Assert.True(result.VehicalId > 0);
        }

        [Fact]
        public async Task AddVehicleSpecificationAsync_ShouldAddSpecification_WhenValidDataPassed()
        {
            var context = DbContextFactory.Create("AddVehicleSpecification_TestDb");
            var vehicle = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "BMW X5",
                Model = "X5",
                YearOfProduction = 2024,
                BasePrice = 8000000,
                StockCount = 2,
                ShortDescription = "Luxury SUV"
            };

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var repository = new ManagerRepository(context);

            var dto = new CreateVehicleSpecificationDto
            {
                Engine = 2998,
                PowerOfVehical = "335 HP",
                Torque = "450 Nm",
                FuelType = "Petrol",
                Mileage = "12 km/l",
                BodyType = "SUV",
                SeatingCapacity = 5
            };

            await repository.AddVehicleSpecificationAsync(vehicle.VehicalId, dto);

            var specification = await context.VehicleSpecifications.FirstOrDefaultAsync();

            Assert.NotNull(specification);
            Assert.Equal(vehicle.VehicalId, specification!.VehicalId);
            Assert.Equal(2998, specification.Engine);
            Assert.Equal("335 HP", specification.PowerOfvehical);
            Assert.Equal("450 Nm", specification.Torque);
            Assert.Equal("Petrol", specification.FuleType);
            Assert.Equal("12 km/l", specification.Mileage);
            Assert.Equal("SUV", specification.BodyType);
            Assert.Equal(5, specification.SeatingCapacity);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_ShouldReturnVehicleWithSpecifications_WhenVehicleExists()
        {
            var context = DbContextFactory.Create("GetVehicleById_WithSpecs_TestDb");

            var vehicle = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "BMW X5",
                Model = "X5",
                YearOfProduction = 2024,
                BasePrice = 8000000,
                StockCount = 2,
                ShortDescription = "Luxury SUV"
            };

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var specification = new VehicleSpecification
            {
                VehicalId = vehicle.VehicalId,
                Engine = 2998,
                PowerOfvehical = "335 HP",
                Torque = "450 Nm",
                FuleType = "Petrol",
                Mileage = "12 km/l",
                BodyType = "SUV",
                SeatingCapacity = 5
            };

            context.VehicleSpecifications.Add(specification);
            await context.SaveChangesAsync();

            var repository = new ManagerRepository(context);

            var result = await repository.GetVehicleByIdAsync(vehicle.VehicalId);

            Assert.NotNull(result);
            Assert.Equal(vehicle.VehicalId, result!.VehicalId);
            Assert.Equal("BMW X5", result.VehicleName);

            Assert.NotNull(result.VehicleSpecifications);
            Assert.Single(result.VehicleSpecifications);

            var spec = Assert.Single(result.VehicleSpecifications);
            Assert.Equal(2998, spec.Engine);
            Assert.Equal("335 HP", spec.PowerOfvehical);
        }

        [Fact]
        public async Task GetVehicleByIdAsync_ShouldReturnNull_WhenVehicleDoesNotExist()
        {
            var context = DbContextFactory.Create("GetVehicleById_NotFound_TestDb");
            var repository = new ManagerRepository(context);

            var result = await repository.GetVehicleByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetVehiclesByShowroomAsync_ShouldReturnVehiclesWithPrimaryImage_WhenShowroomHasVehicles()
        {
            var context = DbContextFactory.Create("GetVehiclesByShowroom_TestDb");

            var vehicle1 = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "BMW X5",
                Model = "X5",
                YearOfProduction = 2024,
                BasePrice = 8000000,
                StockCount = 2,
                AgeInShowroom = DateTime.UtcNow.AddDays(-10),
                ShortDescription = "Luxury SUV"
            };

            var vehicle2 = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "Audi Q7",
                Model = "Q7",
                YearOfProduction = 2023,
                BasePrice = 7500000,
                StockCount = 1,
                AgeInShowroom = DateTime.UtcNow.AddDays(-20),
                ShortDescription = "Premium SUV"
            };

            context.Vehicles.AddRange(vehicle1, vehicle2);
            await context.SaveChangesAsync();

            context.VehicleImages.AddRange(
                new VehicleImage
                {
                    VehicleId = vehicle1.VehicalId,
                    ImageLocation = "image2.jpg",
                    SortOrder = 2
                },
                new VehicleImage
                {
                    VehicleId = vehicle1.VehicalId,
                    ImageLocation = "image1.jpg",
                    SortOrder = 1
                }
            );

            await context.SaveChangesAsync();

            var repository = new ManagerRepository(context);

            var result = await repository.GetVehiclesByShowroomAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var firstVehicle = result.First(v => v.VehicleName == "BMW X5");

            Assert.Equal(vehicle1.VehicalId, firstVehicle.VehicalId);
            Assert.Equal(2, firstVehicle.StockCount);
            Assert.Equal("image1.jpg", firstVehicle.PrimaryImageUrl);
        }

        [Fact]
        public async Task GetVehiclesByShowroomAsync_ShouldReturnEmptyList_WhenNoVehiclesExist()
        {
            var context = DbContextFactory.Create("GetVehiclesByShowroom_Empty_TestDb");
            var repository = new ManagerRepository(context);

            var result = await repository.GetVehiclesByShowroomAsync(99);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteVehicleAsync_ShouldDeleteVehicle_AndRelatedEntities()
        {
            var context = DbContextFactory.Create("DeleteVehicle_TestDb");

            var vehicle = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "BMW X5",
                Model = "X5",
                YearOfProduction = 2024,
                BasePrice = 8000000,
                StockCount = 2,
                ShortDescription = "Luxury SUV"
            };

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var image = new VehicleImage
            {
                VehicleId = vehicle.VehicalId,
                ImageLocation = "image1.jpg",
                SortOrder = 1
            };

            var specification = new VehicleSpecification
            {
                VehicalId = vehicle.VehicalId,
                Engine = 2998,
                PowerOfvehical = "335 HP",
                Torque = "450 Nm",
                FuleType = "Petrol",
                Mileage = "12 km/l",
                BodyType = "SUV",
                SeatingCapacity = 5
            };

            context.VehicleImages.Add(image);
            context.VehicleSpecifications.Add(specification);
            await context.SaveChangesAsync();

            var vehicleFromDb = await context.Vehicles
                .Include(v => v.VehicleImages)
                .Include(v => v.VehicleSpecifications)
                .FirstAsync();

            var repository = new ManagerRepository(context);

            await repository.DeleteVehicleAsync(vehicleFromDb);

            var vehicleCount = await context.Vehicles.CountAsync();
            var imageCount = await context.VehicleImages.CountAsync();
            var specificationCount = await context.VehicleSpecifications.CountAsync();

            Assert.Equal(0, vehicleCount);
            Assert.Equal(0, imageCount);
            Assert.Equal(0, specificationCount);
        }

        [Fact]
        public async Task UpdateVehicleAsync_ShouldUpdateVehicleFields()
        {
            var context = DbContextFactory.Create("UpdateVehicle_TestDb");

            var vehicle = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "BMW X5",
                Model = "X5",
                YearOfProduction = 2023,
                BasePrice = 7000000,
                StockCount = 2,
                ShortDescription = "Old description"
            };

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var repository = new ManagerRepository(context);

            vehicle.StockCount = 10;
            vehicle.BasePrice = 8500000;
            vehicle.ShortDescription = "Updated description";

            await repository.UpdateVehicleAsync(vehicle);

            var updatedVehicle = await context.Vehicles.FirstAsync();

            Assert.Equal(10, updatedVehicle.StockCount);
            Assert.Equal(8500000, updatedVehicle.BasePrice);
            Assert.Equal("Updated description", updatedVehicle.ShortDescription);
        }

        [Fact]
        public async Task GetVehicleImagesAsync_ShouldReturnImagesOrderedBySortOrder()
        {
            var context = DbContextFactory.Create("GetVehicleImages_TestDb");

            var vehicle = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "Audi Q7",
                Model = "Q7",
                YearOfProduction = 2024,
                BasePrice = 7500000,
                StockCount = 1,
                ShortDescription = "Premium SUV"
            };

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            context.VehicleImages.AddRange(
                new VehicleImage
                {
                    VehicleId = vehicle.VehicalId,
                    ImageLocation = "image2.jpg",
                    SortOrder = 2
                },
                new VehicleImage
                {
                    VehicleId = vehicle.VehicalId,
                    ImageLocation = "image1.jpg",
                    SortOrder = 1
                }
            );

            await context.SaveChangesAsync();
            var repository = new ManagerRepository(context);
            var images = await repository.GetVehicleImagesAsync(vehicle.VehicalId);
            Assert.Equal(2, images.Count);
            Assert.Equal("image1.jpg", images[0].ImageLocation);
            Assert.Equal("image2.jpg", images[1].ImageLocation);
        }

        [Fact]
        public async Task GetVehicleImageByIdAsync_ShouldReturnImage_WhenImageExists()
        {
            var context = DbContextFactory.Create("GetVehicleImageById_TestDb");

            var vehicle = new Vehicle
            {
                ShowroomId = 1,
                VehicleName = "Tesla Model Y",
                Model = "Model Y",
                YearOfProduction = 2024,
                BasePrice = 6000000,
                StockCount = 5,
                ShortDescription = "Electric SUV"
            };

            context.Vehicles.Add(vehicle);
            await context.SaveChangesAsync();

            var image = new VehicleImage
            {
                VehicleId = vehicle.VehicalId,
                ImageLocation = "tesla.jpg",
                SortOrder = 1
            };

            context.VehicleImages.Add(image);
            await context.SaveChangesAsync();

            var repository = new ManagerRepository(context);
            var result = await repository.GetVehicleImageByIdAsync(image.ImageId);
            Assert.NotNull(result);
            Assert.Equal(image.ImageId, result!.ImageId);
            Assert.Equal("tesla.jpg", result.ImageLocation);
        }

        [Fact]
        public async Task GetVehicleImageByIdAsync_ShouldReturnNull_WhenImageDoesNotExist()
        {
            var context = DbContextFactory.Create("GetVehicleImageById_NotFound_TestDb");
            var repository = new ManagerRepository(context);
            var result = await repository.GetVehicleImageByIdAsync(999);
            Assert.Null(result);
        }

    }
}
