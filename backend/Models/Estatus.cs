using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Estatus")]
    public class Estatus
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        public ICollection<Caso> Casos { get; set; } = new List<Caso>();
    }
}