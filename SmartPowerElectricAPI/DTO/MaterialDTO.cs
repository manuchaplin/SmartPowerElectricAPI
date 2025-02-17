using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.DTO;

public partial class MaterialDTO
{

    public int? Id { get; set; }
    public double? Precio { get; set; }

    public double? Cantidad { get; set; }
  
    public int? IdTipoMaterial { get; set; }

    public int? IdUnidadMedida { get; set; }
    public int? IdOrden { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }


    public static MaterialDTO FromEntity(Material material)
    {
        return new MaterialDTO
        {
            Id = material.Id,
            Precio = material.Precio,
            Cantidad = material.Cantidad,
            IdTipoMaterial = material.IdTipoMaterial,
            IdUnidadMedida = material.IdUnidadMedida,                   
            IdOrden = material.IdOrden,
            FechaCreacion = material.FechaCreacion?.ToString("yyyy-MM-dd")
        };
    }

    // Constructor para mapear desde TrabajadorDTO
    public Material ToEntity()
    {
        return new Material
        {
            Precio = this.Precio,
            Cantidad = this.Cantidad,
            IdTipoMaterial = this.IdTipoMaterial,
            IdUnidadMedida = this.IdUnidadMedida,                   
            IdOrden = this.IdOrden,                   
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null)
           
        };
    }

}
