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
        private readonly SmartPowerElectricContext _context;
        public MaterialController(IMaterialRepository materialRepository, SmartPowerElectricContext context)
        {
            _materialRepository = materialRepository;
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] MaterialDTO materialDTO)
        {
            try
            {
                //Cuando se incorpore proyecto al modelo hay q hacer la validacion de la creacion de material, para no agregar dos materiales iguales al mismo proyecto, con el mismo precio
                //List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                //where.Add(x => x.Nombre.ToLower() == cliente.Nombre.ToLower());
                //where.Add(x => x.Email.ToLower() == cliente.Email.ToLower());
                //Material materialSearch = _clienteRepository.Get(where).FirstOrDefault();

                //if (materialSearch == null)
                //{
                Material material=materialDTO.ToEntity();
                material.FechaCreacion = DateTime.Now;
                _materialRepository.Insert(material);

                return Ok();
                //}
                //else
                //{
                //    return Conflict(new { message="El cliente ya existe"});
                //}


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
                    if (materialDTO.IdProyecto != null)materialSearch.IdProyecto = (int)materialDTO.IdProyecto;
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

        [HttpGet("list")]
        public IActionResult List()//ActionResult<IEnumerable<Material>>
        {
            try
            {
                List<Material> materials = new List<Material>();
                List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                materials = _materialRepository.Get(where).ToList();

                //var materials = _context.Material.Include(x => x.TipoMaterial).Include(x => x.UnidadMedida).ToList();//Probar con Yannia Luego
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
