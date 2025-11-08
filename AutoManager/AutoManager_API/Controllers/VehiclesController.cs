using AutoManager.AutoManager_Application.DTOs;
using AutoManager.AutoManager_Application.Services;
using AutoManager.AutoManager_Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace AutoManager.AutoManager_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleService _vehicleService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(VehicleService vehicleService, ILogger<VehiclesController> logger)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener vehículos");
                return StatusCode(500, new { message = "Error interno al listar vehículos." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetById(int id)
        {
            if (id <= 0) return BadRequest(new { message = "ID inválido." });

            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null) return NotFound(new { message = "Vehículo no encontrado." });
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create([FromBody] VehicleDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Mapear DTO a entidad
                var vehicle = new Vehicle
                {
                    Brand = dto.Brand,
                    Model = dto.Model,
                    Year = dto.Year,
                    SerialNumber = dto.SerialNumber,
                    LicensePlate = dto.LicensePlate,
                    Color = dto.Color,
                    Status = dto.Status,
                    Location = dto.Location,
                    PurchasePrice = dto.PurchasePrice,
                    SalePrice = dto.SalePrice,
                    Mileage = dto.Mileage,
                    FuelType = dto.FuelType,
                    Transmission = dto.Transmission,
                    ImageUrl = dto.ImageUrl,
                    Notes = dto.Notes,
                    OwnerId = dto.OwnerId
                };

                var createdDto = await _vehicleService.AddVehicleAsync(vehicle);
                return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Duplicado al crear vehículo: {SerialNumber}", dto.SerialNumber);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear vehículo");
                return StatusCode(500, new { message = "Error interno al crear vehículo." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleDto dto)
        {
            if (id != dto.Id) return BadRequest(new { message = "ID mismatch." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Mapear DTO a entidad
                var vehicle = new Vehicle
                {
                    Id = dto.Id,
                    Brand = dto.Brand,
                    Model = dto.Model,
                    Year = dto.Year,
                    SerialNumber = dto.SerialNumber,
                    LicensePlate = dto.LicensePlate,
                    Color = dto.Color,
                    Status = dto.Status,
                    Location = dto.Location,
                    PurchasePrice = dto.PurchasePrice,
                    SalePrice = dto.SalePrice,
                    Mileage = dto.Mileage,
                    FuelType = dto.FuelType,
                    Transmission = dto.Transmission,
                    ImageUrl = dto.ImageUrl,
                    Notes = dto.Notes,
                    OwnerId = dto.OwnerId
                };

                var updatedDto = await _vehicleService.UpdateVehicleAsync(vehicle);
                return Ok(updatedDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error al actualizar vehículo {Id}", id);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar vehículo {Id}", id);
                return StatusCode(500, new { message = "Error interno al actualizar." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(new { message = "ID inválido." });
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return Ok(new { message = $"Vehículo {id} eliminado exitosamente." });  // <- Mueve aquí: 200 con body
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Vehículo no encontrado al eliminar {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar vehículo {Id}", id);
                return StatusCode(500, new { message = "Error interno al eliminar." });
            }
        }
    }
}