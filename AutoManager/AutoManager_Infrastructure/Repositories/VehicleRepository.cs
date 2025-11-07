using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;
using AutoManager.AutoManager_Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace AutoManager.AutoManager_Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.MaintenanceRecords)
                .ToListAsync();
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.MaintenanceRecords)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            return _context.SaveChangesAsync();
        }
    }
}
