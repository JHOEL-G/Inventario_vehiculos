using System.ComponentModel.DataAnnotations;

namespace AutoManager.AutoManager_Domain.Entidades
{
    public class Model
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Relación con marca
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
    }
}
