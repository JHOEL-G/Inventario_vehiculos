namespace AutoManager.AutoManager_Domain.Entidades
{
    public class Client
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Identification { get; set; }

        public ClientType Clienttype { get; set; }
        public string? Notes { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }

    public enum ClientType
    {
        Particular,
        Empresa,
        Individual
    }
}
