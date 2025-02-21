using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using SmartPowerElectricAPI.Service;


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
        private IFacturaRepository _facturasRepository;
        private readonly SmartPowerElectricContext _context;
        private readonly EmailService _emailService;
        private readonly FileService _fileService;
        private readonly PDFService _pdfService;
        private readonly IWebHostEnvironment _env;
        public OrdenController(IOrdenRepository ordenRepository, ITrabajadorRepository trabajadorRepository, IMaterialRepository materialRepository, IFacturaRepository facturasRepository, SmartPowerElectricContext context ,EmailService emailService, FileService fileService, PDFService pdfService, IWebHostEnvironment env)
        {
            _ordenRepository = ordenRepository;
            _trabajadorRepository = trabajadorRepository;
            _materialRepository = materialRepository;
            _facturasRepository=facturasRepository;
            _context = context;
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
                orden.Ganancia = ordenDTO.Ganancia ?? 0;
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
                    if (ordenDTO.Ganancia != null) ordenSearch.Ganancia = ordenDTO.Ganancia;
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

        [HttpGet("{id}")]
        public  IActionResult GetById(int id)
        { 

            try
            {
                Orden orden = new Orden();
                //orden = _ordenRepository.GetByID(id, "Materials,Trabajadores");

                orden = _context.Orden.Where(x=>x.Id==id)
                               .Include(x => x.Materials)
                                   .ThenInclude(m => m.UnidadMedida)
                               .Include(x => x.Materials)
                                   .ThenInclude(m => m.TipoMaterial)
                               .Include(x => x.Trabajadores)
                               .Include(x=>x.Facturas).FirstOrDefault();
                orden.Materials = orden.Materials.Where(x => x.Eliminado != true && x.FechaEliminado == null).ToList();
                orden.Facturas = orden.Facturas.Where(x => x.Eliminado != true && x.FechaEliminado == null).ToList();

                if (orden == null) return NotFound();

                OrdenDTO ordenDTO = OrdenDTO.FromEntity(orden);
             

                return Ok(ordenDTO);
              

            }
            catch (Exception ex) {

                return BadRequest(new { message = ex.Message });
            }


        }


        //[HttpGet("listMaterials/{idOrden}")]
        //public IActionResult ListMaterials(int idOrden)
        //{
        //    try
        //    {
        //        List<Material> materials = new List<Material>();
        //        List<Expression<Func<Material, bool>>> where = new List<Expression<Func<Material, bool>>>();
        //        where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
        //        where.Add(x => x.IdOrden == idOrden);
        //        materials = _materialRepository.Get(where, "TipoMaterial,UnidadMedida").ToList();


        //        List<MaterialDTO> materialDTOs = materials.Select(MaterialDTO.FromEntity).ToList();

        //        return Ok(materialDTOs);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }

        //}

        //[HttpGet("listTrabajadores/{idOrden}")]
        //public IActionResult ListTrabajadores(int idOrden)
        //{
        //    try
        //    {
        //        Orden orden = new Orden();
        //        orden = _ordenRepository.GetByID(idOrden, "Trabajadores");

        //        List<TrabajadorDTO> trabajadorDTOs = orden.Trabajadores.Select(TrabajadorDTO.FromEntity).ToList();

        //        return Ok(trabajadorDTOs);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }

        //}
        //[HttpGet("listFacturas/{idOrden}")]
        //public IActionResult ListFacturas(int idOrden)
        //{
        //    try
        //    {
        //        List<Factura> facturas = new List<Factura>();
        //        List<Expression<Func<Factura, bool>>> where = new List<Expression<Func<Factura, bool>>>();
        //        where.Add(x => x.Eliminado != true && x.FechaEliminado == null);
        //        where.Add(x => x.IdOrden == idOrden);
        //        facturas = _facturasRepository.Get(where).ToList();


        //        List<FacturaDTO> facturaDTOs = facturas.Select(FacturaDTO.FromEntity).ToList();

        //        return Ok(facturaDTOs);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }

        //}

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
      

    }

    public class OrdenTrabajadorPar
    {
        public int idOrden { get; set; }
        public int idTrabajador { get; set; }
    }
}
