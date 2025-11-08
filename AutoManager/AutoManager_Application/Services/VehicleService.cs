using AutoManager.AutoManager_Application.DTOs;
using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;

namespace AutoManager.AutoManager_Application.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // Obtener todos los vehículos
        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Select(MapToDto);
        }

        // Obtener vehículo por Id
        public async Task<VehicleDto?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return vehicle == null ? null : MapToDto(vehicle);
        }

        // Agregar vehículo
        public async Task<VehicleDto> AddVehicleAsync(Vehicle vehicle)
        {
            // Validar que no exista otro vehículo con el mismo SerialNumber
            var existing = await _vehicleRepository.GetBySerialNumberAsync(vehicle.SerialNumber);
            if (existing != null)
            {
                throw new InvalidOperationException($"Ya existe un vehículo con el número de serie {vehicle.SerialNumber}");
            }

            await _vehicleRepository.AddAsync(vehicle);
            return MapToDto(vehicle);
        }

        // Actualizar vehículo
        // Actualizar vehículo
        public async Task<VehicleDto> UpdateVehicleAsync(Vehicle inputVehicle)  // Renombré el parámetro para claridad
        {
            // 1. Carga la entidad EXISTENTE (se trackea automáticamente)
            var existingVehicle = await _vehicleRepository.GetByIdAsync(inputVehicle.Id);
            if (existingVehicle == null)
            {
                throw new InvalidOperationException($"No se encontró el vehículo con ID {inputVehicle.Id}");
            }

            // 2. Validación de SerialNumber (usa AsNoTracking en el repo si puedes, para optimizar)
            var duplicate = await _vehicleRepository.GetBySerialNumberAsync(inputVehicle.SerialNumber);
            if (duplicate != null && duplicate.Id != inputVehicle.Id)
            {
                throw new InvalidOperationException($"Ya existe otro vehículo con el número de serie {inputVehicle.SerialNumber}");
            }

            // 3. Copia TODAS las propiedades de inputVehicle a existingVehicle
            // (Esto es manual; si usas AutoMapper, reemplázalo con mapper.Map(inputVehicle, existingVehicle))
            existingVehicle.Brand = inputVehicle.Brand;
            existingVehicle.Model = inputVehicle.Model;
            existingVehicle.Year = inputVehicle.Year;
            existingVehicle.SerialNumber = inputVehicle.SerialNumber;
            existingVehicle.LicensePlate = inputVehicle.LicensePlate;
            existingVehicle.Color = inputVehicle.Color;
            existingVehicle.Status = inputVehicle.Status;
            existingVehicle.Location = inputVehicle.Location;
            existingVehicle.PurchasePrice = inputVehicle.PurchasePrice;
            existingVehicle.SalePrice = inputVehicle.SalePrice;
            existingVehicle.Mileage = inputVehicle.Mileage;
            existingVehicle.FuelType = inputVehicle.FuelType;
            existingVehicle.Transmission = inputVehicle.Transmission;
            existingVehicle.ImageUrl = inputVehicle.ImageUrl;
            existingVehicle.Notes = inputVehicle.Notes;
            existingVehicle.OwnerId = inputVehicle.OwnerId;
            // Nota: No copies Owner, ya que es una navegación; se maneja en el DTO

            // 4. Actualiza la entidad ya trackeada (EF la marca como Modified)
            await _vehicleRepository.UpdateAsync(existingVehicle);

            // 5. Retorna el DTO (ya está actualizado, no necesitas GetById extra)
            return MapToDto(existingVehicle);
        }

        // Eliminar vehículo
        public async Task DeleteVehicleAsync(int id)
        {
            var existing = await _vehicleRepository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new InvalidOperationException($"No se encontró el vehículo con ID {id}");
            }

            await _vehicleRepository.DeleteAsync(id);
        }

        // Método privado para mapear Vehicle a VehicleDto (CON TODOS LOS CAMPOS)
        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                SerialNumber = vehicle.SerialNumber,
                LicensePlate = vehicle.LicensePlate,
                Color = vehicle.Color,
                Status = vehicle.Status,                     // ⚠️ FALTABA
                Location = vehicle.Location,                 // ⚠️ FALTABA
                PurchasePrice = vehicle.PurchasePrice,       // ⚠️ FALTABA
                SalePrice = vehicle.SalePrice,               // ⚠️ FALTABA
                Mileage = vehicle.Mileage,                   // ⚠️ FALTABA
                FuelType = vehicle.FuelType,                 // ⚠️ FALTABA
                Transmission = vehicle.Transmission,         // ⚠️ FALTABA
                ImageUrl = vehicle.ImageUrl,                 // ⚠️ FALTABA
                Notes = vehicle.Notes,                       // ⚠️ FALTABA
                OwnerId = vehicle.OwnerId,                   // ⚠️ FALTABA
                OwnerName = vehicle.Owner?.FullName
            };
        }
    }
}