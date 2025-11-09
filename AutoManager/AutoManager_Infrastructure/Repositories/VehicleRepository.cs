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

            // ⭐ Includes fijos por default (AGREGADO Brand y Model)
            query = query
                .Include(v => v.Brand)              // ← AGREGADO
                .Include(v => v.Model)              // ← AGREGADO
                .Include(v => v.Owner)
                .Include(v => v.MaintenanceRecords);

            // Aplica includes dinámicos adicionales
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(params Expression<Func<Vehicle, object>>[] includes)
        {
            IQueryable<Vehicle> query = _context.Vehicles;

            // ⭐ Includes fijos por default (AGREGADO Brand y Model)
            query = query
                .Include(v => v.Brand)              // ← AGREGADO
                .Include(v => v.Model)              // ← AGREGADO
                .Include(v => v.Owner)
                .Include(v => v.MaintenanceRecords);

            // Aplica includes dinámicos adicionales
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<Vehicle?> GetBySerialNumberAsync(string serialNumber)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.SerialNumber == serialNumber);
        }

        // ⭐ CORREGIDO: Ahora retorna Vehicle
        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            // Cargar relaciones después de guardar para retornar objeto completo
            await _context.Entry(vehicle).Reference(v => v.Brand).LoadAsync();
            await _context.Entry(vehicle).Reference(v => v.Model).LoadAsync();

            if (vehicle.OwnerId.HasValue)
            {
                await _context.Entry(vehicle).Reference(v => v.Owner).LoadAsync();
            }

            return vehicle; // ← RETORNA el vehículo con ID y relaciones
        }

        // ⭐ CORREGIDO: Ahora retorna Vehicle
        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {
            // Buscar el vehículo existente
            var existingVehicle = await _context.Vehicles.FindAsync(vehicle.Id);

            if (existingVehicle == null)
            {
                throw new KeyNotFoundException($"Vehículo con ID {vehicle.Id} no encontrado");
            }

            // Actualizar las propiedades
            _context.Entry(existingVehicle).CurrentValues.SetValues(vehicle);
            await _context.SaveChangesAsync();

            // Cargar relaciones actualizadas
            await _context.Entry(existingVehicle).Reference(v => v.Brand).LoadAsync();
            await _context.Entry(existingVehicle).Reference(v => v.Model).LoadAsync();

            if (existingVehicle.OwnerId.HasValue)
            {
                await _context.Entry(existingVehicle).Reference(v => v.Owner).LoadAsync();
            }

            return existingVehicle; // ← RETORNA el vehículo actualizado
        }

        // ⭐ CORREGIDO: Ahora retorna bool
        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return false; // ← No se encontró, retorna false
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return true; // ← Se eliminó exitosamente, retorna true
        }
    }
}