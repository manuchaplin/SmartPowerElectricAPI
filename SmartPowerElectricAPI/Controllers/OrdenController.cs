using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using SmartPowerElectricAPI.Service;
using SmartPowerElectricAPI.Services;
using SmartPowerElectricAPI.Utilities;

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdenController : ControllerBase
    {
        private IOrdenRepository _ordenRepository;
        private ITrabajadorRepository _trabajadorRepository;
        private IMaterialRepository _materialRepository;
        private readonly EmailService _emailService;
        private readonly FileService _fileService;
        private readonly PDFService _pdfService;
        private readonly IWebHostEnvironment _env;
        public OrdenController(IOrdenRepository ordenRepository, ITrabajadorRepository trabajadorRepository, IMaterialRepository materialRepository, EmailService emailService, FileService fileService, PDFService pdfService, IWebHostEnvironment env)
        {
            _ordenRepository = ordenRepository;
            _trabajadorRepository = trabajadorRepository;
            _materialRepository = materialRepository;
            _emailService = emailService;
            _fileService = fileService;
            _pdfService = pdfService;
            _env = env;
        }

        [HttpPost("create/{idProyecto}")]
        public IActionResult Create(int idProyecto, [FromBody] OrdenDTO ordenDTO)
        {

            try
            {
                List<Expression<Func<Orden, bool>>> where = new List<Expression<Func<Orden, bool>>>();
                where.Add(x => x.IdProyecto == idProyecto);
                Orden ordenSearch = _ordenRepository.Get(where).OrderBy(x => x.NumeroOrden).LastOrDefault();

                if (ordenSearch == null)
                {
                    ordenDTO.NumeroOrden = 1;
                }
                else
                {
                    ordenDTO.NumeroOrden = ordenSearch.NumeroOrden + 1;
                }

                Orden orden = ordenDTO.ToEntity();
                orden.IdProyecto = idProyecto;
                orden.Cobrado = ordenDTO.Cobrado ?? 0;
                orden.OrdenFinalizada = ordenDTO.OrdenFinalizada ?? false;
                orden.FechaCreacion = DateTime.Now;
                _ordenRepository.Insert(orden);

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
                List<Expression<Func<Orden, bool>>> where = new List<Expression<Func<Orden, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Orden orden = _ordenRepository.Get(where).FirstOrDefault();
                if (orden != null)
                {
                    orden.FechaEliminado = DateTime.Now;
                    orden.Eliminado = true;
                    _ordenRepository.Update(orden);

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
        public IActionResult Edit(int id, [FromBody] OrdenDTO ordenDTO)
        {
            try
            {
                List<Expression<Func<Orden, bool>>> where = new List<Expression<Func<Orden, bool>>>();
                where.Add(x => x.Id == id);
                Orden ordenSearch = _ordenRepository.Get(where).FirstOrDefault();

                if (ordenSearch != null)
                {
                    if (ordenDTO.OrdenFinalizada != null) ordenSearch.OrdenFinalizada = ordenDTO.OrdenFinalizada;
                    if (ordenDTO.CosteManoObra != null) ordenSearch.CosteManoObra = ordenDTO.CosteManoObra;
                    if (ordenDTO.Cobrado != null) ordenSearch.Cobrado = ordenDTO.Cobrado;
                    if (ordenDTO.HorasEstimadas != null) ordenSearch.HorasEstimadas = ordenDTO.HorasEstimadas;
                    if (ordenDTO.IdProyecto != null) ordenSearch.IdProyecto = ordenDTO.IdProyecto;
                    if (ordenDTO.FechaCreacion != null) ordenSearch.FechaCreacion = string.IsNullOrWhiteSpace(ordenDTO.FechaCreacion) ? null : DateTime.ParseExact(ordenDTO.FechaCreacion, "yyyy-MM-dd", null);


                    _ordenRepository.Update(ordenSearch);

                    return Ok(ordenSearch);
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


        [HttpGet("listMaterials/{idOrden}")]
        public IActionResult ListMaterials(int idOrden)//ActionResult<IEnumerable<Material>>
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

        [HttpGet("listTrabajadores/{idOrden}")]
        public IActionResult ListTrabajadores(int idOrden)
        {
            try
            {
                Orden orden = new Orden();
                orden = _ordenRepository.GetByID(idOrden, "Trabajadores");

                List<TrabajadorDTO> trabajadorDTOs = orden.Trabajadores.Select(TrabajadorDTO.FromEntity).ToList();

                return Ok(trabajadorDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("asociarOrdenTrabajador")]
        public IActionResult AddWorkerToOrder([FromBody] OrdenTrabajadorPar ordenTrabajadorPar)
        {
            try
            {
                Orden orden = new Orden();
                orden = _ordenRepository.GetByID(ordenTrabajadorPar.idOrden, "Trabajadores");
                Trabajador trabajador = new Trabajador();
                trabajador = _trabajadorRepository.GetByID(ordenTrabajadorPar.idTrabajador);
                if (orden == null) return NotFound();
                if (trabajador == null) return NotFound();

                if (!orden.Trabajadores.Any(x => x.Id == ordenTrabajadorPar.idTrabajador))
                {
                    orden.Trabajadores.Add(trabajador);
                    _ordenRepository.Update(orden);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpDelete("{idOrden},{idTrabajador}")]
        public IActionResult DeleteWorkerToOrder(int idOrden, int idTrabajador)
        {
            try
            {
                Orden orden = new Orden();
                orden = _ordenRepository.GetByID(idOrden, "Trabajadores");
                Trabajador trabajador = new Trabajador();
                trabajador = _trabajadorRepository.GetByID(idTrabajador);
                if (orden == null) return NotFound();
                if (trabajador == null) return NotFound();

                if (orden.Trabajadores.Any(x => x.Id == idTrabajador))
                {
                    orden.Trabajadores.Remove(trabajador);
                    _ordenRepository.Update(orden);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("SendMail/{idOrden}")]
        public async Task<IActionResult> SendMailBills(int idOrden)
        {
            try
            {
                Orden orden = _ordenRepository.GetByID(idOrden, "Trabajadores,Materials,Proyecto");

                if (orden == null)
                {
                    return NotFound();
                }

                // Datos del correo
                string MailTo = "manuchaplin@gmail.com";
                string Topic = "Confirmación de Orden";
                string Body = "<div>";
                Body += "<p>Buenos días</p>";
                Body += "<p>Factura de prueba</p>";
                Body += "</div>";

                // Generar factura (PDF)
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "BillTemp", $"FacturaProyecto_{orden.IdProyecto}_Orden_{idOrden}.pdf");
                _pdfService.GenerarFacturaPdf(filePath, orden.NumeroOrden, orden.CosteManoObra ?? 0, orden.Cobrado ?? 0);

                // Enviar correo con el archivo adjunto
                List<string> Attachments = new List<string> { filePath };

                await _emailService.SendMailAsync(MailTo, Topic, Body, Attachments);

                // Eliminar el archivo PDF después de enviar el correo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("descargarFactura")]
        public IActionResult DescargarFactura(int numeroOrden, double precioTotal, double pagado)
        {

            // Ruta en la carpeta Assets/BillTemp dentro del proyecto
            string assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "BillTemp");

            // Verificar si la carpeta existe, si no, crearla
            if (!Directory.Exists(assetsPath))
            {
                Directory.CreateDirectory(assetsPath);
            }
            // Definir la ruta temporal para guardar el archivo
            string fileName = $"FacturaProyecto-17-Orden-{numeroOrden}.pdf";
            string filePath = Path.Combine(assetsPath, fileName);

            try
            {
                // Generar el PDF
                _pdfService.GenerarFacturaPdf(filePath, numeroOrden, precioTotal, pagado);

                // Leer el archivo como un stream
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileResult = File(fileStream, "application/pdf", fileName);

                // Eliminar el archivo después de enviarlo
                Response.OnCompleted(() =>
                {
                    fileStream.Dispose();
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    return Task.CompletedTask;
                });

                return fileResult;
            }
            catch
            {
                return StatusCode(500, "Error al generar o descargar la factura.");
            }
        }


    }

    public class OrdenTrabajadorPar
    {
        public int idOrden { get; set; }
        public int idTrabajador { get; set; }
    }
}
