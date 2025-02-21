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
    public class TrabajadorController : ControllerBase
    {
        #region async Test
        //Funciones async Test
        //private readonly SmartPowerElectricContext _context;

        //public TrabajadorController(SmartPowerElectricContext context)
        //{
        //    _context = context;
        //}

        //// GET: api/Trabajador
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Trabajador>>> GetTrabajadors()
        //{
        //  if (_context.Trabajadors == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Trabajadors.ToListAsync();
        //}

        //// GET: api/Trabajador/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Trabajador>> GetTrabajador(int id)
        //{
        //  if (_context.Trabajadors == null)
        //  {
        //      return NotFound();
        //  }
        //    var trabajador = await _context.Trabajadors.FindAsync(id);

        //    if (trabajador == null)
        //    {
        //        return NotFound();
        //    }

        //    return trabajador;
        //}

        //// PUT: api/Trabajador/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTrabajador(int id, Trabajador trabajador)
        //{
        //    if (id != trabajador.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(trabajador).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TrabajadorExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Trabajador
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Trabajador>> PostTrabajador(Trabajador trabajador)
        //{
        //  if (_context.Trabajadors == null)
        //  {
        //      return Problem("Entity set 'SmartPowerElectricContext.Trabajadors'  is null.");
        //  }
        //    _context.Trabajadors.Add(trabajador);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTrabajador", new { id = trabajador.Id }, trabajador);
        //}

        //// DELETE: api/Trabajador/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTrabajador(int id)
        //{
        //    if (_context.Trabajadors == null)
        //    {
        //        return NotFound();
        //    }
        //    var trabajador = await _context.Trabajadors.FindAsync(id);
        //    if (trabajador == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Trabajadors.Remove(trabajador);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool TrabajadorExists(int id)
        //{
        //    return (_context.Trabajadors?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
        #endregion

        private readonly ITrabajadorRepository _trabajadorRepository;
        public TrabajadorController(ITrabajadorRepository trabajadorRepository) {
            _trabajadorRepository = trabajadorRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] TrabajadorDTO trabajadorDTO)
        {

            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Nombre.ToLower() == trabajadorDTO.Nombre.ToLower());
                where.Add(x => x.Apellido.ToLower() == trabajadorDTO.Apellido.ToLower());                
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch == null)
                {
                    Trabajador trabajador= trabajadorDTO.ToEntity();
                    trabajador.FechaCreacion = DateTime.Now;
                    _trabajadorRepository.Insert(trabajador);

                    return Ok();
                }
                else
                {
                    return Conflict(new { message= "El trabajador ya existe" });
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
        public IActionResult Edit(int id,[FromBody] TrabajadorDTO trabajadorDTO)
        {
            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Id == id);
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch != null)
                {
                    if (trabajadorDTO.Nombre != null) trabajadorSearch.Nombre = trabajadorDTO.Nombre;
                    if (trabajadorDTO.Apellido != null) trabajadorSearch.Apellido = trabajadorDTO.Apellido;
                    if (trabajadorDTO.Especialidad != null) trabajadorSearch.Especialidad = trabajadorDTO.Especialidad;
                    if (trabajadorDTO.Email != null) trabajadorSearch.Email = trabajadorDTO.Email;
                    if (trabajadorDTO.Telefono != null) trabajadorSearch.Telefono = trabajadorDTO.Telefono;
                    if (trabajadorDTO.Direccion != null) trabajadorSearch.Direccion = trabajadorDTO.Direccion;
                    if (trabajadorDTO.SeguridadSocial != null) trabajadorSearch.SeguridadSocial = trabajadorDTO.SeguridadSocial;
                    if (trabajadorDTO.FechaInicioContrato != null) trabajadorSearch.FechaInicioContrato = string.IsNullOrWhiteSpace(trabajadorDTO.FechaInicioContrato) ? null : DateTime.ParseExact(trabajadorDTO.FechaInicioContrato, "yyyy-MM-dd", null);
                    if (trabajadorDTO.FechaFinContrato != null) trabajadorSearch.FechaFinContrato = string.IsNullOrWhiteSpace(trabajadorDTO.FechaFinContrato) ? null : DateTime.ParseExact(trabajadorDTO.FechaFinContrato, "yyyy-MM-dd", null);
                    if (trabajadorDTO.CobroxHora != null) trabajadorSearch.CobroxHora = trabajadorDTO.CobroxHora;
                    if (trabajadorDTO.NumeroCuenta != null) trabajadorSearch.NumeroCuenta = trabajadorDTO.NumeroCuenta;
                    if (trabajadorDTO.Enrutamiento != null) trabajadorSearch.Enrutamiento = trabajadorDTO.Enrutamiento;
                    if (trabajadorDTO.FechaCreacion != null) trabajadorSearch.FechaCreacion  = string.IsNullOrWhiteSpace(trabajadorDTO.FechaCreacion) ? null : DateTime.ParseExact(trabajadorDTO.FechaCreacion, "yyyy-MM-dd", null);

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

                List<TrabajadorDTO> trabajadorDTOs=trabajadores.Select(TrabajadorDTO.FromEntity).ToList();

                return Ok(trabajadorDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
      
    }
}
