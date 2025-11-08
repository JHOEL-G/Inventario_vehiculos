using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;
using AutoManager.AutoManager_Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace AutoManager.AutoManager_Infrastructure.Repositories
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Maintenance maintenance)
        {
            _context.MaintenanceRecords.Add(maintenance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var maintenance = await _context.MaintenanceRecords.FindAsync(id);
            if (maintenance != null)
            {
                _context.MaintenanceRecords.Remove(maintenance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Maintenance>> GetAllAsync()
        {
            return await _context.MaintenanceRecords
                .Include(m => m.Vehicle)
                .ThenInclude(v => v.Owner)
                .ToListAsync();
        }

        public async Task<Maintenance> GetByIdAsync(int id)
        {
            return await _context.MaintenanceRecords
                .Include(m => m.Vehicle)
                .ThenInclude(v => v.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Maintenance>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _context.MaintenanceRecords
                .Where(m => m.VehicleId == vehicleId)
                .Include(m => m.Vehicle)
                .ToListAsync();
        }

        public async Task UpdateAsync(Maintenance maintenance)
        {
            // Detach cualquier entidad rastreada con el mismo ID
            var tracked = _context.ChangeTracker.Entries<Maintenance>()
                .FirstOrDefault(e => e.Entity.Id == maintenance.Id);

            if (tracked != null)
            {
                tracked.State = EntityState.Detached;
            }

            // Ahora sí actualiza
            _context.MaintenanceRecords.Update(maintenance);
            await _context.SaveChangesAsync();
        }
    }
}