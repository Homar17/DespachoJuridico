using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación (Un Rol puede tener muchos Usuarios)
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}