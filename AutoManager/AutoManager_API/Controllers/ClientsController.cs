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
        public async Task<IActionResult> Create(Client client)
        {
            try
            {
                await _clienteService.AddClientAsync(client);
                return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            try
            {
                await _clienteService.UpdateClientAsync(client);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.DeleteClientAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}