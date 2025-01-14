using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.Models;

public class UnidadMedida
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UMedida { get; set; }
    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }

    public virtual ICollection<Material> Materials { get; set; }


}
