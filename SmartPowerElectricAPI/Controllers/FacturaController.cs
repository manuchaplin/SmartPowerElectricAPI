using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SmartPowerElectricAPI.DTO;
using System.Data.Entity;
using SmartPowerElectricAPI.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FacturaController : ControllerBase
    {
        private IFacturaRepository _facturaRepository;
        private readonly IMaterialRepository _materialRepository;
        private IOrdenRepository _ordenRepository;
        private readonly EmailService _emailService;
        private readonly FileService _fileService;
        private readonly PDFService _pdfService;
        private readonly SmartPowerElectricContext _context;
        public FacturaController(IFacturaRepository facturaRepository,IMaterialRepository materialRepository, IOrdenRepository ordenRepository,EmailService emailService,FileService fileService, PDFService pdfService, SmartPowerElectricContext context)
        {
            _facturaRepository = facturaRepository;
            _materialRepository = materialRepository;
            _ordenRepository = ordenRepository;
            _emailService=emailService;
            _fileService=fileService;
            _pdfService=pdfService;
            _context = context;
        }

        [HttpPost("create/{idOrden}")]
        public IActionResult Create(int idOrden,[FromBody] FacturaDTO facturaDTO)
        {
            try
            {
                
                List<Expression<Func<Orden, bool>>> where = new List<Expression<Func<Orden, bool>>>();
                where.Add(x => x.Id== idOrden);
                
                Orden ordenSearch = _ordenRepository.Get(where, "Materials,Facturas").FirstOrDefault();

                if (ordenSearch == null) return NotFound();

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(ordenSearch);


                #region Validate
                if (facturaDTO.MontoACobrar > ordenDTO.FaltanteCobrar)
                {
                    return BadRequest(new { message = "El monto a cobrar es superior al faltante por cobrar." });
                }

                if ((ordenDTO.facturaDTOs.Where(x=>x.FacturaCompletada==false && x.Eliminado!=true && x.FechaEliminado==null).Sum(x=>x.MontoACobrar)+facturaDTO.MontoACobrar)>ordenDTO.FaltanteCobrar)
                {
                    return BadRequest(new { message = "La sumatoria de las facturas por cobrar es superior al faltante por cobrar." });
                }
                #endregion



                Factura factura= facturaDTO.ToEntity();
                factura.NumeroFactura="F"+ordenDTO.Id+DateTime.Now.Year + DateTime.Now.Month+DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                factura.IdOrden = idOrden;
                factura.FacturaCompletada = false;
                factura.EmailEnviado = false;
                factura.FechaCreacion = DateTime.Now;
                _facturaRepository.Insert(factura);

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
                List<Expression<Func<Factura, bool>>> where = new List<Expression<Func<Factura, bool>>>();
                where.Add(x => x.Id == id);
                where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
                Factura factura = _facturaRepository.Get(where).FirstOrDefault();
                if (factura != null)
                {
                    if (factura.FacturaCompletada!=true)
                    {
                        factura.FechaEliminado = DateTime.Now;
                        factura.Eliminado = true;
                        _facturaRepository.Update(factura);
                    }                   

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
        public IActionResult Edit(int id,[FromBody] FacturaDTO facturaDTO)
        {
            try
            {
                List<Expression<Func<Factura, bool>>> where = new List<Expression<Func<Factura, bool>>>();
                where.Add(x => x.Id == id);
                Factura facturaSearch = _facturaRepository.Get(where).FirstOrDefault();

                List<Expression<Func<Orden, bool>>> whereOrden = new List<Expression<Func<Orden, bool>>>();
                where.Add(x => x.Id == facturaSearch.IdOrden);
                Orden ordenSearch = _ordenRepository.Get(whereOrden, "Materials,Facturas").FirstOrDefault();

                if (ordenSearch == null) return NotFound();

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(ordenSearch);


                if (facturaSearch != null)
                {
                    if (facturaDTO.MontoACobrar != null)
                    {
                        
                        if (facturaDTO.MontoACobrar > ordenDTO.FaltanteCobrar)
                        {
                            return BadRequest(new { message = "El monto a cobrar es superior al faltante por cobrar." });
                        }

                        if ((ordenDTO.facturaDTOs.Where(x => x.FacturaCompletada == false && x.Eliminado != true && x.FechaEliminado == null).Sum(x => x.MontoACobrar) + facturaDTO.MontoACobrar) > ordenDTO.FaltanteCobrar)
                        {
                            return BadRequest(new { message = "La sumatoria de las facturas por cobrar es superior al faltante por cobrar." });
                        }

                        facturaSearch.MontoACobrar = facturaDTO.MontoACobrar ?? 0; 
                    }
                    if (facturaDTO.EmailEnviado != null) facturaSearch.EmailEnviado = facturaDTO.EmailEnviado;
                    if (facturaDTO.FacturaCompletada != null) 
                    {
                        if (facturaSearch.FacturaCompletada!=true && facturaDTO.FacturaCompletada==true)
                        {
                            ordenSearch.Cobrado += facturaSearch.MontoACobrar;
                            if (ordenSearch.Cobrado==ordenDTO.CosteTotal)
                            {
                                ordenSearch.OrdenFinalizada = true;
                            }
                            _ordenRepository.Update(ordenSearch);
                        }

                        facturaSearch.FacturaCompletada = facturaDTO.FacturaCompletada; 
                    }                                   
                    if (facturaDTO.IdOrden != null) facturaSearch.IdOrden = (int)facturaDTO.IdOrden;                   
                    if (facturaDTO.FechaCreacion != null) facturaSearch.FechaCreacion = string.IsNullOrWhiteSpace(facturaDTO.FechaCreacion) ? null : DateTime.ParseExact(facturaDTO.FechaCreacion, "yyyy-MM-dd", null);

                    _facturaRepository.Update(facturaSearch);

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

        [HttpPost("sendInvoice/{idFactura}")]
        public async Task<IActionResult> sendInvoice(int idFactura)
        {
            try
            {
                Factura factura = _facturaRepository.GetByID(idFactura);
                Orden orden = _ordenRepository.GetByID(factura.IdOrden, "Materials,Proyecto");

                if (factura == null || orden==null)
                {
                    return NotFound();
                }

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(orden);
                FacturaDTO facturaDTO = FacturaDTO.FromEntity(factura);

                // Datos del correo
                string MailTo = "manuchaplin@gmail.com";
                string Topic = "Factura";
                string Body = "<div>";
                Body += "<p>Buenos días</p>";
                Body += "<p>Factura número "+ facturaDTO.NumeroFactura + " correspondiente al Proyecto "+ ordenDTO.NombreProyecto+"</p>";
                Body += "</div>";

                // Generar factura (PDF)
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "BillTemp", $"{facturaDTO.NumeroFactura}.pdf");
                _pdfService.GenerarFacturaPdf(filePath, facturaDTO, ordenDTO);

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


        [HttpGet("downloadInvoice/{idFactura}")]
        public IActionResult downloadInvoice(int idFactura)
        {
            try
            {
                Factura factura = _facturaRepository.GetByID(idFactura);
                Orden orden = _ordenRepository.GetByID(factura.IdOrden, "Materials,Proyecto");

                if (factura == null || orden == null)
                {
                    return NotFound();
                }

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(orden);
                FacturaDTO facturaDTO = FacturaDTO.FromEntity(factura);

                // Ruta en la carpeta Assets/BillTemp dentro del proyecto
                string assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "BillTemp");

                // Verificar si la carpeta existe, si no, crearla
                if (!Directory.Exists(assetsPath))
                {
                    Directory.CreateDirectory(assetsPath);
                }
                // Definir la ruta temporal para guardar el archivo
                string fileName = $"{facturaDTO.NumeroFactura}.pdf";
                string filePath = Path.Combine(assetsPath, fileName);


                // Generar el PDF
                _pdfService.GenerarFacturaPdf(filePath, facturaDTO, ordenDTO);

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

                //return Ok(new {fileResult=fileResult});
                return fileResult;
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
