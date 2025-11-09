using System.ComponentModel.DataAnnotations;

namespace AutoManager.AutoManager_Domain.Entidades
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Relación con modelos
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
