using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class UnidadMedidaDTO
{
    public int? Id { get; set; }
    public string? UMedida { get; set; }
    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }


    public static UnidadMedidaDTO FromEntity(UnidadMedida unidadMedida)
    {
        return new UnidadMedidaDTO
        {
            Id = unidadMedida.Id,
            UMedida = unidadMedida.UMedida,
            FechaCreacion = unidadMedida.FechaCreacion?.ToString("yyyy-MM-dd"),
        };
    }

    // Constructor para mapear desde TrabajadorDTO
    public UnidadMedida ToEntity()
    {
        return new UnidadMedida
        {
            UMedida = this.UMedida,
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null)
        };
    }

}
