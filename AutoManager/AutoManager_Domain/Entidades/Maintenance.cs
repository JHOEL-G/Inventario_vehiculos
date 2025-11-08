using System.ComponentModel.DataAnnotations;  // Para [Required] – opcional, pero útil

namespace AutoManager.AutoManager_Domain.Entidades
{
    public class Maintenance
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de mantenimiento es requerido")]
        public MaintenanceType MaintenanceType { get; set; }  // <- Fix: PascalCase para convención C#

        [Required(ErrorMessage = "La fecha de servicio es requerida")]
        public DateTime ServiceDate { get; set; }

        public DateTime? NextServiceDate { get; set; }  // <- Fix: Nullable, ya que es opcional en dialog

        [Required(ErrorMessage = "La descripción es requerida")]
        public string? Description { get; set; }

        public string? PartsReplaced { get; set; }

        [Required(ErrorMessage = "El costo es requerido")]
        public decimal Cost { get; set; }

        public string? Mechanic { get; set; }

        [Required(ErrorMessage = "El kilometraje es requerido")]
        public double MileageAtService { get; set; }

        [Required(ErrorMessage = "El ID del vehículo es requerido")]
        public int VehicleId { get; set; }

        public virtual Vehicle? Vehicle { get; set; }  // virtual para lazy loading en EF

        // Campos del dialog (agrega si los necesitas; si no, ignora)
        public string Status { get; set; } = "pendiente";  // ej. "pendiente", "completado"
        public string Priority { get; set; } = "media";    // ej. "baja", "alta"
    }

    public enum MaintenanceType
    {
        Preventivo,    // 0 – Mapea directo del Select del dialog
        Correctivo,    // 1
        Revision,      // 2
        Reparacion     // 3
    }
}