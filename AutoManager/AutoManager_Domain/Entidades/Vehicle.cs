using Microsoft.VisualBasic.FileIO;

namespace AutoManager.AutoManager_Domain.Entidades
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? SerialNumber { get; set; }
        public string? LicensePlate { get; set; }
        public string? Color { get; set; }

        public VehicleStatus Status { get; set; }
        public string? Location { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public double? Mileage { get; set; }
        public FuelType Fueltype { get; set; }
        public TransmissionType Transmission { get; set; }
        public string? ImageUrl { get; set; }
        public string? Notes { get; set; }

        public int OwnerId { get; set; }
        public Client? Owner { get; set; }

        public ICollection<Maintenance> MaintenanceRecords { get; set; } = new List<Maintenance>();
    }

    public enum VehicleStatus
    {
        Disponible,
        Vendido,
        EnMantenimiento,
        Reservado
    }

    public enum FuelType
    {
        Gasolina,
        Diesel,
        Electrico,
        Hibrido,
        Gas
    }
    public enum TransmissionType
    {
        Manual,
        Automatica,
        Semiautomatica,
        Cvt
    }
}
