using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class TrabajadorDTO
{
    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;

    public string? Apellido { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? SeguridadSocial { get; set; }

    public string? FechaInicioContrato { get; set; }

    public string? FechaFinContrato { get; set; }

    public double? CobroxHora { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }


    // Constructor para mapear desde Trabajador
    public static TrabajadorDTO FromEntity(Trabajador trabajador)
    {
        return new TrabajadorDTO
        {
            Id = trabajador.Id,
            Nombre = trabajador.Nombre,
            Apellido = trabajador.Apellido,
            Especialidad = trabajador.Especialidad,
            Email = trabajador.Email,
            Telefono = trabajador.Telefono,
            Direccion = trabajador.Direccion,
            SeguridadSocial = trabajador.SeguridadSocial,
            FechaInicioContrato = trabajador.FechaInicioContrato?.ToString("MM-dd-yyyy"),
            FechaFinContrato = trabajador.FechaFinContrato?.ToString("MM-dd-yyyy"),
            CobroxHora = trabajador.CobroxHora,
            FechaCreacion = trabajador.FechaCreacion?.ToString("MM-dd-yyyy"),
        };
    }

    // Constructor para mapear desde TrabajadorDTO
    public Trabajador ToEntity()
    {
        return new Trabajador
        {           
            Nombre = this.Nombre,
            Apellido = this.Apellido,
            Especialidad = this.Especialidad,
            Email = this.Email,
            Telefono = this.Telefono,
            Direccion = this.Direccion,
            SeguridadSocial = this.SeguridadSocial,
            FechaInicioContrato = string.IsNullOrWhiteSpace(this.FechaInicioContrato) ? null : DateTime.ParseExact(this.FechaInicioContrato, "MM-dd-yyyy", null),
            FechaFinContrato = string.IsNullOrWhiteSpace(this.FechaFinContrato) ? null: DateTime.ParseExact(this.FechaFinContrato, "MM-dd-yyyy", null),
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion)? null: DateTime.ParseExact(this.FechaCreacion, "MM-dd-yyyy", null),
            CobroxHora = this.CobroxHora
        };
    }
}
