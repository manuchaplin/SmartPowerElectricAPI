using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.Models
{
    public partial class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double MontoACobrar { get; set; }
        public string NumeroFactura { get; set; }
        public bool? EmailEnviado { get; set; }
        public bool? FacturaCompletada {  get; set; }
        [ForeignKey("Orden")]
        public int? IdOrden {  get; set; }
        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaEliminado { get; set; }

        public bool? Eliminado { get; set; }
        public virtual Orden Orden { get; set; }
    }
}
