using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string Contraseña { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string User { get; set; } = string.Empty;

        // Llave Foránea hacia Roles
        [Required]
        public int RolId { get; set; }

        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }

        public ICollection<Caso> Casos { get; set; } = new List<Caso>();
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}