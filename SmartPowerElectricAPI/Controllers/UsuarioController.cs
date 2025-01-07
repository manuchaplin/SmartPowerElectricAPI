using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using SmartPowerElectricAPI.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private static List<string> TokenBlacklist = new List<string>();
        public UsuarioController (IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }


        #region Login/Logout
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Agrega el token a la lista de revocados
            TokenBlacklist.Add(token);
            return Ok("Sesión cerrada correctamente.");
        }

        public static bool IsTokenRevoked(string token)
        {
            return TokenBlacklist.Contains(token);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {

            // Valida las credenciales del usuario (puedes consultar una base de datos aquí)
            List<Expression<Func<Usuario, bool>>> where = new List<Expression<Func<Usuario, bool>>>();
            where.Add(x => x.Usuario1 == request.Username && x.Password == EncryptHelper.GetSHA1(request.Password));
            where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
            Usuario usuario= _usuarioRepository.Get(where).FirstOrDefault();

            if (usuario!=null)
            {
                var token = GenerarTokenJWT(request.Username);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized("Credenciales inválidas o usuario eliminado");
            }          
          
        }

        private string GenerarTokenJWT(string username)
        {
            var jwtConfig = _configuration.GetSection("Jwt");

            // Claims del token
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, "Usuario"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // Clave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

           
            // Genera el token
            var token = new JwtSecurityToken(
                 expires: DateTime.Now.AddMinutes(double.Parse(jwtConfig["ExpireMinutes"])),
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,               
                signingCredentials: creds);;
         

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion



        [HttpPost("create")]
        [Authorize]
        public IActionResult CreateUser([FromBody] Usuario usuario)//
        {
            //Usuario usuario = new Usuario();

            //usuario.Nombre = "Manuel";
            //usuario.Apellido = "Mon";
            //usuario.Email = "manuchaplin@gmail.com";
            //usuario.Telefono = 645779423;
            //usuario.Usuario1 = "manuel.mon";
            //usuario.Password = EncryptHelper.GetSHA1("12345");
            try
            {
                List<Expression<Func<Usuario, bool>>> where = new List<Expression<Func<Usuario, bool>>>();
                where.Add(x => x.Usuario1 == usuario.Usuario1 && x.Password == EncryptHelper.GetSHA1(usuario.Password));
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Usuario usuariosearch = _usuarioRepository.Get(where).FirstOrDefault();

                if (usuariosearch == null)
                {
                    usuario.FechaCreacion = DateTime.Now;
                    usuario.Password = EncryptHelper.GetSHA1(usuario.Password);
                    _usuarioRepository.Insert(usuario);

                    return Ok(usuario);
                }
                else
                {
                    return BadRequest("El usuario ya existe");
                }
               

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        //[Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                List<Expression<Func<Usuario, bool>>> where = new List<Expression<Func<Usuario, bool>>>();
                where.Add(x => x.Id==id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Usuario usuario = _usuarioRepository.Get(where).FirstOrDefault();
                if (usuario!=null)
                {
                    usuario.FechaEliminado = DateTime.Now;
                    usuario.Eliminado = true;

                    _usuarioRepository.Update(usuario);

                    return Ok("Eliminado correctamente");
                }
                else
                {
                    return BadRequest("No existente");
                }
           

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //// GET: api/<UsuarioController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
                         
        //// GET api/<UsuarioController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<UsuarioController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UsuarioController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

       
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
