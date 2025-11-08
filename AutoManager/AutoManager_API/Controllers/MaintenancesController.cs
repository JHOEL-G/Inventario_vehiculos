using AutoManager.AutoManager_Application.Services;
using AutoManager.AutoManager_Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace AutoManager.AutoManager_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenancesController : ControllerBase
    {
        private readonly MaintenanceService _maintenanceService;

        public MaintenancesController(MaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var maintenances = await _maintenanceService.GetAllMaintenancesAsync();
            return Ok(maintenances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var maintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);

            if (maintenance == null)
            {
                return NotFound(new { message = $"Maintenance with id {id} not found" });
            }

            return Ok(maintenance);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(int vehicleId)
        {
            var maintenances = await _maintenanceService.GetMaintenancesByVehicleAsync(vehicleId);
            return Ok(maintenances);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Maintenance maintenance)
        {
            try
            {
                var created = await _maintenanceService.AddMaintenanceAsync(maintenance);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
        public async Task<IActionResult> Update(int id, Maintenance maintenance)
        {
            if (id != maintenance.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            try
            {
                var updated = await _maintenanceService.UpdateMaintenanceAsync(maintenance);
                return Ok(updated);
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
                await _maintenanceService.DeleteMaintenanceAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}