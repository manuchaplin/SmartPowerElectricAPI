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
    public class ClienteController : ControllerBase
    {
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ClienteDTO clienteDTO)
        {

            try
            {
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
                where.Add(x => x.Nombre.ToLower() == clienteDTO.Nombre.ToLower());
                where.Add(x => x.Email.ToLower() == clienteDTO.Email.ToLower());                
                Cliente clienteSearch = _clienteRepository.Get(where).FirstOrDefault();

                if (clienteSearch == null)
                {
                    Cliente cliente= clienteDTO.ToEntity();
                    cliente.FechaCreacion=DateTime.Now;
                    _clienteRepository.Insert(cliente);

                    return Ok();
                }
                else
                {
                    return Conflict(new {message="El cliente ya existe"});
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
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Cliente cliente = _clienteRepository.Get(where).FirstOrDefault();
                if (cliente != null)
                {
                    cliente.FechaEliminado=DateTime.Now;
                    cliente.Eliminado = true;
                    _clienteRepository.Update(cliente);

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
        public IActionResult Edit(int id,[FromBody] ClienteDTO clienteDTO)
        {
            try
            {
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();
                where.Add(x => x.Id == id);
                Cliente clienteSearch = _clienteRepository.Get(where).FirstOrDefault();

                if (clienteSearch != null)
                {
                    if (clienteDTO.Nombre != null) clienteSearch.Nombre = clienteDTO.Nombre;
                    if (clienteDTO.Direccion != null) clienteSearch.Direccion = clienteDTO.Direccion;
                    if (clienteDTO.Email != null) clienteSearch.Email = clienteDTO.Email;
                    if (clienteDTO.Telefono != null) clienteSearch.Telefono = clienteDTO.Telefono;
                    if (clienteDTO.FechaCreacion != null) clienteSearch.FechaCreacion = string.IsNullOrWhiteSpace(clienteDTO.FechaCreacion) ? null : DateTime.ParseExact(clienteDTO.FechaCreacion, "MM-dd-yyyy", null);


                    _clienteRepository.Update(clienteSearch);

                    return Ok(clienteSearch);
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
                List<Cliente> clientes = new List<Cliente>();
                List<Expression<Func<Cliente, bool>>> where = new List<Expression<Func<Cliente, bool>>>();            
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                clientes = _clienteRepository.Get(where).ToList();

                List<ClienteDTO> clienteDTOs = clientes.Select(ClienteDTO.FromEntity).ToList();

                return Ok(clienteDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

    }
}
