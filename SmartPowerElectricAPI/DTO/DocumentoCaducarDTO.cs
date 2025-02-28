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
    public string? FechaExpedicion { get; set; }
    public int? IdTrabajador { get; set; }
    public string? Trabajador { get; set; }
    public bool? Expirado { get; set; }  


    public static DocumentoCaducarDTO FromEntity(DocumentoCaducar documentoCaducar)
    {
        return new DocumentoCaducarDTO
        {
            Id = documentoCaducar.Id,
            Nombre = documentoCaducar.Nombre,       
            FechaExpiracion = documentoCaducar.FechaExpiracion?.ToString("yyyy-MM-dd"),
            FechaExpedicion = documentoCaducar.FechaExpedicion?.ToString("yyyy-MM-dd"),
            IdTrabajador = documentoCaducar.IdTrabajador,
            Trabajador= documentoCaducar.Trabajador!=null ? documentoCaducar.Trabajador.Nombre+" "+ documentoCaducar.Trabajador.Apellido : null,
            Expirado=  documentoCaducar.FechaExpiracion.Value.Date < DateTime.Now.Date ? true :false
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
            FechaExpedicion = string.IsNullOrWhiteSpace(this.FechaExpedicion) ? null : DateTime.ParseExact(this.FechaExpedicion, "yyyy-MM-dd", null),
        
        };
    }
}
