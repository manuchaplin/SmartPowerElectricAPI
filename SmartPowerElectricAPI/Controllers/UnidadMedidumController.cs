using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;

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
        // GET: api/<UnidadMedidumController>
        [HttpGet("GetAllTest")]
        public IActionResult GetAllTest()
        {
            List<UnidadMedidum> unidadMedida = _unidadMedidumRepository.Get().ToList();          
            return Ok(unidadMedida);
        }

        // GET api/<UnidadMedidumController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UnidadMedidumController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UnidadMedidumController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UnidadMedidumController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
