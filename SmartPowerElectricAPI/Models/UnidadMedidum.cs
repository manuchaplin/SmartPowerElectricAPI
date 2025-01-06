using System;
using System.Collections.Generic;

namespace SmartPowerElectricAPI.Models;

public partial class UnidadMedidum
{
    public int Id { get; set; }

    public string UMedida { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
