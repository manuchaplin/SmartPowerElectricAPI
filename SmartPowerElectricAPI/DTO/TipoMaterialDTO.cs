using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class TipoMaterialDTO
{

    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;

    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }


    public static TipoMaterialDTO FromEntity(TipoMaterial tipoMaterial)
    {
        return new TipoMaterialDTO
        {
            Id = tipoMaterial.Id,
            Nombre = tipoMaterial.Nombre,       
            FechaCreacion = tipoMaterial.FechaCreacion?.ToString("yyyy-MM-dd")
        };
    }

    // Constructor para mapear desde TrabajadorDTO
    public TipoMaterial ToEntity()
    {
        return new TipoMaterial
        {
            Nombre = this.Nombre,
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null),
        
        };
    }
}
