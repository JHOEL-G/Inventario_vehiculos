using AutoManager.AutoManager_Domain.Entidades;
using System.Linq.Expressions;

namespace AutoManager.AutoManager_Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(int id, params Expression<Func<Vehicle, object>>[] includes);
        Task<IEnumerable<Vehicle>> GetAllAsync(params Expression<Func<Vehicle, object>>[] includes);
        Task<Vehicle?> GetBySerialNumberAsync(string serialNumber);

        // ⭐ CAMBIOS: Ahora retornan valores
        Task<Vehicle> AddAsync(Vehicle vehicle);      // Retorna Vehicle con ID generado
        Task<Vehicle> UpdateAsync(Vehicle vehicle);   // Retorna Vehicle actualizado
        Task<bool> DeleteAsync(int id);               // Retorna true si se eliminó
    }
}