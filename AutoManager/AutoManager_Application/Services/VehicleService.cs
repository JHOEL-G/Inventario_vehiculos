using AutoManager.AutoManager_Application.DTOs;
using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;
using AutoManager.AutoManager_Infrastructure.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AutoManager.AutoManager_Application.Services
{
    public class VehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(
            IVehicleRepository vehicleRepository,
            ApplicationDbContext context,
            ILogger<VehicleService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles.Select(MapToDto);
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return vehicle == null ? null : MapToDto(vehicle);
        }

        public async Task<VehicleDto> AddVehicleAsync(VehicleDto dto)
        {
            try
            {
                var existing = await _vehicleRepository.GetBySerialNumberAsync(dto.SerialNumber);
                if (existing != null)
                    throw new InvalidOperationException($"Ya existe un vehículo con el número de serie {dto.SerialNumber}");

                _logger.LogInformation("Guardando vehículo con imagen referenciada por URL.");

                var vehicle = MapToEntity(dto);
                var created = await _vehicleRepository.AddAsync(vehicle);

                return MapToDto(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear vehículo");
                throw;
            }
        }

        public async Task<VehicleDto> UpdateVehicleAsync(VehicleDto dto)
        {
            try
            {
                
                var existingVehicle = await _vehicleRepository.GetByIdAsync(dto.Id);
                if (existingVehicle == null)
                    throw new KeyNotFoundException($"Vehículo con ID {dto.Id} no encontrado");

                var duplicate = await _vehicleRepository.GetBySerialNumberAsync(dto.SerialNumber);
                if (duplicate != null && duplicate.Id != dto.Id)
                    throw new InvalidOperationException($"Ya existe otro vehículo con el número de serie {dto.SerialNumber}");

                _logger.LogInformation("Actualizando vehículo.");

                var vehicleToUpdate = MapToEntity(dto);

                var updated = await _vehicleRepository.UpdateAsync(vehicleToUpdate);

                return MapToDto(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar vehículo {VehicleId}", dto.Id);
                throw;
            }
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            try
            {
                var vehicle = await _vehicleRepository.GetByIdAsync(id);
                if (vehicle == null) return false;

                _logger.LogInformation("Eliminando vehículo {VehicleId}", id);

                return await _vehicleRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar vehículo {VehicleId}", id);
                throw;
            }
        }

        private static VehicleDto MapToDto(Vehicle vehicle)
        {
            return new VehicleDto
            {
                Id = vehicle.Id,
                BrandId = vehicle.BrandId,
                BrandName = vehicle.Brand?.Name ?? string.Empty,
                ModelId = vehicle.ModelId,
                ModelName = vehicle.Model?.Name ?? string.Empty,
                Year = vehicle.Year, // <--- Incluir todos los campos
                SerialNumber = vehicle.SerialNumber,
                LicensePlate = vehicle.LicensePlate,
                Color = vehicle.Color,
                Status = vehicle.Status,
                Location = vehicle.Location,
                PurchasePrice = vehicle.PurchasePrice,
                SalePrice = vehicle.SalePrice,
                Mileage = vehicle.Mileage,
                FuelType = vehicle.FuelType,
                Transmission = vehicle.Transmission,

                // Solo ImageUrl
                ImageUrl = vehicle.ImageUrl,

                Notes = vehicle.Notes,
                OwnerId = vehicle.OwnerId,
                OwnerName = vehicle.Owner?.FullName
            };
        }


        private static Vehicle MapToEntity(VehicleDto dto)
        {
            // NO MÁS LÓGICA DE BASE64. Asumimos que dto.ImageUrl es ahora la URL corta.

            return new Vehicle
            {
                Id = dto.Id,
                BrandId = dto.BrandId,
                ModelId = dto.ModelId,
                Year = dto.Year,
                SerialNumber = dto.SerialNumber,
                LicensePlate = dto.LicensePlate,
                Color = dto.Color,
                Status = dto.Status,
                Location = dto.Location,
                PurchasePrice = dto.PurchasePrice,
                SalePrice = dto.SalePrice,
                Mileage = dto.Mileage,
                FuelType = dto.FuelType,
                Transmission = dto.Transmission,

                // ⭐️ CAMBIO CLAVE: SÓLO Mapear ImageUrl
                ImageUrl = dto.ImageUrl,

                // ELIMINAR CUALQUIER REFERENCIA A ImageBase64
                Notes = dto.Notes,
                OwnerId = dto.OwnerId
            };
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<List<Model>> GetModelsByBrandAsync(int brandId)
        {
            return await _context.Models
                .Where(m => m.BrandId == brandId)
                .ToListAsync();
        }
    }
}
