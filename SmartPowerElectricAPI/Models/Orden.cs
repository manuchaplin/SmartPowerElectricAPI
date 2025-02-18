using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.Models
{
    public partial class Orden
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int NumeroOrden { get; set; }
        public bool? OrdenFinalizada {  get; set; }
        public double? CosteManoObra { get; set; }
        public double? Cobrado {  get; set; }
        public double? HorasEstimadas {  get; set; }
        [ForeignKey("Proyecto")]
        public int? IdProyecto { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaEliminado { get; set; }

        public bool? Eliminado { get; set; }

        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<Trabajador> Trabajadores { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }


    }
}
