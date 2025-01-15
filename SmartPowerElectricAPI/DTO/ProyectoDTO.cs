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
            FechaInicio = proyecto.FechaInicio?.ToString("MM-dd-yyyy"),
            FechaFin = proyecto.FechaFin?.ToString("MM-dd-yyyy"),
            FechaCreacion = proyecto.FechaCreacion?.ToString("MM-dd-yyyy"),
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
            FechaInicio = string.IsNullOrWhiteSpace(this.FechaInicio) ? null : DateTime.ParseExact(this.FechaInicio, "MM-dd-yyyy", null),
            FechaFin = string.IsNullOrWhiteSpace(this.FechaFin) ? null : DateTime.ParseExact(this.FechaFin, "MM-dd-yyyy", null),
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "MM-dd-yyyy", null)                     
        };
    }
}
