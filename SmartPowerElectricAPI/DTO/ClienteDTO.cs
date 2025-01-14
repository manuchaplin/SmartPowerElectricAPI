using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class ClienteDTO
{
      
    public string? Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
}
