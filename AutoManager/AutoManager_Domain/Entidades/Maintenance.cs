namespace AutoManager.AutoManager_Domain.Entidades
{
    public class Maintenance
    {
        public int Id { get; set; }
        public MaintenanceType maintenancetype { get; set; }

        public DateTime ServiceDate { get; set; }
        public DateTime NextServiceDate { get; set; }

        public string? Description { get; set; }
        public string? PartsReplaced { get; set; }

        public decimal Cost { get; set; }
        public string? Mechanic { get; set; }
        public double MileageAtService { get; set; }

        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
    }

    public enum MaintenanceType
    {
        Preventivo,
        Correctivo,
        Revision,
        Reparacion
    }
}
