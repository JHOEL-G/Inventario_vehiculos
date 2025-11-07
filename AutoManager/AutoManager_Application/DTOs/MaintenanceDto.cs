namespace AutoManager.AutoManager_Application.DTOs
{
    public class MaintenanceDto
    {
        public int Id { get; set; }
        public string? VehicleSerialNumber { get; set; }
        public string? VehicleBrand { get; set; }
        public string? MaintenanceType { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime NextServiceDate { get; set; }
        public decimal Cost { get; set; }
        public string? Mechanic { get; set; }
        public double MileageAtService { get; set; }
    }
}
