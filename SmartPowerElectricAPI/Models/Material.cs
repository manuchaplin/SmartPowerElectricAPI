using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.Models;

public class Material
{
    [Key]
    public int Id { get; set; }

    public double? Precio { get; set; }

    public double? Cantidad { get; set; }
    [Required]
    [ForeignKey("TipoMaterial")]
    public int IdTipoMaterial { get; set; }
    [Required]
    [ForeignKey("UnidadMedida")]
    public int IdUnidadMedida { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }

    public virtual TipoMaterial TipoMaterial { get; set; }

    public virtual UnidadMedida UnidadMedida { get; set; }
}
