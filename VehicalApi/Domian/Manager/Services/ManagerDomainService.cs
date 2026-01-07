using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Dtos.Manager;
using VehicalApi.Dtos.Vehicle;
using VehicalApi.Dtos.VehicleImage;
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

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _repository.GetVehicleByIdAsync(id);
            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            return vehicle;
        }

        public async Task<int> CreateVehicleWithSpecificationAsync(
            int managerUserId,
            CreateVehicleWithSpecDto dto)
        {
                                    Console.WriteLine("Here in the bussiness");
            Console.WriteLine(dto);

            var showroomId =
                await _repository.GetShowroomIdByManagerAsync(managerUserId);
            if (showroomId == null)
                throw new NotFoundException("Showroom not found for manager");
            var vehicleDto = new CreateVehicleDto
            {
                ShowroomId = showroomId.Value,
                VehicleName = dto.VehicleName,
                Model = dto.Model,
                YearOfProduction = dto.YearOfProduction,
                AgeInShowroom = dto.AgeInShowroom,
                BasePrice = dto.BasePrice,
                StockCount = dto.StockCount,
                ShortDescription = dto.ShortDescription
            };

            Console.WriteLine(vehicleDto);
            var vehicle = await _repository.CreateVehicleAsync(vehicleDto);
            await _repository.AddVehicleSpecificationAsync(
                vehicle.VehicalId,
                dto.Specification
            );
            return vehicle.VehicalId;
        }

        public async Task<List<ManagerVehicleDto>> GetVehiclesByManagerAsync(
            int managerUserId)
        {
            var showroomId =
                await _repository.GetShowroomIdByManagerAsync(managerUserId);

            if (showroomId == null)
                throw new NotFoundException("Showroom not found for manager");

            return await _repository.GetVehiclesByShowroomAsync(showroomId.Value);
        }

        public async Task<int> DeleteVehicleAsync(int managerUserId, int vehicleId)
        {


            var vehicle = await _repository.GetVehicleByIdAsync(vehicleId);

            if (vehicle == null)
                throw new NotFoundException("Requested vehicle not found");

            var showroomId = await _repository.GetShowroomIdByManagerAsync(managerUserId);

            if (showroomId == null || vehicle.ShowroomId != showroomId)
                throw new UnauthorizedException("you are not autorixed for deleting that vehicle");

            await _repository.DeleteVehicleAsync(vehicle);

            return vehicleId;
        }

        public async Task<string> UpdateVehicleAsync(int managerUserId, int vehicleId, UpdateVehicleWithSpecDto dto)
        {
            var vehicle = await _repository.GetVehicleByIdAsync(vehicleId);

            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            var showroomId =
                await _repository.GetShowroomIdByManagerAsync(managerUserId);

            if (showroomId == null || vehicle.ShowroomId != showroomId)
                throw new UnauthorizedAccessException("You are not the manager of the vehicle");

            vehicle.VehicleName = dto.VehicleName;
            vehicle.Model = dto.Model;
            vehicle.YearOfProduction = dto.YearOfProduction;
            vehicle.BasePrice = dto.BasePrice;
            vehicle.StockCount = dto.StockCount;
            vehicle.ShortDescription = dto.ShortDescription;

            var spec = vehicle.VehicleSpecifications.FirstOrDefault();
            if (spec == null)
                throw new NotFoundException("First add the spec of the vehicle");

            spec.Engine = dto.Engine;
            spec.PowerOfvehical = dto.PowerOfVehical;
            spec.Torque = dto.Torque;
            spec.FuleType = dto.FuelType;
            spec.Mileage = dto.Mileage;
            spec.BodyType = dto.BodyType;
            spec.SeatingCapacity = dto.SeatingCapacity;

            await _repository.UpdateVehicleAsync(vehicle);
            return "Vehicle Edited with vehicle Id";
        }


        public async Task AddVehicleImageAsync(
            int managerUserId,
            int vehicleId,
            List<SaveVehicleImageDto> images
        )
        {
            var vehicle = await _repository.GetVehicleByIdAsync(vehicleId);

            if (vehicle == null)
                throw new NotFoundException("Vehicle not found");

            var showroomId =
                await _repository.GetShowroomIdByManagerAsync(managerUserId);

            if (showroomId == null)
                throw new UnauthorizedAccessException("Manager showroom not found");

            if (vehicle.ShowroomId != showroomId)
                throw new UnauthorizedAccessException(
                    "You are not allowed to modify images of this vehicle"
                );

            var existingImages =
                await _repository.GetVehicleImagesAsync(vehicleId);

            var existingImageMap =
                existingImages.ToDictionary(img => img.ImageId);

            var incomingImageIds = images
                .Where(i => i.ImageId != 0)
                .Select(i => i.ImageId)
                .ToHashSet();

            var imagesToDelete = existingImages
                .Where(img => !incomingImageIds.Contains(img.ImageId))
                .ToList();

            foreach (var img in imagesToDelete)
            {
                await _repository.DeleteVehicleImageAsync(img.ImageId);
            }

            foreach (var img in images)
            {
                if (img.ImageId != 0 && existingImageMap.ContainsKey(img.ImageId))
                {
                    await _repository.UpdateVehicleImageSortOrderAsync(
                        img.ImageId,
                        img.SortOrder
                    );
                }
                else if (img.ImageId == 0)
                {
                    await _repository.AddVehicleImageAsync(
                        vehicleId,
                        img.ImageLocation,
                        img.SortOrder
                    );
                }
            }
        }


        public async Task<List<VehicleImageResponseDto>> GetVehicleImagesAsync(int vehicleId)
        {
            var vehicle = await _repository
                .GetVehicleByIdAsync(vehicleId);

            if (vehicle == null)
                throw new Exception("Vehicle not found");

            var images =
                await _repository
                    .GetVehicleImagesAsync(vehicleId);

            var response = images.Select(img =>
                new VehicleImageResponseDto
                {
                    ImageId = img.ImageId,
                    VehicleId = img.VehicleId,
                    ImageLocation = img.ImageLocation,
                    SortOrder = img.SortOrder
                }
            ).ToList();

            return response;
        }
    }
}
