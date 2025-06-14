﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Service;
using Org.BouncyCastle.Asn1.X500;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                ordenSearch.Materials = ordenSearch.Materials.Where(x => x.Eliminado != true && x.FechaEliminado == null).ToList();
                ordenSearch.Facturas = ordenSearch.Facturas.Where(x => x.Eliminado != true && x.FechaEliminado == null).ToList();

                if (ordenSearch == null) return NotFound();

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(ordenSearch);


                #region Validate
                if (facturaDTO.MontoACobrar > ordenDTO.FaltanteCobrar)
                {
                    return Conflict(new { message = "El monto a cobrar es superior al faltante por cobrar." });
                }

                if ((ordenDTO.facturaDTOs.Where(x=>x.FacturaCompletada!=true).Sum(x=>x.MontoACobrar)+facturaDTO.MontoACobrar)>ordenDTO.FaltanteCobrar)
                {
                    return Conflict(new { message = "La sumatoria de las facturas por cobrar es superior al faltante por cobrar." });
                }
                #endregion



                Factura factura= facturaDTO.ToEntity();
                factura.NumeroFactura="F"+ordenDTO.Id+DateTime.Now.Year + DateTime.Now.Month+DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                factura.IdOrden = idOrden;
                factura.FacturaCompletada = false;
                factura.EmailEnviado = false;                ;
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
          
                Orden ordenSearch = _ordenRepository.GetByID(facturaSearch.IdOrden, "Materials,Facturas");               
              

                if (ordenSearch == null) return NotFound();

                ordenSearch.Materials = ordenSearch.Materials.Where(x => x.Eliminado != true && x.FechaEliminado == null).ToList();
                ordenSearch.Facturas = ordenSearch.Facturas.Where(x => x.Eliminado != true && x.FechaEliminado == null).ToList();
                OrdenDTO ordenDTO = OrdenDTO.FromEntity(ordenSearch);


                if (facturaSearch != null)
                {
                    if (facturaDTO.MontoACobrar != null)
                    {
                        
                        if (facturaDTO.MontoACobrar > ordenDTO.FaltanteCobrar)
                        {
                            return Conflict(new { message = "El monto a cobrar es superior al faltante por cobrar." });
                        }                      

                        if ((ordenDTO.facturaDTOs.Where(x => x.FacturaCompletada != true).Sum(x => x.MontoACobrar) + facturaDTO.MontoACobrar) > ordenDTO.FaltanteCobrar)
                        {
                            return Conflict(new { message = "La sumatoria de las facturas por cobrar es superior al faltante por cobrar." });
                        }

                        facturaSearch.MontoACobrar = facturaDTO.MontoACobrar ?? 0; 
                    }
                    if (facturaDTO.EmailEnviado != null) facturaSearch.EmailEnviado = facturaDTO.EmailEnviado;
                    if (facturaDTO.FacturaCompletada != null) 
                    {
                        if (facturaSearch.FacturaCompletada!=true && facturaDTO.FacturaCompletada==true && ordenDTO.FaltanteCobrar >= facturaSearch.MontoACobrar)
                        {
                            ordenSearch.Cobrado += facturaSearch.MontoACobrar;
                            if (ordenSearch.Cobrado==ordenDTO.CosteTotal)
                            {
                                ordenSearch.OrdenFinalizada = true;
                            }
                            _ordenRepository.Update(ordenSearch);
                            facturaSearch.FacturaCompletada = facturaDTO.FacturaCompletada;
                        }else if (ordenDTO.FaltanteCobrar < facturaSearch.MontoACobrar)
                        {
                            return Conflict(new { message = "El Faltante a cobrar es menor que la factura que quiere cobrar" });
                        }                        
                    }                                   
                    if (facturaDTO.IdOrden != null) facturaSearch.IdOrden = (int)facturaDTO.IdOrden;                   
                    if (facturaDTO.Descripcion != null) facturaSearch.Descripcion = facturaDTO.Descripcion;                   
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

        [HttpGet("sendInvoice/{idFactura}")]
        public async Task<IActionResult> sendInvoice(int idFactura)
        {
            try
            {
                Factura factura = _facturaRepository.GetByID(idFactura);
                //Orden orden = _ordenRepository.GetByID(factura.IdOrden, "Materials,Proyecto");
                Orden orden = _context.Orden.Where(x=>x.Id==factura.IdOrden).Include(x=>x.Materials).Include(x=>x.Proyecto).ThenInclude(x=>x.Cliente).FirstOrDefault();               
                orden.Materials.ToList().RemoveAll(material => material.Eliminado == true);
               


                if (factura == null || orden==null)
                {
                    return NotFound();
                }

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(orden);
                FacturaDTO facturaDTO = FacturaDTO.FromEntity(factura);
                ProyectoDTO proyectoDTO = ProyectoDTO.FromEntity(orden.Proyecto);
                ClienteDTO clienteDTO = ClienteDTO.FromEntity(orden.Proyecto.Cliente);

                // Datos del correo
                string MailTo = orden.Proyecto.Cliente.Email;
                string Topic = " Invoice No. " + facturaDTO.NumeroFactura+" - Project "+ ordenDTO.NombreProyecto;
                string Body = "<div>";
                Body += "<p>Dear "+ orden.Proyecto.Cliente.Nombre + "</p>";
                Body += "<p>I hope you are doing well. Please find attached the invoice corresponding to "+ordenDTO.NombreProyecto+", with the invoice number "+ facturaDTO.NumeroFactura + ".</p>";
                Body += "<p>Please let me know if you need any additional information or documentation. If you have already made the payment or if this invoice has been processed, please disregard this message and accept my thanks.</p>";
                Body += "<p>Thank you for you business. Don´t hesitate to reach out if you have any questions.</p>";
                Body += "</br>";
                Body += "<p>Best Regards,</p>";
                Body += "<p><bold>Smart Power Electric.</bold></p>";

                Body += "</div>";

                // Generar factura (PDF)
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "BillTemp", $"{facturaDTO.NumeroFactura}.pdf");
                _pdfService.GenerarFacturaPdf(filePath, facturaDTO, ordenDTO,proyectoDTO,clienteDTO);

                // Enviar correo con el archivo adjunto
                List<string> Attachments = new List<string> { filePath };

                await _emailService.SendMailAsync(MailTo, Topic, Body, Attachments);

                factura.EmailEnviado= true;
                _facturaRepository.Update(factura);
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
                //Orden orden = _ordenRepository.GetByID(factura.IdOrden, "Materials,Proyecto");
                Orden orden = _context.Orden.Where(x => x.Id == factura.IdOrden).Include(x => x.Materials).Include(x => x.Proyecto).ThenInclude(x => x.Cliente).FirstOrDefault();
                orden.Materials.ToList().RemoveAll(material => material.Eliminado == true);

                if (factura == null || orden == null)
                {
                    return NotFound();
                }

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(orden);
                FacturaDTO facturaDTO = FacturaDTO.FromEntity(factura);
                ProyectoDTO proyectoDTO = ProyectoDTO.FromEntity(orden.Proyecto);
                ClienteDTO clienteDTO = ClienteDTO.FromEntity(orden.Proyecto.Cliente);

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
                _pdfService.GenerarFacturaPdf(filePath, facturaDTO, ordenDTO, proyectoDTO, clienteDTO);

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
