using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NominaController : ControllerBase
    {
       

        private readonly INominaRepository _nominaRepository;
        private readonly ITrabajadorRepository _trabajadorRepository;
        public NominaController(INominaRepository nominaRepository,ITrabajadorRepository trabajadorRepository)
        {
            _nominaRepository = nominaRepository;
            _trabajadorRepository = trabajadorRepository;
        }

        [HttpPost("create/{idTrabajador}")]
        public IActionResult Create(int idTrabajador,[FromBody] NominaDTO nominaDTO)
        {

            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Id == idTrabajador);           
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch != null)
                {
                    Nomina nomina= nominaDTO.ToEntity();
                    nomina.IdTrabajador = idTrabajador;
                    nomina.SalarioEstandar = nomina.horasTrabajadas*trabajadorSearch.CobroxHora;
                    nomina.FechaCreacion = DateTime.Now;
                    _nominaRepository.Insert(nomina);

                    return Ok();
                }
                else
                {
                    return Conflict(new { message= "El trabajador no existe" });
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                List<Expression<Func<Nomina, bool>>> where = new List<Expression<Func<Nomina, bool>>>();
                where.Add(x => x.Id == id);
                Nomina nomina = _nominaRepository.Get(where).FirstOrDefault();
                if (nomina != null)
                {
                    _nominaRepository.Delete(nomina);

                    return Ok();
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id,[FromBody] NominaDTO nominaDTO)
        {
            try
            {
                List<Expression<Func<Nomina, bool>>> where = new List<Expression<Func<Nomina, bool>>>();
                where.Add(x => x.Id == id);
                Nomina nominaSearch = _nominaRepository.Get(where,"Trabajador").FirstOrDefault();

                if (nominaSearch != null)
                {
                    if (nominaDTO.horasTrabajadas != null) nominaSearch.horasTrabajadas = nominaDTO.horasTrabajadas ?? 0;
                    if (nominaDTO.horasTrabajadas != null) nominaSearch.SalarioEstandar = nominaDTO.horasTrabajadas* nominaSearch.Trabajador.CobroxHora;
                    if (nominaDTO.SalarioPlus != null) nominaSearch.SalarioPlus = nominaDTO.SalarioPlus;
                    if (nominaDTO.FechaPago != null) nominaSearch.FechaPago = string.IsNullOrWhiteSpace(nominaDTO.FechaPago) ? null : DateTime.ParseExact(nominaDTO.FechaPago, "yyyy-MM-dd", null);
                    if (nominaDTO.SemanaCompleta != null) {
                        var DivisionSemana = nominaDTO.SemanaCompleta.Split('/');
                        nominaSearch.NoSemana = int.Parse(DivisionSemana[0]);
                        nominaSearch.InicioSemana = DateTime.ParseExact(DivisionSemana[1], "yyyy-MM-dd", null); 
                        nominaSearch.FinSemana = DateTime.ParseExact(DivisionSemana[2], "yyyy-MM-dd", null);
                    }               
                    if (nominaDTO.FechaCreacion != null) nominaSearch.FechaCreacion  = string.IsNullOrWhiteSpace(nominaDTO.FechaCreacion) ? null : DateTime.ParseExact(nominaDTO.FechaCreacion, "yyyy-MM-dd", null);

                    _nominaRepository.Update(nominaSearch);

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("list/{idTrabajador}")]
        public IActionResult List(int idTrabajador)
        {
            try
            {
                Trabajador trabajador = new Trabajador();
               
                trabajador = _trabajadorRepository.GetByID(idTrabajador, "Nominas");
                if (trabajador!=null)
                {
                    //List<Nomina> nominas = new List<Nomina>();
                    //nominas=trabajador.Nominas.ToList();
                    List<NominaDTO> nominaDTOs = new List<NominaDTO>();
                    nominaDTOs= trabajador.Nominas.Select(NominaDTO.FromEntity).ToList();
                    return Ok(nominaDTOs);
                }
                else
                {
                    return NotFound();
                }
                             
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("ListWeeksOfYear/{year}")]
        public IActionResult ListWeekOfYear(int year)
        {
            List<(int Semana, DateTime FechaInicio, DateTime FechaFin)> semanas = new List<(int, DateTime, DateTime)>();
            DateTime primerDia = new DateTime(year, 1, 1);
            DateTime ultimoDia = new DateTime(year, 12, 31);
            CultureInfo cultura = CultureInfo.CurrentCulture;

            // Obtener la primera semana del año según ISO 8601
            int semanaActual = cultura.Calendar.GetWeekOfYear(primerDia, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Encontrar el primer lunes de la semana 1
            DateTime inicioSemana = primerDia.AddDays(-(int)primerDia.DayOfWeek + (int)DayOfWeek.Monday);
            if (inicioSemana.Year > year)
            {
                inicioSemana = inicioSemana.AddDays(-7); // Ajustar si el primer lunes pertenece al año siguiente
            }
            else if (inicioSemana.Year < year && cultura.Calendar.GetWeekOfYear(ultimoDia.AddDays(-3), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == 1)
            {
                inicioSemana = inicioSemana.AddDays(7); // Ajustar si la última semana del año anterior se considera la primera del año actual
            }

            // Iterar sobre las semanas
            while (inicioSemana.Year <= year)
            {
                DateTime finSemana = inicioSemana.AddDays(6);
                if (finSemana.Year > year) finSemana = ultimoDia; // Ajuste para la última semana del año

                semanas.Add((semanaActual, inicioSemana, finSemana));

                inicioSemana = inicioSemana.AddDays(7);
                semanaActual = cultura.Calendar.GetWeekOfYear(inicioSemana, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }

            List<string> semanasString = new List<string>();

            foreach (var semana in semanas)
            {
                semanasString.Add(semana.Semana.ToString() + "/" + semana.FechaInicio.ToString("yyyy-MM-dd") + "/" + semana.FechaFin.ToString("yyyy-MM-dd"));
            }

            return Ok(semanasString);
        }

    }
}
