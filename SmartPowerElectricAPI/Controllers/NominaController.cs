using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartPowerElectricAPI.DTO;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.Repository;
using SmartPowerElectricAPI.Service;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartPowerElectricAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NominaController : ControllerBase
    {


        private readonly INominaRepository _nominaRepository;
        private readonly ITrabajadorRepository _trabajadorRepository;
        private readonly PDFService _pdfService;
        private readonly EmailService _emailService;
        public NominaController(INominaRepository nominaRepository, ITrabajadorRepository trabajadorRepository, PDFService pdfService, EmailService emailService)
        {
            _nominaRepository = nominaRepository;
            _trabajadorRepository = trabajadorRepository;
            _pdfService = pdfService;
            _emailService = emailService;
        }

        [HttpPost("create/{idTrabajador}")]
        public IActionResult Create(int idTrabajador, [FromBody] NominaDTO nominaDTO)
        {

            try
            {
                List<Expression<Func<Trabajador, bool>>> where = new List<Expression<Func<Trabajador, bool>>>();
                where.Add(x => x.Id == idTrabajador);
                Trabajador trabajadorSearch = _trabajadorRepository.Get(where).FirstOrDefault();

                if (trabajadorSearch != null)
                {
                    Nomina nomina = nominaDTO.ToEntity();
                    nomina.IdTrabajador = idTrabajador;
                    nomina.SalarioEstandar = nomina.horasTrabajadas * trabajadorSearch.CobroxHora;
                    nomina.FechaCreacion = DateTime.Now;
                    nomina.Anyo = (int)nomina.FinSemana?.Year;
                    _nominaRepository.Insert(nomina);

                    return Ok();
                }
                else
                {
                    return Conflict(new { message = "El trabajador no existe" });
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
                List<Expression<Func<Nomina, bool>>> where = new List<Expression<Func<Nomina, bool>>>();
                where.Add(x => x.Id == id);
                Nomina nomina = _nominaRepository.Get(where).FirstOrDefault();
                if (nomina != null)
                {
                    _nominaRepository.Delete(nomina);

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
        public IActionResult Edit(int id, [FromBody] NominaDTO nominaDTO)
        {
            try
            {
                List<Expression<Func<Nomina, bool>>> where = new List<Expression<Func<Nomina, bool>>>();
                where.Add(x => x.Id == id);
                Nomina nominaSearch = _nominaRepository.Get(where, "Trabajador").FirstOrDefault();

                if (nominaSearch != null)
                {
                    if (nominaDTO.horasTrabajadas != null) nominaSearch.horasTrabajadas = nominaDTO.horasTrabajadas ?? 0;
                    if (nominaDTO.horasTrabajadas != null) nominaSearch.SalarioEstandar = nominaDTO.horasTrabajadas * nominaSearch.Trabajador.CobroxHora;
                    if (nominaDTO.SalarioPlus != null) nominaSearch.SalarioPlus = nominaDTO.SalarioPlus;
                    if (nominaDTO.FechaPago != null) nominaSearch.FechaPago = string.IsNullOrWhiteSpace(nominaDTO.FechaPago) ? null : DateTime.ParseExact(nominaDTO.FechaPago, "yyyy-MM-dd", null);
                    if (nominaDTO.SemanaCompleta != null && nominaDTO.NoSemana!=null)
                    {
                        var DivisionSemana = nominaDTO.SemanaCompleta.Split('/');
                        nominaSearch.NoSemana = (int)nominaDTO.NoSemana;
                        nominaSearch.InicioSemana = DateTime.ParseExact(DivisionSemana[0], "yyyy-MM-dd", null);
                        nominaSearch.FinSemana = DateTime.ParseExact(DivisionSemana[1], "yyyy-MM-dd", null);
                        nominaSearch.Anyo = (int)nominaSearch.FinSemana?.Year;
                    }
                    if (nominaDTO.FechaCreacion != null) nominaSearch.FechaCreacion = string.IsNullOrWhiteSpace(nominaDTO.FechaCreacion) ? null : DateTime.ParseExact(nominaDTO.FechaCreacion, "yyyy-MM-dd", null);

                    _nominaRepository.Update(nominaSearch);

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

        [HttpGet("listByAnyoTrabajador/{idTrabajador}/{anyo}")]
        public IActionResult listByAnyoTrabajador(int idTrabajador,int anyo)
        {
            try
            {
                List<Nomina> nominas = new List<Nomina>();
              
                List<Expression<Func<Nomina, bool>>> where = new List<Expression<Func<Nomina, bool>>>();
                where.Add(x => x.IdTrabajador == idTrabajador && x.Anyo==anyo);

                nominas = _nominaRepository.Get(where).ToList();
                
                if (nominas.Count()>0 )
                {                                       
                    List<NominaDTO> nominaDTOs = new List<NominaDTO>();
                    nominaDTOs = nominas.Select(NominaDTO.FromEntity).OrderBy(x => x.NoSemana).ToList();
                    return Ok(nominaDTOs);
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

        [HttpGet("ListWeeksOfYear/{anyo}")]
        public IActionResult ListWeekOfYear(int anyo)
        {
            List<(int Semana, DateTime FechaInicio, DateTime FechaFin)> semanas = new List<(int, DateTime, DateTime)>();
            DateTime primerDia = new DateTime(anyo, 1, 1);
            DateTime ultimoDia = new DateTime(anyo, 12, 31);
            CultureInfo cultura = CultureInfo.CurrentCulture;

            // Obtener la primera semana del año según ISO 8601
            int semanaActual = cultura.Calendar.GetWeekOfYear(primerDia, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // Encontrar el primer lunes de la semana 1
            DateTime inicioSemana = primerDia.AddDays(-(int)primerDia.DayOfWeek + (int)DayOfWeek.Monday);
            if (inicioSemana.Year > anyo)
            {
                inicioSemana = inicioSemana.AddDays(-7); // Ajustar si el primer lunes pertenece al año siguiente
            }
            else if (inicioSemana.Year < anyo && cultura.Calendar.GetWeekOfYear(ultimoDia.AddDays(-3), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == 1)
            {
                inicioSemana = inicioSemana.AddDays(7); // Ajustar si la última semana del año anterior se considera la primera del año actual
            }

            // Iterar sobre las semanas
            while (inicioSemana.Year <= anyo)
            {
                DateTime finSemana = inicioSemana.AddDays(6);
                if (finSemana.Year > anyo) finSemana = ultimoDia; // Ajuste para la última semana del año

                semanas.Add((semanaActual, inicioSemana, finSemana));

                inicioSemana = inicioSemana.AddDays(7);
                semanaActual = cultura.Calendar.GetWeekOfYear(inicioSemana, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }

            
            List<RangoFecha> semanasString = new List<RangoFecha>();
            //List<(string key,string value)> semanasString = new List<(string,string)>();

            foreach (var semana in semanas)
            {
                RangoFecha rangoFecha = new RangoFecha();
                rangoFecha.NoSemana = semana.Semana;
                rangoFecha.rangoFechaIngles = semana.FechaInicio.ToString("yyyy-dd-MM") + "/" + semana.FechaFin.ToString("yyyy-dd-MM");
                rangoFecha.SemanaCompleta = semana.FechaInicio.ToString("yyyy-MM-dd") + "/" + semana.FechaFin.ToString("yyyy-MM-dd");
                semanasString.Add(rangoFecha);
             

            }

            return Ok(semanasString);
        }


        [HttpGet("downloadYTD/{idNomina}")]
        public IActionResult downloadInvoice(int idNomina)
        {
            try
            {
                Nomina nomina = _nominaRepository.GetByID(idNomina);

                Trabajador trabajador = _trabajadorRepository.GetByID(nomina.IdTrabajador);

                if (nomina == null || trabajador == null)
                {
                    return NotFound();
                }
                List<Nomina> nominas = new List<Nomina>();
                List<NominaDTO> nominasDTOs = new List<NominaDTO>();
                List<Expression<Func<Nomina, bool>>> where = new List<Expression<Func<Nomina, bool>>>();
                where.Add(x => x.IdTrabajador == nomina.IdTrabajador);
                where.Add(x => x.Anyo == nomina.Anyo);
                where.Add(x => x.NoSemana <= nomina.NoSemana);

                nominas = _nominaRepository.Get(where).ToList();
                nominasDTOs = nominas.Select(NominaDTO.FromEntity).ToList();
                double YTD = (double)nominasDTOs.Sum(x => x.SalarioTotal);



                NominaDTO nominaDTO = NominaDTO.FromEntity(nomina);
                TrabajadorDTO trabajadorDTO = TrabajadorDTO.FromEntity(trabajador);


                // Ruta en la carpeta Assets/BillTemp dentro del proyecto
                string assetsPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "NominaTemp");

                // Verificar si la carpeta existe, si no, crearla
                if (!Directory.Exists(assetsPath))
                {
                    Directory.CreateDirectory(assetsPath);
                }
                // Definir la ruta temporal para guardar el archivo
                string fileName = $"PayStub {trabajadorDTO.Nombre + " " + trabajadorDTO.Apellido + " " + nominaDTO.NoSemana + "-" + nominaDTO.Anyo}.pdf";
                string filePath = Path.Combine(assetsPath, fileName);


                // Generar el PDF
                _pdfService.GenerarNominaPdf(filePath, nominaDTO, trabajadorDTO, YTD);

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
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("sendYTD/{idNomina}")]
        public async Task<IActionResult> sendYTD(int idNomina)
        {
            try
            {
                Nomina nomina = _nominaRepository.GetByID(idNomina);

                Trabajador trabajador = _trabajadorRepository.GetByID(nomina.IdTrabajador);

                if (nomina == null || trabajador == null)
                {
                    return NotFound();
                }
                List<Nomina> nominas = new List<Nomina>();
                List<NominaDTO> nominasDTOs = new List<NominaDTO>();
                List<Expression<Func<Nomina, bool>>> where = new List<Expression<Func<Nomina, bool>>>();
                where.Add(x => x.IdTrabajador == nomina.IdTrabajador);
                where.Add(x => x.Anyo == nomina.Anyo);
                where.Add(x => x.NoSemana <= nomina.NoSemana);

                nominas = _nominaRepository.Get(where).ToList();
                nominasDTOs = nominas.Select(NominaDTO.FromEntity).ToList();
                double YTD = (double)nominasDTOs.Sum(x => x.SalarioTotal);



                NominaDTO nominaDTO = NominaDTO.FromEntity(nomina);
                TrabajadorDTO trabajadorDTO = TrabajadorDTO.FromEntity(trabajador);

                // Datos del correo
                string MailTo = "manuchaplin@gmail.com";
                //string MailTo = trabajadorDTO.Email;
                string Topic = " PayStub No. " + trabajadorDTO.Nombre + " " + trabajadorDTO.Apellido + " " + nominaDTO.Anyo + nominaDTO.NoSemana;
                string Body = "<div>";
                Body += "<p>Dear " + trabajadorDTO.Nombre + " " + trabajadorDTO.Apellido + "</p>";
                Body += "<p>Attached to this email, you will find the paystub corresponding to the number " + nominaDTO.NoSemana + "/" + nominaDTO.Anyo + ".</p>";
                Body += "<p>We remain at your disposal for any further clarification.</p>";
                Body += "</br>";
                Body += "<p>Atentamente,</p>";
                Body += "<p><bold>Smart Power Electric.</bold></p>";

                Body += "</div>";

                // Generar factura (PDF)
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "NominaTemp", $"PayStub {trabajadorDTO.Nombre + " " + trabajadorDTO.Apellido + " " + nominaDTO.NoSemana + "-" + nominaDTO.Anyo}.pdf");
                _pdfService.GenerarNominaPdf(filePath, nominaDTO, trabajadorDTO, YTD);

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
    }

    //public class NominaTrabajador
    //{
    //    public int idTrabajador { get; set; }
    //    public int anyo { get; set; }
    //}

    public class RangoFecha
    {
        public int NoSemana { get; set; }
        public string rangoFechaIngles { get; set; }
        public string SemanaCompleta { get; set; }
    }
}
