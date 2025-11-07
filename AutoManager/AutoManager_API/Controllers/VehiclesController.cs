using AutoManager.AutoManager_Application.Services;
using AutoManager.AutoManager_Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace AutoManager.AutoManager_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly MaintenanceService _maintenanceService;

        public VehiclesController(MaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var maintenances = await _maintenanceService.GetAllMaintenancesAsync();
            return Ok(maintenances);
        }

        [HttpGet("{vehicleId}/maintenances")]
        public async Task<IActionResult> GetByVehicle(int vehicleId)
        {
            var maintenances = await _maintenanceService.GetMaintenancesByVehicleAsync(vehicleId);
            return Ok(maintenances);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Maintenance maintenance)
        {
            await _maintenanceService.AddMaintenanceAsync(maintenance);
            return CreatedAtAction(nameof(GetAll), new { id = maintenance.Id }, maintenance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Maintenance maintenance)
        {
            if (id != maintenance.Id) return BadRequest();
            
            await _maintenanceService.UpdateMaintenanceAsync(maintenance);
            return NoContent();
        }
    }
}
