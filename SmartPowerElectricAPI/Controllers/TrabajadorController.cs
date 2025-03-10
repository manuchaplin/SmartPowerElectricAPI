﻿using System;
using System.Collections.Generic;
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
    public class TrabajadorController : ControllerBase
    {
        private readonly ITrabajadorRepository _trabajadorRepository;
        private readonly IProyectoRepository _proyectoRepository;
        public TrabajadorController(ITrabajadorRepository trabajadorRepository,IProyectoRepository proyectoRepository)
        {
            _trabajadorRepository = trabajadorRepository;
            _proyectoRepository = proyectoRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] TrabajadorDTO trabajadorDTO)
        {

            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Nombre.ToLower() == trabajadorDTO.Nombre.ToLower());
                where.Add(x => x.Apellido.ToLower() == trabajadorDTO.Apellido.ToLower());
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch == null)
                {
                    Trabajador trabajador = trabajadorDTO.ToEntity();
                    trabajador.FechaCreacion = DateTime.Now;

                    List<string> MessageError = Validate(trabajador);
                    if (MessageError.Count() > 0)
                    {
                        return Conflict(new { message = MessageError });
                    }

                    _trabajadorRepository.Insert(trabajador);

                    return Ok();
                }
                else
                {
                    return Conflict(new { message = "El trabajador ya existe" });
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
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Trabajador trabajador = _trabajadorRepository.Get(where).FirstOrDefault();
                if (trabajador != null)
                {
                    trabajador.FechaEliminado = DateTime.Now;
                    trabajador.Eliminado = true;
                    _trabajadorRepository.Update(trabajador);

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
        public IActionResult Edit(int id, [FromBody] TrabajadorDTO trabajadorDTO)
        {
            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Id == id);
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch != null)
                {
                    if (trabajadorDTO.Nombre != null && trabajadorDTO.Nombre != "") trabajadorSearch.Nombre = trabajadorDTO.Nombre;
                    if (trabajadorDTO.Apellido != null && trabajadorDTO.Apellido != "") trabajadorSearch.Apellido = trabajadorDTO.Apellido;
                    if (trabajadorDTO.Especialidad != null && trabajadorDTO.Especialidad != "") trabajadorSearch.Especialidad = trabajadorDTO.Especialidad;
                    if (trabajadorDTO.Email != null && trabajadorDTO.Email != "") trabajadorSearch.Email = trabajadorDTO.Email;
                    if (trabajadorDTO.Telefono != null) trabajadorSearch.Telefono = trabajadorDTO.Telefono;
                    if (trabajadorDTO.Direccion != null) trabajadorSearch.Direccion = trabajadorDTO.Direccion;
                    if (trabajadorDTO.SeguridadSocial != null && trabajadorDTO.SeguridadSocial != "") trabajadorSearch.SeguridadSocial = trabajadorDTO.SeguridadSocial;
                    if (trabajadorDTO.FechaInicioContrato != null) trabajadorSearch.FechaInicioContrato = string.IsNullOrWhiteSpace(trabajadorDTO.FechaInicioContrato) ? null : DateTime.ParseExact(trabajadorDTO.FechaInicioContrato, "yyyy-MM-dd", null);
                    if (trabajadorDTO.FechaFinContrato != null) trabajadorSearch.FechaFinContrato = string.IsNullOrWhiteSpace(trabajadorDTO.FechaFinContrato) ? null : DateTime.ParseExact(trabajadorDTO.FechaFinContrato, "yyyy-MM-dd", null);
                    if (trabajadorDTO.CobroxHora != null) trabajadorSearch.CobroxHora = trabajadorDTO.CobroxHora;
                    if (trabajadorDTO.NumeroCuenta != null && trabajadorDTO.NumeroCuenta != "") trabajadorSearch.NumeroCuenta = trabajadorDTO.NumeroCuenta;
                    if (trabajadorDTO.Enrutamiento != null && trabajadorDTO.Enrutamiento != "") trabajadorSearch.Enrutamiento = trabajadorDTO.Enrutamiento;
                    if (trabajadorDTO.FechaCreacion != null) trabajadorSearch.FechaCreacion = string.IsNullOrWhiteSpace(trabajadorDTO.FechaCreacion) ? null : DateTime.ParseExact(trabajadorDTO.FechaCreacion, "yyyy-MM-dd", null);

                    _trabajadorRepository.Update(trabajadorSearch);

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

        [HttpGet("list")]
        public IActionResult List()
        {
            try
            {
                List<Trabajador> trabajadores = new List<Trabajador>();
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                trabajadores = _trabajadorRepository.Get(where).ToList();

                List<TrabajadorDTO> trabajadorDTOs = trabajadores.Select(TrabajadorDTO.FromEntity).ToList();

                return Ok(trabajadorDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
               
                Trabajador trabajador = new Trabajador();
                trabajador = _trabajadorRepository.GetByID(id);

                if (trabajador == null)
                {
                    return NotFound();
                }
                ProyectosTrabajadorDTO proyectosTrabajadorDTO = new ProyectosTrabajadorDTO();
                proyectosTrabajadorDTO.proyectosActivos = new List<string>();
                List<Proyecto> proyectos=new List<Proyecto>();
                List<Expression<Func<Proyecto, bool>>> where = new List<Expression<Func<Proyecto, bool>>>();
                where.Add(x =>x.Finalizado!=true && x.Eliminado!=true && x.Ordens.Any(y => y.Trabajadores.Any(t => t.Id == trabajador.Id)));
                proyectos= _proyectoRepository.Get(where).ToList();

                proyectosTrabajadorDTO.trabajadorDTO = TrabajadorDTO.FromEntity(trabajador);
                //proyectos.ForEach(x => proyectosTrabajadorDTO.proyectosActivos.Add(x.Nombre));
                proyectosTrabajadorDTO.proyectosActivos.AddRange(proyectos.Select(x => x.Nombre));

                return Ok(proyectosTrabajadorDTO);
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<string> Validate(Trabajador trabajador)
        {
            List<string> Message = new List<string>();
            if (string.IsNullOrEmpty(trabajador.SeguridadSocial))
            {
                Message.Add("La Seguridad Social del trabajador no puede ser vacio.");
            }
            if (string.IsNullOrEmpty(trabajador.Especialidad))
            {
                Message.Add("La Especialidad del trabajador no puede ser vacio.");
            }
            if (string.IsNullOrEmpty(trabajador.Email))
            {
                Message.Add("La Email del trabajador no puede ser vacio.");
            }
            if (string.IsNullOrEmpty(trabajador.NumeroCuenta))
            {
                Message.Add("La NumeroCuenta del trabajador no puede ser vacio.");
            }
            if (string.IsNullOrEmpty(trabajador.Enrutamiento))
            {
                Message.Add("La Enrutamiento del trabajador no puede ser vacio.");
            }

            return Message;
        }

        public class ProyectosTrabajadorDTO
        {
            public TrabajadorDTO trabajadorDTO { get; set; }
            public List<string> proyectosActivos { get; set; }

        }
    }
}
