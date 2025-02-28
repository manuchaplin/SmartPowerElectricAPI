using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.Models;

public class Trabajador
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; } = null!;
    [Required]
    public string Apellido { get; set; } = null!;
    [Required]
    public string Especialidad { get; set; }
    [Required]
    public string Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }
    [Required]
    public string SeguridadSocial { get; set; }

    public DateTime? FechaInicioContrato { get; set; }

    public DateTime? FechaFinContrato { get; set; }

    public double? CobroxHora { get; set; }
    [Required]
    public string NumeroCuenta { get; set; }
    [Required]
    public string Enrutamiento { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
    public virtual ICollection<Orden> Ordens { get; set; }
    public virtual ICollection<Nomina> Nominas { get; set; }
    public virtual ICollection<DocumentoCaducar> DocumentoCaducars { get; set; }


}
