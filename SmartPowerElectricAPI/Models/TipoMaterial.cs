using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.Models;

public class TipoMaterial
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nombre { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }

    public virtual ICollection<Material> Materials { get; set; }


    public string fechaAlta { get { return FechaCreacion?.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty; } }
}
