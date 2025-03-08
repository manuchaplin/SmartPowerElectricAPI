
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;


using PageSizeCore = PdfSharpCore.PageSize;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.DTO;
using System.Text.RegularExpressions;
using PdfSharpCore.Drawing.Layout;

namespace SmartPowerElectricAPI.Service
{
    public class PDFService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;


        public PDFService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void GenerarFacturaPdf(string filePath, FacturaDTO facturaDTO, OrdenDTO ordenDTO, ProyectoDTO proyectoDTO, ClienteDTO clienteDTO)
        {
            // Crear un nuevo documento PDF
            PdfDocument document = new PdfDocument();
            // Crear una página en el documento
            PdfPage page = document.AddPage();
            // Crear un objeto para dibujar sobre la página
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Definir una fuente para el texto
            XFont font = new XFont("Arial", 12);
            XFont boldFont = new XFont("Arial", 12, XFontStyle.Bold);
            XStringFormat formatRight = new XStringFormat { Alignment = XStringAlignment.Far };
            XTextFormatter tf = new XTextFormatter(gfx);

            // Dibujar el logo            
            string logoPath = System.IO.Path.Combine(_env.ContentRootPath, "Assets", "Img", "Logo.png");
            XImage logo = XImage.FromFile(logoPath);
            gfx.DrawImage(logo, 20, 60, 150, 50);

            // Encabezado
            gfx.DrawString("INVOICE", new XFont("Arial", 16, XFontStyle.Bold), XBrushes.Black, new XPoint(250, 50));

            // Información de la empresa
            gfx.DrawString("SMART POWER ELECTRIC", boldFont, XBrushes.Black, new XPoint(350, 90));
            gfx.DrawString("786-925-7180 (Spanish)", font, XBrushes.Black, new XPoint(350, 110));
            gfx.DrawString("786-816-2891 (English)", font, XBrushes.Black, new XPoint(350, 130));
            gfx.DrawString("Email: powerelectric12@gmail.com", font, XBrushes.Black, new XPoint(350, 150));
            gfx.DrawString("LIC: EC13014620", font, XBrushes.Black, new XPoint(350, 170));

            gfx.DrawString("Invoice Number: " + facturaDTO.NumeroFactura, font, XBrushes.Black, new XPoint(350, 190));
            gfx.DrawString("Invoice Date: " + facturaDTO.FechaCreacionEng, font, XBrushes.Black, new XPoint(350, 210));

            gfx.DrawString("BILL TO", boldFont, XBrushes.Black, new XPoint(50, 230));
            gfx.DrawString("Name: " + clienteDTO.Nombre, font, XBrushes.Black, new XPoint(50, 250));
            gfx.DrawString("Address: " + clienteDTO.Direccion, font, XBrushes.Black, new XPoint(50, 270));
            gfx.DrawString("Phone: " + clienteDTO.Telefono, font, XBrushes.Black, new XPoint(50, 290));
            gfx.DrawString("Email: " + clienteDTO.Email, font, XBrushes.Black, new XPoint(50, 310));
            gfx.DrawString("Project Name: " + ordenDTO.NombreProyecto, font, XBrushes.Black, new XPoint(50, 330));

            // Datos de la factura
            // Tabla de precios
            int tableStartY = 450;
            gfx.DrawString("Description", boldFont, XBrushes.Black, new XPoint(50, tableStartY));
            gfx.DrawString("Invoice Price", boldFont, XBrushes.Black, new XPoint(450, tableStartY));

            // Ajustar la posición y el tamaño del área de texto para la descripción
            string descripcion = facturaDTO.Descripcion ?? "";
            XRect rect = new XRect(50, tableStartY + 20, 400, 600); // Área de texto inicial
            int lineHeight = 15; // Altura de una línea de texto
            int maxLines = 40; // Número máximo de líneas por página (ajustar según necesidad)
            int totalHeight = maxLines * lineHeight; // Altura máxima de texto por página

            // Dividir la descripción en bloques y agregar nuevas páginas si es necesario
            for (int i = 0; i < descripcion.Length; i += maxLines * lineHeight)
            {
                string chunk = descripcion.Substring(i, Math.Min(maxLines * lineHeight, descripcion.Length - i));

                // Dibujar el fragmento de texto
                tf.DrawString(chunk, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                // Si el texto no cabe en la página, agregar una nueva página
                if (i + maxLines * lineHeight < descripcion.Length)
                {
                    page = document.AddPage(); // Agregar una nueva página
                    gfx = XGraphics.FromPdfPage(page); // Dibujar en la nueva página
                    rect = new XRect(50, 20, 400, 600); // Nueva área de texto para la siguiente página
                }
            }

            // Precio de la factura
            gfx.DrawString(facturaDTO.MontoACobrar?.ToString("C", new System.Globalization.CultureInfo("en-US")), font, XBrushes.Black, new XPoint(450, tableStartY + 20));

            // Guardar el documento en el archivo especificado
            document.Save(filePath);
            document.Close();
        }

        public void GenerarNominaPdf(string filePath, NominaDTO nominaDTO, TrabajadorDTO trabajadorDTO, double YTD)
        {
            // Crear un nuevo documento PDF
            PdfDocument document = new PdfDocument();
            // Crear una página en el documento
            PdfPage page = document.AddPage();
            // Crear un objeto para dibujar sobre la página
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // Definir una fuente para el texto
            XFont font = new XFont("Arial", 12);
            XFont boldFont = new XFont("Arial", 12, XFontStyle.Bold);
            XStringFormat formatRight = new XStringFormat { Alignment = XStringAlignment.Far };
            XTextFormatter tf = new XTextFormatter(gfx);

            // Dibujar el logo            
            string logoPath = System.IO.Path.Combine(_env.ContentRootPath, "Assets", "Img", "Logo.png");
            XImage logo = XImage.FromFile(logoPath);
            gfx.DrawImage(logo, 20, 60, 150, 50);

            // Encabezado
            gfx.DrawString("PayStub", new XFont("Arial", 16, XFontStyle.Bold), XBrushes.Black, new XPoint(250, 50));


            // Información de la empresa
            gfx.DrawString("SMART POWER ELECTRIC", boldFont, XBrushes.Black, new XPoint(350, 90));
            gfx.DrawString("786-925-7180 (Spanish)", boldFont, XBrushes.Black, new XPoint(350, 110));
            gfx.DrawString("786-816-2891 (English)", boldFont, XBrushes.Black, new XPoint(350, 130));
            gfx.DrawString("Email: powerelectric12@gmail.com", boldFont, XBrushes.Black, new XPoint(350, 150));
            gfx.DrawString("LIC: EC13014620", boldFont, XBrushes.Black, new XPoint(350, 170));


            DateTime? dateTimeStar = string.IsNullOrWhiteSpace(nominaDTO.InicioSemana) ? null : DateTime.ParseExact(nominaDTO.InicioSemana, "yyyy-MM-dd", null);
            DateTime? dateTimeEnd = string.IsNullOrWhiteSpace(nominaDTO.FinSemana) ? null : DateTime.ParseExact(nominaDTO.FinSemana, "yyyy-MM-dd", null);
            DateTime? dateTimePay = string.IsNullOrWhiteSpace(nominaDTO.FechaPago) ? null : DateTime.ParseExact(nominaDTO.FechaPago, "yyyy-MM-dd", null);
            gfx.DrawString("Period Starting: " + dateTimeStar?.ToString("yyyy-dd-MM"), font, XBrushes.Black, new XPoint(350, 190));
            gfx.DrawString("Period Ending: " + dateTimeEnd?.ToString("yyyy-dd-MM"), font, XBrushes.Black, new XPoint(350, 210));
            gfx.DrawString("Pay Date: " + dateTimePay?.ToString("yyyy-dd-MM"), font, XBrushes.Black, new XPoint(350, 230));

        
            gfx.DrawString("Name: " + trabajadorDTO.Nombre+" "+ trabajadorDTO.Apellido, font, XBrushes.Black, new XPoint(50, 250));
            gfx.DrawString("Address: " + trabajadorDTO.Direccion, font, XBrushes.Black, new XPoint(50, 270));
            gfx.DrawString("Phone: " + trabajadorDTO.Telefono, font, XBrushes.Black, new XPoint(50, 290));
            gfx.DrawString("Email: " + trabajadorDTO.Email, font, XBrushes.Black, new XPoint(50, 310));         
            gfx.DrawString("Social Security: XXXXXXXX" + trabajadorDTO.SeguridadSocial?.Substring(Math.Max(0, trabajadorDTO.SeguridadSocial.Length - 4)), font, XBrushes.Black, new XPoint(50, 330));         
            gfx.DrawString("Deposit Account Number: XXXXXXXX" + trabajadorDTO.NumeroCuenta?.Substring(Math.Max(0, trabajadorDTO.NumeroCuenta.Length - 4)), font, XBrushes.Black, new XPoint(50, 350));         


            // Datos de la factura
            // Tabla de precios
            int tableStartY = 450;
            gfx.DrawString("Rate", boldFont, XBrushes.Black, new XPoint(50, tableStartY));
            gfx.DrawString("Hours/units", boldFont, XBrushes.Black, new XPoint(150, tableStartY));
            gfx.DrawString("This Period", boldFont, XBrushes.Black, new XPoint(250, tableStartY));
            gfx.DrawString("Plus", boldFont, XBrushes.Black, new XPoint(350, tableStartY));
            gfx.DrawString("YTD", boldFont, XBrushes.Black, new XPoint(450, tableStartY));

            //gfx.DrawString(facturaDTO.Descripcion, font, XBrushes.Black, new XPoint(50, tableStartY + 20));
               
            gfx.DrawString(trabajadorDTO.CobroxHora?.ToString("C", new System.Globalization.CultureInfo("en-US")), font, XBrushes.Black, new XPoint(50, tableStartY + 20));
            gfx.DrawString(nominaDTO.horasTrabajadas?.ToString(), font, XBrushes.Black, new XPoint(150, tableStartY + 20));
            gfx.DrawString(nominaDTO.SalarioTotal?.ToString("C", new System.Globalization.CultureInfo("en-US")), font, XBrushes.Black, new XPoint(250, tableStartY + 20));
            gfx.DrawString(nominaDTO.SalarioPlus?.ToString("C", new System.Globalization.CultureInfo("en-US")), font, XBrushes.Black, new XPoint(350, tableStartY + 20));
            gfx.DrawString(YTD.ToString("C", new System.Globalization.CultureInfo("en-US")), font, XBrushes.Black, new XPoint(450, tableStartY + 20));



            // Guardar el documento en el archivo especificado
            document.Save(filePath);
            document.Close();
        }
    }
}
