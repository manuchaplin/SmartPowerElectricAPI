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
        private readonly ILogger _logger;
        public UnidadMedidumController (IUnidadMedidumRepository unidadMedidumRepository, IConfiguration configuration, ILogger logger)
        {
            _unidadMedidumRepository = unidadMedidumRepository;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost("create")]       
        public IActionResult Create([FromBody] UnidadMedida unidadMedidum)
        {
          
            try
            {                
                List<Expression<Func<UnidadMedida, bool>>> where = new List<Expression<Func<UnidadMedida, bool>>>();
                where.Add(x => x.UMedida == unidadMedidum.UMedida);
                UnidadMedida unidadMed = _unidadMedidumRepository.Get(where).FirstOrDefault();

                if (unidadMed == null)
                {                   
                    _unidadMedidumRepository.Insert(unidadMedidum);

                    return Ok(unidadMedidum);
                }
                else
                {
                    return BadRequest("La unidad de medida ya existe");
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
                List<Expression<Func<UnidadMedida, bool>>> where = new List<Expression<Func<UnidadMedida, bool>>>();
                where.Add(x => x.Id == id);
                UnidadMedida unidadMedidum = _unidadMedidumRepository.Get(where).FirstOrDefault();
                if (unidadMedidum != null)
                {
                    _unidadMedidumRepository.Delete(id);

                    return Ok();
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

                    _unidadMedidumRepository.Update(unidadMedSearch);

                    return Ok(unidadMedSearch);
                }
                else
                {
                    return BadRequest("Unidad de medida no encontrada");
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
                List<UnidadMedida> unidadMedidas = new List<UnidadMedida>();
                unidadMedidas = _unidadMedidumRepository.Get().ToList();

                return Ok(unidadMedidas);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
