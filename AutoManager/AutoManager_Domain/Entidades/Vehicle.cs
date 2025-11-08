using System.ComponentModel.DataAnnotations;

namespace AutoManager.AutoManager_Domain.Entidades
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty; // Quitado nullable

        [Required]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "El número de serie (VIN) es obligatorio.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "El VIN debe tener exactamente 17 caracteres.")]
        public string SerialNumber { get; set; } = string.Empty; // Quitado nullable y ahora required

        [MaxLength(20)]
        public string? LicensePlate { get; set; }

        [MaxLength(50)]
        public string? Color { get; set; }

        public VehicleStatus Status { get; set; } = VehicleStatus.Disponible;

        [MaxLength(200)]
        public string? Location { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor o igual a 0.")]
        public decimal PurchasePrice { get; set; } = 0; // Ya no nullable

        [Range(0, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor o igual a 0.")]
        public decimal SalePrice { get; set; } = 0; // Ya no nullable

        [Range(0, double.MaxValue, ErrorMessage = "El kilometraje debe ser mayor o igual a 0.")]
        public double Mileage { get; set; } = 0; // Ya no nullable

        public FuelType FuelType { get; set; } = FuelType.Gasolina;

        public TransmissionType Transmission { get; set; } = TransmissionType.Manual;

        [MaxLength(500)]
        [Url(ErrorMessage = "Debe ser una URL válida.")]
        public string? ImageUrl { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Relación con propietario
        public int? OwnerId { get; set; }
        public Client? Owner { get; set; }

        // Historial de mantenimientos
        public ICollection<Maintenance> MaintenanceRecords { get; set; } = new List<Maintenance>();
    }

    public enum VehicleStatus
    {
        Disponible = 0,
        Vendido = 1,
        EnMantenimiento = 2,
        Reservado = 3
    }

    public enum FuelType
    {
        Gasolina = 0,
        Diesel = 1,
        Electrico = 2,
        Hibrido = 3,
        Gas = 4
    }

    public enum TransmissionType
    {
        Manual = 0,
        Automatica = 1,
        Semiautomatica = 2,
        Cvt = 3
    }
}