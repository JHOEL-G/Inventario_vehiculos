using AutoManager.AutoManager_Domain.Entidades;

namespace AutoManager.AutoManager_Domain.Interfaces
{
    public interface IMaintenanceRepository
    {
        Task<Maintenance> GetByIdAsync(int id);
        Task<IEnumerable<Maintenance>> GetAllAsync();
        Task AddAsync(Maintenance maintenance);
        Task UpdateAsync(Maintenance maintenance);
        Task DeleteAsync(int id);

        Task<IEnumerable<Maintenance>> GetByVehicleIdAsync(int vehicleId);
    }
}
