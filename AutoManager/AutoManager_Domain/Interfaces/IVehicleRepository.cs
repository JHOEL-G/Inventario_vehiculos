using AutoManager.AutoManager_Domain.Entidades;
using System.Linq.Expressions;

namespace AutoManager.AutoManager_Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(int id, params Expression<Func<Vehicle, object>>[] includes);
        Task<IEnumerable<Vehicle>> GetAllAsync(params Expression<Func<Vehicle, object>>[] includes);
        Task<Vehicle?> GetBySerialNumberAsync(string serialNumber); // NUEVO - Para validar duplicados
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(int id);
    }
}