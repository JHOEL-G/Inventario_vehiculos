using AutoManager.AutoManager_Application.DTOs;
using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;

namespace AutoManager.AutoManager_Application.Services
{
    public class MaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;

        public MaintenanceService(IMaintenanceRepository maintenanceRepository)
        {
            _maintenanceRepository = maintenanceRepository;
        }

        public async Task<IEnumerable<MaintenanceDto>> GetAllMaintenancesAsync()
        {
            var maintenances = await _maintenanceRepository.GetAllAsync();
            return maintenances.Select(m => new MaintenanceDto
            {
                Id = m.Id,
                VehicleSerialNumber = m.Vehicle?.SerialNumber,
                VehicleBrand = m.Vehicle?.Brand,
                MaintenanceType = m.maintenancetype.ToString(),
                ServiceDate = m.ServiceDate,
                NextServiceDate = m.NextServiceDate,
                Cost = m.Cost,
                Mechanic = m.Mechanic,
                MileageAtService = m.MileageAtService
            });
        }

        public async Task<IEnumerable<MaintenanceDto>> GetMaintenancesByVehicleAsync(int vehicleId)
        {
            var maintenances = await _maintenanceRepository.GetByVehicleIdAsync(vehicleId);
            return maintenances.Select(m => new MaintenanceDto
            {
                Id = m.Id,
                VehicleSerialNumber = m.Vehicle?.SerialNumber,
                VehicleBrand = m.Vehicle?.Brand,
                MaintenanceType = m.maintenancetype.ToString(),
                ServiceDate = m.ServiceDate,
                NextServiceDate = m.NextServiceDate,
                Cost = m.Cost,
                Mechanic = m.Mechanic,
                MileageAtService = m.MileageAtService
            });
        }

        public async Task AddMaintenanceAsync(Maintenance maintenance)
        {
            await _maintenanceRepository.AddAsync(maintenance);
        }

        public async Task UpdateMaintenanceAsync(Maintenance maintenance)
        {
            await _maintenanceRepository.UpdateAsync(maintenance);
        }
    }
}
