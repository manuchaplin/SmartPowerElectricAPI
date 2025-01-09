using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult Create([FromBody] Trabajador trabajador)
        {

            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Nombre.ToLower() == trabajador.Nombre.ToLower());
                where.Add(x => x.Apellido.ToLower() == trabajador.Apellido.ToLower());
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch == null)
                {
                    trabajador.FechaCreacion = DateTime.Now;
                    _trabajadorRepository.Insert(trabajador);

                    return Ok(trabajador);
                }
                else
                {
                    return BadRequest("El trabajador ya existe");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

                    return Ok("Eliminado correctamente");
                }
                else
                {
                    return BadRequest("No existente");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("edit")]
        public IActionResult Edit([FromBody] Trabajador trabajador)
        {
            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Id == trabajador.Id);
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch != null)
                {
                    _trabajadorRepository.Update(trabajador);

                    return Ok(trabajador);
                }
                else
                {
                    return BadRequest("Trabajador no encontrado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

                return Ok(trabajadores);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
