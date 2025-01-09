using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrabajadorController : ControllerBase
    {
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
    }
}
