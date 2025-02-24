using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class DocumentoCaducarDTO
{

    public int? Id { get; set; }
    public string? Nombre { get; set; }
    public string? FechaExpiracion { get; set; }
    public string? FechaCreacion { get; set; }
    public int? IdTrabajador { get; set; }


    public static DocumentoCaducarDTO FromEntity(DocumentoCaducar documentoCaducar)
    {
        return new DocumentoCaducarDTO
        {
            Id = documentoCaducar.Id,
            Nombre = documentoCaducar.Nombre,       
            FechaExpiracion = documentoCaducar.FechaExpiracion?.ToString("yyyy-MM-dd"),
            FechaCreacion = documentoCaducar.FechaCreacion?.ToString("yyyy-MM-dd"),
            IdTrabajador = documentoCaducar.IdTrabajador,
        };
    }

    // Constructor para mapear desde TrabajadorDTO
    public DocumentoCaducar ToEntity()
    {
        return new DocumentoCaducar
        {
            Nombre = this.Nombre,
            IdTrabajador = this.IdTrabajador,
            FechaExpiracion = string.IsNullOrWhiteSpace(this.FechaExpiracion) ? null: DateTime.ParseExact(this.FechaExpiracion, "yyyy-MM-dd", null),
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null),
        
        };
    }
}
