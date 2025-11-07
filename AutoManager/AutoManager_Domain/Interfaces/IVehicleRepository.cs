using AutoManager.AutoManager_Domain.Entidades;

namespace AutoManager.AutoManager_Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetByIdAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(int id);
    }
}
