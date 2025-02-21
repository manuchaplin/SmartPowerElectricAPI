using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SmartPowerElectricAPI.Migrations;

namespace SmartPowerElectricAPI.DTO;

public partial class OrdenDTO
{
  
    public int? Id { get; set; }  
    public int NumeroOrden { get; set; }
    public bool? OrdenFinalizada {  get; set; }
    public double? CosteManoObra { get; set; }
    public double? Cobrado {  get; set; }
    public double? Ganancia {  get; set; }
    public double? HorasEstimadas {  get; set; }  
    public int? IdProyecto { get; set; }
    public string? NombreProyecto { get; set; }
    public List<MaterialDTO>? materialDTOs { get; set; }
    public List<TrabajadorDTO>? trabajadorDTOs { get; set; }
    public List<FacturaDTO>? facturaDTOs { get; set; }
    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }
 
    public double? CosteMateriales {  get; set; }
    public double? CosteTotal {  get; set; }
    public double? FaltanteCobrar {  get; set; }

    public static OrdenDTO FromEntity(Orden orden)
    {
        var costeMateriales = orden.Materials != null ? orden.Materials.Sum(x => x.Cantidad * x.Precio) : 0;
        var costeTotal = (orden.Ganancia ?? 0) + (orden.CosteManoObra ?? 0) + costeMateriales;
        var faltanteCobrar = costeTotal- (orden.Cobrado ?? 0);
        return new OrdenDTO
        {
            Id = orden.Id,
            NumeroOrden = orden.NumeroOrden,
            OrdenFinalizada = orden.OrdenFinalizada,
            Cobrado = orden.Cobrado,
            HorasEstimadas = orden.HorasEstimadas,
            IdProyecto = orden.IdProyecto,           
            NombreProyecto = orden.Proyecto!=null ? orden.Proyecto.Nombre:null,           
            FechaCreacion = orden.FechaCreacion?.ToString("yyyy-MM-dd"),
            FechaEliminado = orden.FechaEliminado?.ToString("yyyy-MM-dd"),
            Eliminado = orden.Eliminado,
            materialDTOs = orden.Materials != null ? orden.Materials.Select(MaterialDTO.FromEntity).ToList() : null,
            trabajadorDTOs = orden.Trabajadores != null ? orden.Trabajadores.Select(TrabajadorDTO.FromEntity).ToList() : null,
            facturaDTOs = orden.Facturas != null ? orden.Facturas.Select(FacturaDTO.FromEntity).ToList() : null,
            CosteManoObra = orden.CosteManoObra,
            Ganancia = orden.Ganancia,
            CosteMateriales= costeMateriales,
            CosteTotal = costeTotal,
            FaltanteCobrar = faltanteCobrar,
        };
    }

    // Constructor para mapear desde TrabajadorDTO
    public Orden ToEntity()
    {
        return new Orden
        {           
            OrdenFinalizada = this.OrdenFinalizada,
            CosteManoObra = this.CosteManoObra,
            Cobrado = this.Cobrado,
            NumeroOrden = this.NumeroOrden,
            HorasEstimadas = this.HorasEstimadas,
            IdProyecto = this.IdProyecto,
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null)

        };
    }
}

