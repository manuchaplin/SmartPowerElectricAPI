using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.Models;

public class UnidadMedida
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string UMedida { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; }
}
