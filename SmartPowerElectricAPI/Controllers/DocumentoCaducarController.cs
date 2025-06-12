using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using SmartPowerElectricAPI.Service;
using SmartPowerElectricAPI.Utilities;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentoCaducarController : ControllerBase
    {
        private IDocumentoCaducarRepository _documentoCaducarRepository;
        private ITrabajadorRepository _trabajadorRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private EmailService _emailService;
        private SmartPowerElectricContext _context;
        public DocumentoCaducarController(IDocumentoCaducarRepository documentoCaducarRepository, ITrabajadorRepository trabajadorRepository, IUsuarioRepository usuarioRepository, EmailService emailService, SmartPowerElectricContext context)
        {
            _documentoCaducarRepository = documentoCaducarRepository;
            _trabajadorRepository = trabajadorRepository;
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] DocumentoCaducarDTO documentoCaducarDTO)
        {

            try
            {
                List<Expression<Func<DocumentoCaducar, bool>>> where = new List<Expression<Func<DocumentoCaducar, bool>>>();
                where.Add(x => x.Nombre.ToLower() == documentoCaducarDTO.Nombre.ToLower());

                DocumentoCaducar documentoCaducarSearch = _documentoCaducarRepository.Get(where).FirstOrDefault();

                if (documentoCaducarSearch == null)
                {
                    DocumentoCaducar documentoCaducar = documentoCaducarDTO.ToEntity();
                    documentoCaducar.FechaCreacion = DateTime.Now;

                    List<string> MessageError = Validate(documentoCaducar);
                    if (MessageError.Count()>0)
                    {
                        return Conflict(new { message = MessageError });
                    }
                    _documentoCaducarRepository.Insert(documentoCaducar);

                    return Ok();
                }
                else
                {                    
                    return Conflict(new { message = "El documento ya existe" });
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
                List<Expression<Func<DocumentoCaducar, bool>>> where = new List<Expression<Func<DocumentoCaducar, bool>>>();
                where.Add(x => x.Id == id);
                DocumentoCaducar documentoCaducar = _documentoCaducarRepository.Get(where).FirstOrDefault();
                if (documentoCaducar != null)
                {
                    _documentoCaducarRepository.Delete(documentoCaducar);

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
        public IActionResult Edit(int id,[FromBody] DocumentoCaducarDTO documentoCaducarDTO)
        {
            try
            {
                List<Expression<Func<DocumentoCaducar, bool>>> where = new List<Expression<Func<DocumentoCaducar, bool>>>();
                where.Add(x => x.Id == id);
                DocumentoCaducar documentoCaducarSearch = _documentoCaducarRepository.Get(where).FirstOrDefault();

                if (documentoCaducarSearch != null)
                {
                    if (documentoCaducarDTO.Nombre != null && documentoCaducarDTO.Nombre!="") documentoCaducarSearch.Nombre = documentoCaducarDTO.Nombre;
                    if (documentoCaducarDTO.FechaExpiracion != null && documentoCaducarDTO.FechaExpiracion != "") documentoCaducarSearch.FechaExpiracion = string.IsNullOrWhiteSpace(documentoCaducarDTO.FechaExpiracion) ? null : DateTime.ParseExact(documentoCaducarDTO.FechaExpiracion, "yyyy-MM-dd", null);
                    if (documentoCaducarDTO.FechaExpedicion != null && documentoCaducarDTO.FechaExpedicion != "") documentoCaducarSearch.FechaExpedicion = string.IsNullOrWhiteSpace(documentoCaducarDTO.FechaExpedicion) ? null : DateTime.ParseExact(documentoCaducarDTO.FechaExpedicion, "yyyy-MM-dd", null);

                    _documentoCaducarRepository.Update(documentoCaducarSearch);

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
                List<DocumentoCaducar> documentoCaducars = new List<DocumentoCaducar>();
                List<Expression<Func<DocumentoCaducar, bool>>> where = new List<Expression<Func<DocumentoCaducar, bool>>>();
                
                documentoCaducars = _documentoCaducarRepository.Get(where,"Trabajador").ToList();
               

                List<DocumentoCaducarDTO> documentoCaducarDTOs= documentoCaducars.Select(DocumentoCaducarDTO.FromEntity).ToList();

                return Ok(documentoCaducarDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        //[AllowAnonymous]
        [HttpGet("sendEmailDocumentExpiration")]
        public async Task<IActionResult> sendEmailDocumentExpiration()
        {
            try
            {
                List<Expression<Func<Usuario, bool>>> whereUser = new List<Expression<Func<Usuario, bool>>>();
                whereUser.Add(x => x.Id == 3);//3mayda
                whereUser.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Usuario usuario = _usuarioRepository.Get(whereUser).FirstOrDefault();

                if (usuario.FechaLanzamiento?.Date != DateTime.Now.Date)
                {
                    try
                    {
                        DateTime dateTime15Early = DateTime.Now.AddDays(15);
                        DateTime dateTime30Early = DateTime.Now.AddDays(30);
                        DateTime dateTime31Early = DateTime.Now.AddDays(31);
                        List<Trabajador> trabajadors = new List<Trabajador>();
                        List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                        where.Add(x => x.DocumentoCaducars.Any(y => y.FechaExpiracion.HasValue && ((y.FechaExpiracion.Value.Date <= dateTime31Early.Date && y.FechaExpiracion.Value.Date >= dateTime30Early.Date) || y.FechaExpiracion.Value.Date <= dateTime30Early.Date)));
                        trabajadors = _trabajadorRepository.Get(where, "DocumentoCaducars").ToList();

                        if (trabajadors.Count() > 0)
                        {
                            foreach (var trabajador in trabajadors)
                            {

                                string MailTo = trabajador.Email;
                                string Topic = "Documentation about to expire";
                                string Body = "<div>";
                                Body += "<p>Dear " + trabajador.Nombre + " " + trabajador.Apellido + "</p>";
                                Body += "<p>The following documentation is soon to expire.</p>";

                                Body += "</br>";
                                Body += "<div>";
                                foreach (var documento in trabajador.DocumentoCaducars)
                                {
                                    //if (documento.FechaExpiracion.Value.Date == dateTime15Early.Date || documento.FechaExpiracion.Value.Date == dateTime30Early.Date)
                                    if ((documento.FechaExpiracion.Value.Date <= dateTime31Early.Date && documento.FechaExpiracion.Value.Date >= dateTime30Early.Date) || documento.FechaExpiracion.Value.Date <= dateTime30Early.Date)
                                    {
                                        Body += "<p>Name: " + documento.Nombre + " Expiration Date: " + documento.FechaExpiracion?.ToString("yyyy - dd - MM") + " </p>";
                                    }
                                }
                                Body += "</div>";
                                Body += "<p>We remain at your disposal for any further clarification.</p>";
                                Body += "</br>";
                                Body += "<p>Best Regards,</p>";
                                Body += "<p><bold>Smart Power Electric.</bold></p>";

                                Body += "</div>";
                                await _emailService.SendMailAsync(MailTo, Topic, Body);
                                Body = "";
                            }

                        }
                        usuario.FechaLanzamiento=DateTime.Now;
                        _usuarioRepository.Update(usuario);

                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }
                }                               
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok();
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public List<string> Validate(DocumentoCaducar documento)
        {
            List<string> Message = new List<string>();
            if (string.IsNullOrEmpty(documento.Nombre))
            {
                Message.Add("La Nombre del documento no puede ser vacio.");
            }
            if (documento.IdTrabajador==null)
            {
                Message.Add("La IdTrabajador del documento no puede ser vacio.");
            }
            if (documento.FechaExpedicion==null)
            {
                Message.Add("La Fecha Expedición del documento no puede ser vacio.");
            }
            if (documento.FechaExpiracion==null)
            {
                Message.Add("La Fecha Expiración del documento no puede ser vacio.");
            }
         

            return Message;
        }
    }
}
