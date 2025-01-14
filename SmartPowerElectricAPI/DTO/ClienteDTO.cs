using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class ClienteDTO
{
    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }


    // Constructor para mapear desde Cliente
    public static ClienteDTO FromEntity(Cliente cliente)
    {
        return new ClienteDTO
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Direccion = cliente.Direccion,            
            Email = cliente.Email,
            Telefono = cliente.Telefono,                             
            FechaCreacion = cliente.FechaCreacion?.ToString("MM-dd-yyyy"),
        };
    }

    // Constructor para mapear desde ClienteDTO
    public Cliente ToEntity()
    {
        return new Cliente
        {
            Nombre = this.Nombre,
            Direccion = this.Direccion,
            Email = this.Email,            
            Telefono = this.Telefono,
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null: DateTime.ParseExact(this.FechaCreacion, "MM-dd-yyyy", null),          
        };
    }
}
