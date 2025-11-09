using AutoManager.AutoManager_Application.DTOs;
using AutoManager.AutoManager_Application.Services;
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
                // ✅ Pasar el DTO directamente al Service
                var createdDto = await _vehicleService.AddVehicleAsync(dto);
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
                // ✅ Pasar el DTO directamente al Service
                var updatedDto = await _vehicleService.UpdateVehicleAsync(dto);
                return Ok(updatedDto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Vehículo no encontrado {Id}", id);
                return NotFound(new { message = ex.Message });
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
                var deleted = await _vehicleService.DeleteVehicleAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = $"Vehículo {id} no encontrado." });
                }

                return Ok(new { message = $"Vehículo {id} eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar vehículo {Id}", id);
                return StatusCode(500, new { message = "Error interno al eliminar." });
            }
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _vehicleService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("brands/{brandId}/models")]
        public async Task<IActionResult> GetModelsByBrand(int brandId)
        {
            var models = await _vehicleService.GetModelsByBrandAsync(brandId);
            return Ok(models);
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No se ha proporcionado ninguna imagen." });

            // 1. Definir la ruta donde guardar la imagen (ej: wwwroot/images)
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // 2. Crear un nombre de archivo único
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            // 3. Guardar el archivo físicamente
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 4. Crear la URL que el frontend usará para acceder
                // (Asumiendo que el hosting está configurado para servir 'wwwroot' como raíz)
                var publicUrl = $"/images/{fileName}";

                // 5. Devolver la URL pública y corta
                return Ok(new { imageUrl = publicUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar imagen");
                return StatusCode(500, new { message = $"Error al guardar la imagen: {ex.Message}" });
            }
        }


    }
}