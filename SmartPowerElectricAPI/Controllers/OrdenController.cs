using System;
using System.Collections.Generic;
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
        private readonly IWebHostEnvironment _env;    
        public OrdenController(IOrdenRepository ordenRepository,ITrabajadorRepository trabajadorRepository, IMaterialRepository materialRepository, EmailService emailService,FileService fileService, IWebHostEnvironment env)
        {
            _ordenRepository = ordenRepository;
            _trabajadorRepository = trabajadorRepository;
            _materialRepository = materialRepository;
            _emailService = emailService;
            _fileService = fileService;
            _env = env;        
        }

        [HttpPost("create/{idProyecto}")]
        public IActionResult Create(int idProyecto,[FromBody] OrdenDTO ordenDTO)
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

        [HttpPost("{idOrden},{idTrabajador}")]
        public IActionResult AddWorkerToOrder(int idOrden, int idTrabajador)
        {
            try
            {
                Orden orden = new Orden();
                orden = _ordenRepository.GetByID(idOrden, "Trabajadores");
                Trabajador trabajador = new Trabajador();
                trabajador = _trabajadorRepository.GetByID(idTrabajador);
                if (orden == null) return NotFound();
                if (trabajador == null) return NotFound();

                if (!orden.Trabajadores.Any(x=>x.Id==idTrabajador))
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
                Orden orden = new Orden();
                orden = _ordenRepository.GetByID(idOrden, "Trabajadores,Materials,Proyecto");

                if (orden==null)
                {
                    return NotFound();
                }
                string MailTo = "manuchaplin@gmail.com";
                string Topic = "Confirmación de Orden";
                string Body = "";

                Body += "<div>";
                Body += "<p>Buenos días</p>";
                Body += "<p>Factura de prueba</p>";
                Body += "</div>";


                List<string> Attachments= new List<string>();
                List<string> bills= _fileService.GetFilesFromBillTempDirectory();
                if (bills.Any())
                    foreach (string bill in bills) {
                        var split = bill.Split('.')[0].Split('-');
                        int idProyecto = int.Parse(split[1]);
                        int idOrd = int.Parse(split[3]);
                        if (idProyecto == orden.IdProyecto && idOrd == orden.Id)
                        {
                            Attachments.Add(bill);
                        }
                    }

                if (Attachments.Count() > 0)
                {
                    await _emailService.SendMailAsync(MailTo, Topic, Body, Attachments.Count() > 0 ? Attachments : null);
                }
                else
                {
                    return NotFound(new { message = "La factura del Proyecto: " + orden.Proyecto.Nombre + " y Orden " + orden.NumeroOrden + " no se ha encontrado." });
                }
              

                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
           
        }
    
    }
}
