using SmartPowerElectricAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartPowerElectricAPI.DTO;

public class NominaDTO
{
  
    public int? Id { get; set; }   
    public double? horasTrabajadas { get; set; }
    public double? SalarioEstandar { get; set; }
    public double? SalarioPlus { get; set; }
    public double? SalarioTotal { get; set; }
    public string? FechaPago { get; set; }  
    public string? SemanaCompleta { get; set; }
    public int? NoSemana { get; set; }  
    public int? Anyo { get; set; }  
    public string? InicioSemana { get; set; }  
    public string? FinSemana { get; set; }
    public string? FechaCreacion { get; set; }


    // Constructor para mapear desde Nomina
    public static NominaDTO FromEntity(Nomina nomina)
    {
        return new NominaDTO
        {
            Id = nomina.Id,
            horasTrabajadas = nomina.horasTrabajadas,
            SalarioEstandar = nomina.SalarioEstandar,
            SalarioPlus = nomina.SalarioPlus,
            SalarioTotal= nomina.SalarioEstandar+ nomina.SalarioPlus,
            FechaPago = nomina.FechaPago?.ToString("yyyy-MM-dd"),
            NoSemana = nomina.NoSemana,
            Anyo = nomina.Anyo,
            InicioSemana = nomina.InicioSemana?.ToString("yyyy-MM-dd"),
            FinSemana = nomina.FinSemana?.ToString("yyyy-MM-dd"),      
            FechaCreacion = nomina.FechaCreacion?.ToString("yyyy-MM-dd"),
            SemanaCompleta = nomina.InicioSemana?.ToString("yyyy-MM-dd")+"/"+ nomina.FinSemana?.ToString("yyyy-MM-dd")
        };
    }

    // Constructor para mapear desde NominaDTO
    public Nomina ToEntity()
    {
        var DivisionSemana = SemanaCompleta.Split('/');      
        this.InicioSemana = DivisionSemana[0];
        this.FinSemana = DivisionSemana[1];
        return new Nomina
        {
            horasTrabajadas = this.horasTrabajadas ?? 0,
            SalarioEstandar = this.SalarioEstandar ?? 0,
            SalarioPlus = this.SalarioPlus ?? 0,
            FechaPago = string.IsNullOrWhiteSpace(this.FechaPago) ? null : DateTime.ParseExact(this.FechaPago, "yyyy-MM-dd", null),
            NoSemana = this.NoSemana ?? 0,
            InicioSemana = string.IsNullOrWhiteSpace(this.InicioSemana) ? null : DateTime.ParseExact(this.InicioSemana, "yyyy-MM-dd", null),
            FinSemana = string.IsNullOrWhiteSpace(this.FinSemana) ? null: DateTime.ParseExact(this.FinSemana, "yyyy-MM-dd", null),
            FechaCreacion = string.IsNullOrWhiteSpace(this.FechaCreacion)? null: DateTime.ParseExact(this.FechaCreacion, "yyyy-MM-dd", null),           
        };
    }
}
