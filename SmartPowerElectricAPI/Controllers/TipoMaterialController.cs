using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TipoMaterialController : ControllerBase
    {
        private ITipoMaterialRepository _tipoMaterialRepository;
        public TipoMaterialController(ITipoMaterialRepository tipoMaterialRepository)
        {
            _tipoMaterialRepository = tipoMaterialRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] TipoMaterialDTO tipoMaterialDTO)
        {

            try
            {
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Nombre.ToLower() == tipoMaterialDTO.Nombre.ToLower());
                TipoMaterial tipoMaterialSearch = _tipoMaterialRepository.Get(where).FirstOrDefault();

                if (tipoMaterialSearch == null)
                {
                    TipoMaterial tipoMaterial= tipoMaterialDTO.ToEntity();
                    tipoMaterial.FechaCreacion = DateTime.Now;
                    _tipoMaterialRepository.Insert(tipoMaterial);

                    return Ok();
                }
                else
                {                    
                    return Conflict(new { message = "El tipo material ya existe" });
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
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                TipoMaterial tipoMaterial = _tipoMaterialRepository.Get(where).FirstOrDefault();
                if (tipoMaterial != null)
                {
                    tipoMaterial.FechaEliminado = DateTime.Now;
                    tipoMaterial.Eliminado = true;
                    _tipoMaterialRepository.Update(tipoMaterial);

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
        public IActionResult Edit(int id,[FromBody] TipoMaterialDTO tipoMaterialDTO)
        {
            try
            {
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Id == id);
                TipoMaterial tipoMaterialSearch = _tipoMaterialRepository.Get(where).FirstOrDefault();

                if (tipoMaterialSearch != null)
                {
                    if (tipoMaterialDTO.Nombre != null) tipoMaterialSearch.Nombre = tipoMaterialDTO.Nombre;
                    if (tipoMaterialDTO.FechaCreacion != null) tipoMaterialSearch.FechaCreacion = string.IsNullOrWhiteSpace(tipoMaterialDTO.FechaCreacion) ? null : DateTime.ParseExact(tipoMaterialDTO.FechaCreacion, "MM-dd-yyyy", null);

                    _tipoMaterialRepository.Update(tipoMaterialSearch);

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
                List<TipoMaterial> tipoMaterials = new List<TipoMaterial>();
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                tipoMaterials = _tipoMaterialRepository.Get(where).ToList();

                List<TipoMaterialDTO> tipoMaterialDTOs= tipoMaterials.Select(TipoMaterialDTO.FromEntity).ToList();

                return Ok(tipoMaterialDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

    }
}
