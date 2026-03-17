using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Cita")]
    public class Cita
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int id_user { get; set; }
        [ForeignKey("id_user")]
        public Usuario? Usuario { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;
    }
}