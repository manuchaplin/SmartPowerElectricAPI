using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public IActionResult Create([FromBody] Material material)
        {
            try
            {
                //Cuando se incorpore proyecto al modelo hay q hacer la validacion de la creacion de material, para no agregar dos materiales iguales al mismo proyecto, con el mismo precio
                //List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                //where.Add(x => x.Nombre.ToLower() == cliente.Nombre.ToLower());
                //where.Add(x => x.Email.ToLower() == cliente.Email.ToLower());
                //Material materialSearch = _clienteRepository.Get(where).FirstOrDefault();

                //if (clienteSearch == null)
                //{
                material.FechaCreacion = DateTime.Now;
                _materialRepository.Insert(material);

                return Ok(material);
                //}
                //else
                //{
                //    return BadRequest("El cliente ya existe");
                //}


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
                List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Material material = _materialRepository.Get(where).FirstOrDefault();
                if (material != null)
                {
                    material.FechaEliminado = DateTime.Now;
                    material.Eliminado = true;
                    _materialRepository.Update(material);

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
        public IActionResult Edit([FromBody] Material material)
        {
            try
            {
                List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                where.Add(x => x.Id == material.Id);
                Material materialSearch = _materialRepository.Get(where).FirstOrDefault();

                if (materialSearch != null)
                {
                    _materialRepository.Update(material);

                    return Ok(material);
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
        public IActionResult List()//ActionResult<IEnumerable<Material>>
        {
            try
            {
                List<Material> materials = new List<Material>();
                List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                materials = _materialRepository.Get(where).ToList();


                //var materials = _context.Material.Include(x => x.TipoMaterial).Include(x => x.UnidadMedida).ToList();//Probar con Yannia Luego

                return Ok(materials);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
