using System;
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
    public class ProyectoController : ControllerBase
    {
        private IProyectoRepository _proyectoRepository;
        private SmartPowerElectricContext _context;
        public ProyectoController(IProyectoRepository proyectoRepository, SmartPowerElectricContext context)
        {
            _proyectoRepository = proyectoRepository;
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ProyectoDTO proyectoDTO)
        {

            try
            {
                List<Expression<Func<Proyecto, bool>>> where = new List<Expression<Func<Proyecto, bool>>>();
                where.Add(x => x.Nombre.ToLower() == proyectoDTO.Nombre.ToLower());
                Proyecto proyectoSearch = _proyectoRepository.Get(where).FirstOrDefault();

                if (proyectoSearch == null)
                {
                    Proyecto proyecto = proyectoDTO.ToEntity();
                    proyecto.FechaCreacion=DateTime.Now;
                    proyecto.Finalizado=false;
                    _proyectoRepository.Insert(proyecto);

                    return Ok();
                }
                else
                {
                    return Conflict(new {message="El proyecto ya existe"});
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
                List<Expression<Func<Proyecto, bool>>> where = new List<Expression<Func<Proyecto, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Proyecto proyecto = _proyectoRepository.Get(where).FirstOrDefault();
                if (proyecto != null)
                {
                    proyecto.FechaEliminado=DateTime.Now;
                    proyecto.Eliminado = true;
                    _proyectoRepository.Update(proyecto);

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
        public IActionResult Edit(int id,[FromBody] ProyectoDTO proyectoDTO)
        {
            try
            {
                List<Expression<Func<Proyecto, bool>>> where = new List<Expression<Func<Proyecto, bool>>>();
                where.Add(x => x.Id == id);
                Proyecto proyectoSearch = _proyectoRepository.Get(where, "Ordens").FirstOrDefault();

                if (proyectoSearch != null)
                {
                    if (proyectoDTO.Nombre != null) proyectoSearch.Nombre = proyectoDTO.Nombre;
                    if (proyectoDTO.Direccion != null) proyectoSearch.Direccion = proyectoDTO.Direccion;
                    if (proyectoDTO.Descripcion != null) proyectoSearch.Descripcion = proyectoDTO.Descripcion;                    
                    if (proyectoDTO.horasEstimadas != null) proyectoSearch.horasEstimadas = proyectoDTO.horasEstimadas;                    
                    if (proyectoDTO.IdCliente != null) proyectoSearch.IdCliente = proyectoDTO.IdCliente;
                    if (proyectoDTO.Finalizado != null) 
                    {
                        if (proyectoSearch.Ordens.Any(x=>x.OrdenFinalizada!=true && x.Eliminado!=true))
                        {
                            return Conflict(new { message = "Hay ordenes sin finalizar" });
                        }
                        else
                        {
                            proyectoSearch.Finalizado=proyectoDTO.Finalizado;
                        }
                    }
                    if (proyectoDTO.FechaInicio != null) proyectoSearch.FechaInicio = string.IsNullOrWhiteSpace(proyectoDTO.FechaInicio) ? null : DateTime.ParseExact(proyectoDTO.FechaInicio, "yyyy-MM-dd", null);
                    if (proyectoDTO.FechaFin != null) proyectoSearch.FechaFin = string.IsNullOrWhiteSpace(proyectoDTO.FechaFin) ? null : DateTime.ParseExact(proyectoDTO.FechaFin, "yyyy-MM-dd", null);
                    if (proyectoDTO.FechaCreacion != null) proyectoSearch.FechaCreacion = string.IsNullOrWhiteSpace(proyectoDTO.FechaCreacion) ? null : DateTime.ParseExact(proyectoDTO.FechaCreacion, "yyyy-MM-dd", null);


                    _proyectoRepository.Update(proyectoSearch);

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
                List<Proyecto> proyectos = new List<Proyecto>();
                List<Expression<Func<Proyecto, bool>>> where = new List<Expression<Func<Proyecto, bool>>>();            
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                //proyectos = _proyectoRepository.Get(where, "Trabajadores,Materials").ToList();//Para saber los trabajadores,y materiales asociados al proyecto
                proyectos = _proyectoRepository.Get(where,"Cliente").ToList();

                List<ProyectoDTO> proyectoDTOs = proyectos.Select(ProyectoDTO.FromEntity).ToList();

                return Ok(proyectoDTOs);

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
               
               
                Proyecto proyecto = new Proyecto();
                //proyecto = _proyectoRepository.GetByID(id, "Cliente,Ordens");
                proyecto = _context.Proyecto.Where(x => x.Id == id).Include(x => x.Cliente).Include(x => x.Ordens).ThenInclude(x => x.Materials).FirstOrDefault();
                
                proyecto.Ordens=proyecto.Ordens.Where(x=>x.Eliminado!=true && x.FechaEliminado == null).ToList();
                proyecto.Ordens.ToList().ForEach(orden => orden.Materials.ToList().RemoveAll(material => material.Eliminado==true));

                if (proyecto == null) return NotFound();

                ProyectoDTO proyectoDTO = ProyectoDTO.FromEntity(proyecto);


                return Ok(proyectoDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }

    }
}
