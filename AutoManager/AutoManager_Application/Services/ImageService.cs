namespace AutoManager.AutoManager_Application.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageService> _logger;

        public ImageService(IWebHostEnvironment environment, ILogger<ImageService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Guarda una imagen en formato Base64 y retorna la URL relativa
        /// </summary>
        public async Task<string> SaveVehicleImageAsync(string base64Image)
        {
            try
            {
                // 1. Limpiar el Base64 (remover prefijo data:image/...;base64,)
                var base64Data = base64Image;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }

                // 2. Convertir a bytes
                byte[] imageBytes;
                try
                {
                    imageBytes = Convert.FromBase64String(base64Data);
                }
                catch (FormatException)
                {
                    throw new ArgumentException("El formato Base64 es inválido");
                }

                // 3. Validar tamaño (máximo 5MB)
                const int maxSizeBytes = 5 * 1024 * 1024;
                if (imageBytes.Length > maxSizeBytes)
                {
                    throw new InvalidOperationException("La imagen excede el tamaño máximo de 5MB");
                }

                // 4. Generar nombre único
                var fileName = $"{Guid.NewGuid()}.jpg";

                // 5. Crear ruta de carpeta
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "vehicles");

                // 6. Crear carpeta si no existe (por si acaso)
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Carpeta de uploads creada: {UploadsFolder}", uploadsFolder);
                }

                // 7. Ruta completa del archivo
                var filePath = Path.Combine(uploadsFolder, fileName);

                // 8. Guardar archivo
                await File.WriteAllBytesAsync(filePath, imageBytes);

                _logger.LogInformation("Imagen guardada: {FileName} ({Size} bytes)", fileName, imageBytes.Length);

                // 9. Retornar URL relativa
                return $"/uploads/vehicles/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar imagen de vehículo");
                throw;
            }
        }

        /// <summary>
        /// Elimina una imagen del servidor
        /// </summary>
        public void DeleteVehicleImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                // Extraer nombre del archivo de la URL
                // Ejemplo: "/uploads/vehicles/abc123.jpg" -> "abc123.jpg"
                var fileName = Path.GetFileName(imageUrl);
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", "vehicles", fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Imagen eliminada: {FileName}", fileName);
                }
                else
                {
                    _logger.LogWarning("Imagen no encontrada para eliminar: {FilePath}", filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen: {ImageUrl}", imageUrl);
                // No lanzamos excepción para no bloquear la eliminación del vehículo
            }
        }
    }
}
