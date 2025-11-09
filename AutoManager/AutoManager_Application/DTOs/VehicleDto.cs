using AutoManager.AutoManager_Domain.Entidades;
using System.Text.Json.Serialization;

namespace AutoManager.AutoManager_Application.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        public int ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;

        public int Year { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string? LicensePlate { get; set; }
        public string? Color { get; set; }
        public VehicleStatus Status { get; set; }
        public string? Location { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public double Mileage { get; set; }
        public FuelType FuelType { get; set; }
        public TransmissionType Transmission { get; set; }
        public string? ImageUrl { get; set; }
        public string? Notes { get; set; }
        public int? OwnerId { get; set; }
        public string? OwnerName { get; set; }
    }
}