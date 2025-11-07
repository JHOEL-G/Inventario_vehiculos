using AutoManager.AutoManager_Application.DTOs;
using AutoManager.AutoManager_Domain.Entidades;
using AutoManager.AutoManager_Domain.Interfaces;

namespace AutoManager.AutoManager_Application.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync()
        {
            var clients = await _clientRepository.GetAllAsync();
            return clients.Select(c => new ClientDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Pone = c.Phone,
                Email = c.Email,
                City = c.City,
                Identificacion = c.Identification,
                Notes = c.Notes
            });
        }
        public async Task<ClientDto?> GetClientByIdAsync(int id)
        {
            var c = await _clientRepository.GetByIdAsync(id);
            if (c == null) return null;
            return new ClientDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Pone = c.Phone,
                Email = c.Email,
                City = c.City,
                Identificacion = c.Identification,
                Notes = c.Notes
            };
        }
        public async Task AddClientAsync(Client client)
        {
            await _clientRepository.AddAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            await _clientRepository.UpdateAsync(client);
        }
    }
}
