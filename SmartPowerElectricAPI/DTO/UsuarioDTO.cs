using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class UsuarioDTO
{

  
    public string? Nombre { get; set; } = null!;
   
    public string? Apellido { get; set; } = null!;
  
    public string? Email { get; set; } = null!;
   
    public string? Username { get; set; } = null!;
   
    public string? Password { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
}
