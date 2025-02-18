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
    public string? NumeroCuenta { get; set; }
    public string? Enrutamiento { get; set; }

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
            FechaInicioContrato = trabajador.FechaInicioContrato?.ToString("yyyy-MM-dd"),
            FechaFinContrato = trabajador.FechaFinContrato?.ToString("yyyy-MM-dd"),
            CobroxHora = trabajador.CobroxHora,
            NumeroCuenta = trabajador.NumeroCuenta,
            Enrutamiento = trabajador.Enrutamiento,
            FechaCreacion = trabajador.FechaCreacion?.ToString("yyyy-MM-dd"),
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
            FechaInicioContrato = string.IsNullOrWhiteSpace(this.FechaInicioContrato) ? null : DateTime.ParseExact(this.FechaInicioContrato, "yyyy-MM-dd", null),
            FechaFinContrato = string.IsNullOrWhiteSpace(this.FechaFinContrato) ? null: DateTime.ParseExact(this.FechaFinContrato, "yyyy-MM-dd", null),
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion)? null: DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null),
            CobroxHora = this.CobroxHora,
            NumeroCuenta = this.NumeroCuenta,
            Enrutamiento = this.Enrutamiento
        };
    }
}
