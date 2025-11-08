namespace AutoManager.AutoManager_Application.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }  // ← Era "Pone"
        public string? Email { get; set; }
        public string? Address { get; set; }  // ← Agregado
        public string? City { get; set; }
        public string? Identification { get; set; }  // ← Era "Identificacion"
        public string? ClientType { get; set; }  // ← Agregado
        public string? Notes { get; set; }
    }
}