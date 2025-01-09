using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMaterialController : ControllerBase
    {
        private ITipoMaterialRepository _tipoMaterialRepository;
        public TipoMaterialController(ITipoMaterialRepository tipoMaterialRepository)
        {
            _tipoMaterialRepository = tipoMaterialRepository;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] TipoMaterial tipoMaterial)
        {

            try
            {
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Nombre.ToLower() == tipoMaterial.Nombre.ToLower());
                TipoMaterial tipoMaterialSearch = _tipoMaterialRepository.Get(where).FirstOrDefault();

                if (tipoMaterialSearch == null)
                {
                    tipoMaterial.FechaCreacion = DateTime.Now;
                    _tipoMaterialRepository.Insert(tipoMaterial);

                    return Ok(tipoMaterial);
                }
                else
                {
                    return BadRequest("El tipo material ya existe");
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
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                TipoMaterial tipoMaterial = _tipoMaterialRepository.Get(where).FirstOrDefault();
                if (tipoMaterial != null)
                {
                    tipoMaterial.FechaEliminado = DateTime.Now;
                    tipoMaterial.Eliminado = true;
                    _tipoMaterialRepository.Update(tipoMaterial);

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
        public IActionResult Edit([FromBody] TipoMaterial tipoMaterial)
        {
            try
            {
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Id == tipoMaterial.Id);
                TipoMaterial tipoMaterialSearch = _tipoMaterialRepository.Get(where).FirstOrDefault();

                if (tipoMaterialSearch != null)
                {
                    _tipoMaterialRepository.Update(tipoMaterial);

                    return Ok(tipoMaterial);
                }
                else
                {
                    return BadRequest("Tipo de Material no encontrado");
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
                List<TipoMaterial> tipoMaterials = new List<TipoMaterial>();
                List<Expression<Func<TipoMaterial, bool>>> where = new List<Expression<Func<TipoMaterial, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                tipoMaterials = _tipoMaterialRepository.Get(where).ToList();

                return Ok(tipoMaterials);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
