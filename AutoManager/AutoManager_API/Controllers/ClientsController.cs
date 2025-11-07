using AutoManager.AutoManager_Application.Services;
using AutoManager.AutoManager_Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace AutoManager.AutoManager_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _clienteService;

        public ClientsController(ClientService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clienteService.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clienteService.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create( Client client)
        {
            await _clienteService.AddClientAsync(client);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Client client)
        {
            if (id != client.Id) return BadRequest();
            await _clienteService.UpdateClientAsync(client);
            return NoContent();
        }
    }
}
