namespace AutoManager.AutoManager_Application.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? SerialNumber { get; set; }
        public string? LicensePlate { get; set; }
        public string? Color { get; set; }
        public string? OwnerName { get; set; }
    }
}
