using AutoManager.AutoManager_Application.DTOs;
using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoManager.AutoManager_Application.Services
{
    public class MaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public MaintenanceService(IMaintenanceRepository maintenanceRepository, IVehicleRepository vehicleRepository)
        {
            _maintenanceRepository = maintenanceRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<MaintenanceDto>> GetAllMaintenancesAsync()
        {
            var maintenances = await _maintenanceRepository.GetAllAsync();
            return maintenances.Select(MapToDto);
        }

        // AGREGA ESTE MÉTODO
        public async Task<MaintenanceDto?> GetMaintenanceByIdAsync(int id)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(id);

            if (maintenance == null)
                return null;

            return MapToDto(maintenance);
        }

        public async Task<IEnumerable<MaintenanceDto>> GetMaintenancesByVehicleAsync(int vehicleId)
        {
            var maintenances = await _maintenanceRepository.GetByVehicleIdAsync(vehicleId);
            return maintenances.Select(MapToDto);
        }

        public async Task<MaintenanceDto> AddMaintenanceAsync(Maintenance maintenance)
        {
            // 1. Validaciones (evita FK error)
            if (string.IsNullOrWhiteSpace(maintenance.Description))
                throw new ArgumentException("La descripción es requerida.");

            if (maintenance.ServiceDate == default(DateTime))
                throw new ArgumentException("La fecha de servicio es requerida.");

            var existingVehicle = await _vehicleRepository.GetByIdAsync(maintenance.VehicleId);
            if (existingVehicle == null)
                throw new InvalidOperationException($"El vehículo con ID {maintenance.VehicleId} no existe.");

            // 2. Default para NextServiceDate si no se setea
            if (maintenance.NextServiceDate == default(DateTime))
                maintenance.NextServiceDate = maintenance.ServiceDate.AddMonths(6);

            await _maintenanceRepository.AddAsync(maintenance);

            // 3. Retorna DTO con info del vehículo
            var added = await _maintenanceRepository.GetByIdAsync(maintenance.Id);
            return MapToDto(added!);
        }

        public async Task<MaintenanceDto> UpdateMaintenanceAsync(Maintenance maintenance)
        {
            // Validaciones similares a Add
            var existingVehicle = await _vehicleRepository.GetByIdAsync(maintenance.VehicleId);
            if (existingVehicle == null)
                throw new InvalidOperationException($"El vehículo con ID {maintenance.VehicleId} no existe.");

            await _maintenanceRepository.UpdateAsync(maintenance);
            var updated = await _maintenanceRepository.GetByIdAsync(maintenance.Id);
            return MapToDto(updated!);
        }

        // AGREGA ESTE MÉTODO
        public async Task DeleteMaintenanceAsync(int id)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(id);

            if (maintenance == null)
                throw new InvalidOperationException($"El mantenimiento con ID {id} no existe.");

            await _maintenanceRepository.DeleteAsync(id);
        }

        // Método privado para mapeo (evita duplicación)
        private static MaintenanceDto MapToDto(Maintenance m)
        {
            return new MaintenanceDto
            {
                Id = m.Id,
                VehicleSerialNumber = m.Vehicle?.SerialNumber,
                VehicleBrand = m.Vehicle?.Brand,
                MaintenanceType = m.MaintenanceType.ToString(),
                ServiceDate = m.ServiceDate,
                NextServiceDate = m.NextServiceDate,
                Cost = m.Cost,
                Mechanic = m.Mechanic,
                MileageAtService = m.MileageAtService,
                Description = m.Description,
                PartsReplaced = m.PartsReplaced
            };
        }
    }
}