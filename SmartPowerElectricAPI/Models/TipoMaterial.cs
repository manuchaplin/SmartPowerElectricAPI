using System;
using System.Collections.Generic;

namespace SmartPowerElectricAPI.Models;

public partial class TipoMaterial
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
