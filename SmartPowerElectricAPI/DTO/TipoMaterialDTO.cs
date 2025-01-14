using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class TipoMaterialDTO
{

    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
   
}
