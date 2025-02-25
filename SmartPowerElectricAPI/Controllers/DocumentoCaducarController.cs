using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using SmartPowerElectricAPI.Service;
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
        private EmailService _emailService;
        public DocumentoCaducarController(IDocumentoCaducarRepository documentoCaducarRepository, ITrabajadorRepository trabajadorRepository, EmailService emailService)
        {
            _documentoCaducarRepository = documentoCaducarRepository;
            _trabajadorRepository = trabajadorRepository;
            _emailService = emailService;
        }

        [HttpPost("create/{idTrabajador}")]
        public IActionResult Create(int idTrabajador,[FromBody] DocumentoCaducarDTO documentoCaducarDTO)
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
                    documentoCaducar.IdTrabajador = idTrabajador;
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
                    if (documentoCaducarDTO.Nombre != null) documentoCaducarSearch.Nombre = documentoCaducarDTO.Nombre;
                    if (documentoCaducarDTO.FechaExpiracion != null) documentoCaducarSearch.FechaExpiracion = string.IsNullOrWhiteSpace(documentoCaducarDTO.FechaExpiracion) ? null : DateTime.ParseExact(documentoCaducarDTO.FechaExpiracion, "yyyy-MM-dd", null);

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
                //List<Expression<Func<DocumentoCaducar, bool>>> where = new List<Expression<Func<DocumentoCaducar, bool>>>();
                //where.Add(x => x.IdTrabajador==idTrabajador);
                documentoCaducars = _documentoCaducarRepository.Get().ToList();

                List<DocumentoCaducarDTO> documentoCaducarDTOs= documentoCaducars.Select(DocumentoCaducarDTO.FromEntity).ToList();

                return Ok(documentoCaducarDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [AllowAnonymous]
        [HttpGet("sendEmailDocumentExpiration")]
        public async Task<IActionResult> sendEmailDocumentExpiration()
        {
            try
            {
                DateTime dateTime15Early= DateTime.Now.AddDays(15);
                DateTime dateTime30Early= DateTime.Now.AddDays(30);
                List<Trabajador> trabajadors = new List<Trabajador>();
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.DocumentoCaducars.Any(y=>y.FechaExpiracion.HasValue && (y.FechaExpiracion.Value.Date == dateTime15Early.Date || y.FechaExpiracion.Value.Date == dateTime30Early.Date)));
                trabajadors=_trabajadorRepository.Get(where, "DocumentoCaducars").ToList();

                if (trabajadors.Count()>0)
                {                   
                    foreach (var trabajador in trabajadors)
                    {
                        string MailTo = "manuchaplin@gmail.com";
                        //string MailTo = trabajador.Email;
                        string Topic = "Documentation about to expire";
                        string Body = "<div>";
                        Body += "<p>Dear " + trabajador.Nombre + " " + trabajador.Apellido + "</p>";
                        Body += "<p>The following documentation is soon to expire.</p>";
                      
                        Body += "</br>";
                        Body += "<div>";
                        foreach (var documento in trabajador.DocumentoCaducars)
                        {
                            if (documento.FechaExpiracion.Value.Date == dateTime15Early.Date || documento.FechaExpiracion.Value.Date == dateTime30Early.Date)
                            {
                                Body += "<p>Name: "+documento.Nombre+ " Expiration Date: "+documento.FechaExpiracion?.ToString("yyyy - dd - MM")+" </p>";                            
                            }
                        }
                        Body += "</div>";
                        Body += "<p>We remain at your disposal for any further clarification.</p>";
                        Body += "</br>";
                        Body += "<p>Atentamente,</p>";
                        Body += "<p><bold>Smart Power Electric.</bold></p>";

                        Body += "</div>";
                        await _emailService.SendMailAsync(MailTo, Topic, Body);
                        Body = "";
                    }

                }

                return Ok("Mensaje enviado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
