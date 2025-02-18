using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using SmartPowerElectricAPI.Utilities;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnidadMedidumController : ControllerBase
    {
        private readonly IUnidadMedidumRepository _unidadMedidumRepository;
        private readonly IConfiguration _configuration;   
        public UnidadMedidumController (IUnidadMedidumRepository unidadMedidumRepository, IConfiguration configuration)
        {
            _unidadMedidumRepository = unidadMedidumRepository;
            _configuration = configuration;           
        }
        [HttpPost("create")]       
        public IActionResult Create([FromBody] UnidadMedidaDTO unidadMedidumDTO)
        {
          
            try
            {                
                List<Expression<Func<UnidadMedida, bool>>> where = new List<Expression<Func<UnidadMedida, bool>>>();
                where.Add(x => x.UMedida.ToLower() == unidadMedidumDTO.UMedida.ToLower());
                UnidadMedida unidadMedSearch = _unidadMedidumRepository.Get(where).FirstOrDefault();

                if (unidadMedSearch == null)
                {
                    UnidadMedida unidadMedida=unidadMedidumDTO.ToEntity();
                    unidadMedida.FechaCreacion = DateTime.Now;
                    _unidadMedidumRepository.Insert(unidadMedida);

                    return Ok();
                }
                else
                {
                    return Conflict(new { message = "La unidad de medida ya existe" });
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
                List<Expression<Func<UnidadMedida, bool>>> where = new List<Expression<Func<UnidadMedida, bool>>>();
                where.Add(x => x.Id == id);
                UnidadMedida unidadMedidum = _unidadMedidumRepository.Get(where).FirstOrDefault();
                if (unidadMedidum != null)
                {
                    unidadMedidum.FechaEliminado = DateTime.Now;
                    unidadMedidum.Eliminado = true;
                    _unidadMedidumRepository.Update(unidadMedidum);

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
        public IActionResult Edit(int id,[FromBody] UnidadMedidaDTO unidadMedidumDTO)
        {
            try
            {
                List<Expression<Func<UnidadMedida, bool>>> where = new List<Expression<Func<UnidadMedida, bool>>>();
                where.Add(x => x.Id == id);
                UnidadMedida unidadMedSearch = _unidadMedidumRepository.Get(where).FirstOrDefault();

                if (unidadMedSearch != null)
                {
                    if (unidadMedidumDTO.UMedida!=null)unidadMedSearch.UMedida = unidadMedidumDTO.UMedida;
                    if (unidadMedidumDTO.FechaCreacion != null) unidadMedSearch.FechaCreacion = string.IsNullOrWhiteSpace(unidadMedidumDTO.FechaCreacion) ? null : DateTime.ParseExact(unidadMedidumDTO.FechaCreacion, "yyyy-MM-dd", null);

                    _unidadMedidumRepository.Update(unidadMedSearch);

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
                List<UnidadMedida> unidadMedidas = new List<UnidadMedida>();
                List<Expression<Func<UnidadMedida, bool>>> where = new List<Expression<Func<UnidadMedida, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                unidadMedidas = _unidadMedidumRepository.Get(where).ToList();

                List<UnidadMedidaDTO> unidadMedidaDTOs = unidadMedidas.Select(UnidadMedidaDTO.FromEntity).ToList();

                return Ok(unidadMedidaDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

    }
}
