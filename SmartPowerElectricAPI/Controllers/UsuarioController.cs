using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Migrations;
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
            return Ok();
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
            where.Add(x => x.Username == request.Username && x.Password == EncryptHelper.GetSHA1(request.Password));
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

        [HttpGet("validateToken")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var claveSecreta = _configuration["Jwt:Key"];//"EstaEsUnaClaveMuySeguraYDe32Caracteres"; // La misma clave utilizada para firmar el token
            var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta));
            var handler = new JwtSecurityTokenHandler();

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            string token = authorizationHeader.Split("Bearer ")[1];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token no proporcionado o formato invalido");
            }
            string asd = _configuration["Jwt:Issuer"];
            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],//capturar variables del setting
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = clave
                }, out SecurityToken securityToken);

                return Ok(new { mensaje = "Token válido" }); // El token es válido
            }
            catch (Exception)
            {
                return Unauthorized(new { mensaje = "Token inválido" }); // El token no es válido
            }
        }
        #endregion



        [HttpPost("create")]       
        public IActionResult Create([FromBody] UsuarioDTO usuarioDTO)
        {            
            try
            {
                List<Expression<Func<Usuario, bool>>> where = new List<Expression<Func<Usuario, bool>>>();
                where.Add(x => x.Username.ToLower() == usuarioDTO.Username.ToLower() || (x.Nombre.ToLower()==usuarioDTO.Nombre.ToLower() && x.Apellido.ToLower()==usuarioDTO.Apellido.ToLower()));
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);             
                Usuario usuariosearch = _usuarioRepository.Get(where).FirstOrDefault();

                if (usuariosearch == null)
                {
                    Usuario usuario = usuarioDTO.ToEntity();
                    usuario.FechaCreacion = DateTime.Now;
                    usuario.Password = EncryptHelper.GetSHA1(usuario.Password);
                    _usuarioRepository.Insert(usuario);

                    return Ok(usuario);
                }
                else
                {
                    return Conflict(new { message = "El Usuario ya existe" });
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
       
        [HttpDelete("{id}")]
        [Authorize]
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
                    if (usuario.Protegido==true)
                    {
                        return Forbid();
                    }
                   
                    usuario.FechaEliminado = DateTime.Now;
                    usuario.Eliminado = true;

                    _usuarioRepository.Update(usuario);
                    return Ok();
                                      
                   
                }
                else
                {
                    return NotFound();
                }
           

            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Edit(int id,[FromBody] UsuarioDTO usuarioDTO) 
        {
            try
            {
                List<Expression<Func<Usuario, bool>>> where = new List<Expression<Func<Usuario, bool>>>();
                where.Add(x=>x.Id==id);               
                Usuario usuariosearch = _usuarioRepository.Get(where).FirstOrDefault();

                if (usuariosearch != null)
                {
                    if (usuarioDTO.Nombre != null) usuariosearch.Nombre = usuarioDTO.Nombre;
                    if (usuarioDTO.Apellido != null) usuariosearch.Apellido = usuarioDTO.Apellido;
                    if (usuarioDTO.Email != null) usuariosearch.Email = usuarioDTO.Email;
                    if (usuarioDTO.Username != null) usuariosearch.Username = usuarioDTO.Username;
                    if (usuarioDTO.Password != null) usuariosearch.Password = EncryptHelper.GetSHA1(usuarioDTO.Password); ;
                    if (usuarioDTO.Telefono != null) usuariosearch.Telefono = usuarioDTO.Telefono;
                    if (usuarioDTO.FechaCreacion != null) usuariosearch.FechaCreacion = string.IsNullOrWhiteSpace(usuarioDTO.FechaCreacion) ? null : DateTime.ParseExact(usuarioDTO.FechaCreacion, "yyyy-MM-dd", null);

                    _usuarioRepository.Update(usuariosearch);

                    return Ok(usuariosearch);
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


        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            try
            {
                Usuario usuario = new Usuario();
                usuario = _usuarioRepository.GetByID(id);

                if (usuario == null)return NotFound();

                UsuarioDTO usuarioDTO = UsuarioDTO.FromEntity(usuario);

                return Ok(usuarioDTO);

            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("list")]
        [Authorize]
        public IActionResult List()
        {
            try
            {
                List<Usuario> usuarios = new List<Usuario>();
                List<Expression<Func<Usuario, bool>>> where = new List<Expression<Func<Usuario, bool>>>();
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                usuarios = _usuarioRepository.Get(where).ToList();

                List<UsuarioDTO> usuarioDTOs = usuarios.Select(UsuarioDTO.FromEntity).ToList();

                return Ok(usuarios);

            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
      
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
