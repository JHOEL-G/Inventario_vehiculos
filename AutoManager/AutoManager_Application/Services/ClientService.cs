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
                Phone = c.Phone,  // ← Corregido: era "Pone"
                Email = c.Email,
                Address = c.Address,  // ← Agregado
                City = c.City,
                Identification = c.Identification,  // ← Corregido: era "Identificacion"
                ClientType = c.Clienttype.ToString(),  // ← Agregado
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
                Phone = c.Phone,  // ← Corregido
                Email = c.Email,
                Address = c.Address,  // ← Agregado
                City = c.City,
                Identification = c.Identification,  // ← Corregido
                ClientType = c.Clienttype.ToString(),  // ← Agregado
                Notes = c.Notes
            };
        }

        public async Task AddClientAsync(Client client)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(client.FullName))
                throw new ArgumentException("El nombre completo es requerido.");

            if (string.IsNullOrWhiteSpace(client.Phone))
                throw new ArgumentException("El teléfono es requerido.");

            await _clientRepository.AddAsync(client);
        }

        public async Task UpdateClientAsync(Client client)
        {
            var existing = await _clientRepository.GetByIdAsync(client.Id);
            if (existing == null)
                throw new InvalidOperationException($"El cliente con ID {client.Id} no existe.");

            await _clientRepository.UpdateAsync(client);
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);

            if (client == null)
                throw new InvalidOperationException($"El cliente con ID {id} no existe.");

            await _clientRepository.DeleteAsync(id);
        }
    }
}