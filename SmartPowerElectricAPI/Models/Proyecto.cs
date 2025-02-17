using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.Models
{
    public class Proyecto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string? Direccion { get; set; }  
        public string? Descripcion { get; set; }  
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        [ForeignKey("Cliente")]
        public int? IdCliente { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaEliminado { get; set; }

        public bool? Eliminado { get; set; }

        public double? horasEstimadas { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<Orden> Ordens { get; set; }

       

    }
}
