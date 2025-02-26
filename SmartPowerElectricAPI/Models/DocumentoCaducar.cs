using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.Models
{
    public class DocumentoCaducar
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
       
        public DateTime? FechaExpiracion { get; set; }

        public DateTime? FechaExpedicion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }

        public virtual Trabajador Trabajador { get; set; }
    }
}
