using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class UsuarioDTO
{

    public int? Id { get; set; }
    public string? Nombre { get; set; } = null!;
   
    public string? Apellido { get; set; } = null!;
  
    public string? Email { get; set; } = null!;
   
    public string? Username { get; set; } = null!;
   
    public string? Password { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
    public bool? Protegido { get; set; }


    // Constructor para mapear desde Usuario
    public static UsuarioDTO FromEntity(Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Username = usuario.Username,
            Password = usuario.Password,
            Telefono = usuario.Telefono,
            FechaCreacion = usuario.FechaCreacion?.ToString("yyyy-MM-dd"),
        };
    }

    // Constructor para mapear desde UsuarioDTO
    public Usuario ToEntity()
    {
        return new Usuario
        {
            Nombre = this.Nombre,
            Apellido = this.Apellido,
            Email = this.Email,
            Username = this.Username,
            Password=this.Password,
            Telefono = this.Telefono,
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null),
        };
    }
}
