using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartPowerElectricAPI.DTO;

public partial class FacturaDTO
{

    public int? Id { get; set; }
    public double? MontoACobrar { get; set; }
    public string? NumeroFactura { get; set; }
    public bool? EmailEnviado { get; set; }
    public bool? FacturaCompletada { get; set; }
    public int? IdOrden { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaEliminado { get; set; }

    public bool? Eliminado { get; set; }


    public static FacturaDTO FromEntity(Factura factura)
    {
        return new FacturaDTO
        {
            Id = factura.Id,
            MontoACobrar = factura.MontoACobrar,
            NumeroFactura = factura.NumeroFactura,
            EmailEnviado = factura.EmailEnviado,
            FacturaCompletada = factura.FacturaCompletada,         
            IdOrden = factura.IdOrden,           
            FechaCreacion = factura.FechaCreacion?.ToString("yyyy-MM-dd")
        };
    }

    // Constructor para mapear desde TrabajadorDTO
    public Factura ToEntity()
    {
        return new Factura
        {
            MontoACobrar = this.MontoACobrar ?? 0,
            NumeroFactura = this.NumeroFactura,
            EmailEnviado = this.EmailEnviado,
            FacturaCompletada = this.FacturaCompletada,                           
            IdOrden = this.IdOrden,                   
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion) ? null : DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null)
           
        };
    }

}
