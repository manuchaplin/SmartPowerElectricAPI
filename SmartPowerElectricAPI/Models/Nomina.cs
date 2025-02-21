using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.Models
{
    public class Nomina
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double horasTrabajadas { get; set; }      
        public double? SalarioEstandar {  get; set; }       
        public double? SalarioPlus { get; set; }
        public DateTime? FechaPago { get; set; }
        [Required]
        public int NoSemana { get; set; }
        [Required]
        public DateTime? InicioSemana { get; set; }
        [Required]
        public DateTime? FinSemana { get; set; }
        public DateTime? FechaCreacion { get; set; }
        [ForeignKey("Trabajador")]
        public int? IdTrabajador { get; set; }
        public virtual Trabajador Trabajador { get; set; }


    }
}
