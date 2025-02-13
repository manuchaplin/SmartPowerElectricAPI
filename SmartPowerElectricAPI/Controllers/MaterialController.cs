using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SmartPowerElectricAPI.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        private IOrdenRepository _ordenRepository;
        private readonly SmartPowerElectricContext _context;
        public MaterialController(IMaterialRepository materialRepository, IOrdenRepository ordenRepository, SmartPowerElectricContext context)
        {
            _materialRepository = materialRepository;
            _ordenRepository = ordenRepository;
            _context = context;
        }

        [HttpPost("create{idOrden}")]
        public IActionResult Create(int idOrden,[FromBody] MaterialDTO materialDTO)
        {
            try
            {
              
                List<Expression<Func<Orden, bool>>> where = new List<Expression<Func<Orden, bool>>>();
                where.Add(x => x.Id== idOrden);
                
                Orden ordenSearch = _ordenRepository.Get(where).FirstOrDefault();

                if (ordenSearch == null) return NotFound();
               
                Material material=materialDTO.ToEntity();
                material.IdOrden = idOrden;
                material.FechaCreacion = DateTime.Now;
                _materialRepository.Insert(material);

                return Ok();
             
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
                List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Material material = _materialRepository.Get(where).FirstOrDefault();
                if (material != null)
                {
                    material.FechaEliminado = DateTime.Now;
                    material.Eliminado = true;
                    _materialRepository.Update(material);

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
        public IActionResult Edit(int id,[FromBody] MaterialDTO materialDTO)
        {
            try
            {
                List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                where.Add(x => x.Id == id);
                Material materialSearch = _materialRepository.Get(where).FirstOrDefault();

                if (materialSearch != null)
                {
                    if (materialDTO.Precio!=null)materialSearch.Precio=materialDTO.Precio;
                    if (materialDTO.Cantidad != null)materialSearch.Cantidad = materialDTO.Cantidad;
                    if (materialDTO.IdTipoMaterial != null)materialSearch.IdTipoMaterial = (int)materialDTO.IdTipoMaterial;
                    if (materialDTO.IdUnidadMedida != null)materialSearch.IdUnidadMedida = (int)materialDTO.IdUnidadMedida;                   
                    if (materialDTO.IdOrden != null)materialSearch.IdOrden = (int)materialDTO.IdOrden;                   
                    if (materialDTO.FechaCreacion != null) materialSearch.FechaCreacion = string.IsNullOrWhiteSpace(materialDTO.FechaCreacion) ? null : DateTime.ParseExact(materialDTO.FechaCreacion, "yyyy-MM-dd", null);

                    _materialRepository.Update(materialSearch);

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

        [HttpGet("list{idOrden}")]
        public IActionResult List(int idOrden)//ActionResult<IEnumerable<Material>>
        {
            try
            {
                List<Material> materials = new List<Material>();
                List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                where.Add(x => x.IdOrden == idOrden);
                materials = _materialRepository.Get(where).ToList();

          
                List<MaterialDTO> materialDTOs = materials.Select(MaterialDTO.FromEntity).ToList();

                return Ok(materialDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


    }
}
