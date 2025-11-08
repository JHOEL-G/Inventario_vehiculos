using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;
using AutoManager.AutoManager_Infrastructure.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoManager.AutoManager_Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Vehicle?> GetByIdAsync(int id, params Expression<Func<Vehicle, object>>[] includes)
        {
            IQueryable<Vehicle> query = _context.Vehicles.Where(v => v.Id == id);

            // Aplica includes dinámicos
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Includes fijos por default
            query = query.Include(v => v.Owner).Include(v => v.MaintenanceRecords);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(params Expression<Func<Vehicle, object>>[] includes)
        {
            IQueryable<Vehicle> query = _context.Vehicles;

            // Aplica includes dinámicos
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Includes fijos por default (Owner y MaintenanceRecords)
            query = query.Include(v => v.Owner).Include(v => v.MaintenanceRecords);

            return await query.ToListAsync();
        }

        // NUEVO: Método para buscar por SerialNumber (validación de duplicados)
        public async Task<Vehicle?> GetBySerialNumberAsync(string serialNumber)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.SerialNumber == serialNumber);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
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
    }
}