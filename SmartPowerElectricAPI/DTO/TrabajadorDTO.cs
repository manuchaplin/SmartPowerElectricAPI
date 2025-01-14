using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class TrabajadorDTO
{
   
    public string? Nombre { get; set; } = null!;

    public string? Apellido { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? SeguridadSocial { get; set; }

    public DateTime? FechaInicioContrato { get; set; }

    public DateTime? FechaFinContrato { get; set; }

    public double? CobroxHora { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
}
