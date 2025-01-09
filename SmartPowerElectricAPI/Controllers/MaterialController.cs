using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private IMaterialRepository _materialRepository;
        public MaterialController(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        //[HttpPost("create")]
        //public IActionResult Create([FromBody] Material material)
        //{
        //    try
        //    {
        //        //Cuando se incorpore proyecto al modelo hay q hacer la validacion de la creacion de material, para no agregar dos materiales iguales al mismo proyecto, con el mismo precio
        //        //List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
        //        //where.Add(x => x.Nombre.ToLower() == cliente.Nombre.ToLower());
        //        //where.Add(x => x.Email.ToLower() == cliente.Email.ToLower());
        //        //Material materialSearch = _clienteRepository.Get(where).FirstOrDefault();

        //        //if (clienteSearch == null)
        //        //{
        //        material.FechaCreacion = DateTime.Now;
        //            _clienteRepository.Insert(cliente);

        //            return Ok(cliente);
        //        //}
        //        //else
        //        //{
        //        //    return BadRequest("El cliente ya existe");
        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    try
        //    {
        //        List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
        //        where.Add(x => x.Id == id);
        //        where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
        //        Cliente cliente = _clienteRepository.Get(where).FirstOrDefault();
        //        if (cliente != null)
        //        {
        //            cliente.FechaEliminado = DateTime.Now;
        //            cliente.Eliminado = true;
        //            _clienteRepository.Update(cliente);

        //            return Ok("Eliminado correctamente");
        //        }
        //        else
        //        {
        //            return BadRequest("No existente");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        //[HttpPost("edit")]
        //public IActionResult Edit([FromBody] Cliente cliente)
        //{
        //    try
        //    {
        //        List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
        //        where.Add(x => x.Id == cliente.Id);
        //        Cliente clienteSearch = _clienteRepository.Get(where).FirstOrDefault();

        //        if (clienteSearch != null)
        //        {
        //            _clienteRepository.Update(cliente);

        //            return Ok(cliente);
        //        }
        //        else
        //        {
        //            return BadRequest("Cliente no encontrado");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        //[HttpGet("list")]
        //public IActionResult List()
        //{
        //    try
        //    {
        //        List<Cliente> clientes = new List<Cliente>();
        //        List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
        //        where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
        //        clientes = _clienteRepository.Get(where).ToList();

        //        return Ok(clientes);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
    }
}
