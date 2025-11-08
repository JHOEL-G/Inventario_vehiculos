using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;
using AutoManager.AutoManager_Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace AutoManager.AutoManager_Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.Vehicles)
                .ToListAsync();
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Client client)
        {
            // Detach cualquier entidad rastreada con el mismo ID
            var tracked = _context.ChangeTracker.Entries<Client>()
                .FirstOrDefault(e => e.Entity.Id == client.Id);

            if (tracked != null)
            {
                tracked.State = EntityState.Detached;
            }

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
    }
}