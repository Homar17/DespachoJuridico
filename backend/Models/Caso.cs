using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("Caso")]
    public class Caso
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int id_usuario { get; set; }
        [ForeignKey("id_usuario")]
        public Usuario? Usuario { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int Id_estatus { get; set; }
        [ForeignKey("Id_estatus")]
        public Estatus? Estatus { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        public int Id_RamaJuridica { get; set; }
        [ForeignKey("Id_RamaJuridica")]
        public RamaJuridica? RamaJuridica { get; set; }
    }
}