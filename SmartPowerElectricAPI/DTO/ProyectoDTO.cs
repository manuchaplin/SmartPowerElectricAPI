using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.DTO;

public class ProyectoDTO
{    
    public int? Id { get; set; }
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public string? Descripcion { get; set; }
    public string? FechaInicio { get; set; }
    public string? FechaFin { get; set; }    
    public int? IdCliente { get; set; }
    public string? NombreCLiente { get; set; }
    public double? horasEstimadas { get; set; }
    public string? FechaCreacion { get; set; }
    public string? FechaEliminado { get; set; }
    public bool? Eliminado { get; set; }
    public List<MaterialDTO>? materialDTOs { get; set; }
    public List<TrabajadorDTO>? trabajadorDTOs { get; set; }


    // Constructor para mapear desde Proyecto
    public static ProyectoDTO FromEntity(Proyecto proyecto)
    {
        return new ProyectoDTO
        {
            Id = proyecto.Id,
            Nombre = proyecto.Nombre,
            Direccion = proyecto.Direccion,
            Descripcion = proyecto.Descripcion,
            horasEstimadas = proyecto.horasEstimadas,
            IdCliente = proyecto.IdCliente,
            NombreCLiente = proyecto.Cliente!=null ? proyecto.Cliente.Nombre : null,
            FechaInicio = proyecto.FechaInicio?.ToString("yyyy-MM-dd"),
            FechaFin = proyecto.FechaFin?.ToString("yyyy-MM-dd"),
            FechaCreacion = proyecto.FechaCreacion?.ToString("yyyy-MM-dd"),
            materialDTOs = proyecto.Materials != null ? proyecto.Materials.Select(MaterialDTO.FromEntity).ToList() :null,
            trabajadorDTOs = proyecto.Trabajadores != null ? proyecto.Trabajadores.Select(TrabajadorDTO.FromEntity).ToList() : null
        };
    }

    // Constructor para mapear desde ClienteDTO
    public Proyecto ToEntity()
    {
        return new Proyecto
        {            
            Nombre = this.Nombre,
            Direccion = this.Direccion,
            Descripcion = this.Descripcion,
            horasEstimadas = this.horasEstimadas,
            IdCliente = this.IdCliente,
            FechaInicio = string.IsNullOrWhiteSpace(this.FechaInicio) ? null : DateTime.ParseExact(this.FechaInicio, "yyyy-MM-dd", null),
            FechaFin = string.IsNullOrWhiteSpace(this.FechaFin) ? null : DateTime.ParseExact(this.FechaFin, "yyyy-MM-dd", null),
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null)                     
        };
    }
}
