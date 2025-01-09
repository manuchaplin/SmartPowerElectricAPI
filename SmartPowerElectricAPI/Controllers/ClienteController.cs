using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] Cliente cliente)
        {

            try
            {
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
                where.Add(x => x.Nombre.ToLower() == cliente.Nombre.ToLower());
                where.Add(x => x.Email.ToLower() == cliente.Email.ToLower());                
                Cliente clienteSearch = _clienteRepository.Get(where).FirstOrDefault();

                if (clienteSearch == null)
                {
                    cliente.FechaCreacion=DateTime.Now;
                    _clienteRepository.Insert(cliente);

                    return Ok(cliente);
                }
                else
                {
                    return BadRequest("El cliente ya existe");
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
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Cliente cliente = _clienteRepository.Get(where).FirstOrDefault();
                if (cliente != null)
                {
                    cliente.FechaEliminado=DateTime.Now;
                    cliente.Eliminado = true;
                    _clienteRepository.Update(cliente);

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
        public IActionResult Edit([FromBody] Cliente cliente)
        {
            try
            {
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
                where.Add(x => x.Id == cliente.Id);
                Cliente clienteSearch = _clienteRepository.Get(where).FirstOrDefault();

                if (clienteSearch != null)
                {
                    _clienteRepository.Update(cliente);

                    return Ok(cliente);
                }
                else
                {
                    return BadRequest("Cliente no encontrado");
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
                List<Cliente> clientes = new List<Cliente>();
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();            
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                clientes = _clienteRepository.Get(where).ToList();

                return Ok(clientes);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
