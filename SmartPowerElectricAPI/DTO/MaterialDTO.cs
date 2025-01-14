using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.DTO;

public partial class MaterialDTO
{
   

    public double? Precio { get; set; }

    public double? Cantidad { get; set; }
  
    public int? IdTipoMaterial { get; set; }

    public int? IdUnidadMedida { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
 
}
