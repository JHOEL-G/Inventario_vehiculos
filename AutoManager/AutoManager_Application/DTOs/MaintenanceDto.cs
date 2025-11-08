using System.ComponentModel.DataAnnotations;  // Para [Required]
using System.Text.Json.Serialization;  // Para JsonPropertyName, opcional

namespace AutoManager.AutoManager_Application.DTOs
{
    public class MaintenanceDto
    {
        public int Id { get; set; }

        [JsonPropertyName("vehicle_serial_number")]  // Para snake_case en JSON del frontend
        public string? VehicleSerialNumber { get; set; }

        [JsonPropertyName("vehicle_brand")]
        public string? VehicleBrand { get; set; }

        [Required(ErrorMessage = "El tipo de mantenimiento es requerido")]
        [JsonPropertyName("maintenance_type")]
        public string? MaintenanceType { get; set; }  // String para JSON; mapea a enum en service

        [Required(ErrorMessage = "La fecha de servicio es requerida")]
        [JsonPropertyName("service_date")]
        public DateTime ServiceDate { get; set; }

        [JsonPropertyName("next_service_date")]
        public DateTime? NextServiceDate { get; set; }  // Nullable para opcional

        [Required(ErrorMessage = "La descripción es requerida")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }  // <- Agregado: Faltaba de la entidad

        [JsonPropertyName("parts_replaced")]
        public string? PartsReplaced { get; set; }  // <- Agregado: Faltaba de la entidad

        [Required(ErrorMessage = "El costo es requerido")]
        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }

        [JsonPropertyName("mechanic")]
        public string? Mechanic { get; set; }

        [Required(ErrorMessage = "El kilometraje es requerido")]
        [JsonPropertyName("mileage_at_service")]
        public double MileageAtService { get; set; }

        [JsonPropertyName("vehicle_id")]  // <- Agregado: Para FK en create/update
        public int? VehicleId { get; set; }

        // Campos del dialog (si los usas)
        [JsonPropertyName("status")]
        public string? Status { get; set; } = "pendiente";

        [JsonPropertyName("priority")]
        public string? Priority { get; set; } = "media";
    }
}