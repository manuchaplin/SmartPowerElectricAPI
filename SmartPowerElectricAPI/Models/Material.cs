using System;
using System.Collections.Generic;

namespace SmartPowerElectricAPI.Models;

public partial class Material
{
    public int Id { get; set; }

    public double? Precio { get; set; }

    public double? Cantidad { get; set; }

    public int IdTipoMaterial { get; set; }

    public int IdUnidadMedida { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }

    public virtual TipoMaterial TipoMaterial { get; set; }

    public virtual UnidadMedidum UnidadMedidum { get; set; }
}
