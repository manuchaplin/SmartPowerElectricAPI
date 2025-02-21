
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;


using PageSizeCore = PdfSharpCore.PageSize;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.DTO;
using System.Text.RegularExpressions;

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
        public void GenerarFacturaPdf(string filePath, FacturaDTO facturaDTO,OrdenDTO ordenDTO, ProyectoDTO proyectoDTO, ClienteDTO clienteDTO)
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

            // Dibujar el logo            
            string logoPath = System.IO.Path.Combine(_env.ContentRootPath, "Assets", "Img", "Logo.png");
            XImage logo = XImage.FromFile(logoPath);
            gfx.DrawImage(logo, 5, 1, 250, 250);

            // Encabezado
            gfx.DrawString("INVOICE", new XFont("Arial", 16, XFontStyle.Bold), XBrushes.Black, new XPoint(250, 50));
     

            // Información de la empresa
            gfx.DrawString("SMART POWER ELECTRIC", boldFont, XBrushes.Black, new XPoint(350, 90));
            gfx.DrawString("786-925-7180 (Spanish)", font, XBrushes.Black, new XPoint(350, 110));
            gfx.DrawString("786-816-2891 (English)", font, XBrushes.Black, new XPoint(350, 130));
            gfx.DrawString("Email: powerelectric22@gmail.com", font, XBrushes.Black, new XPoint(350, 150));
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
            gfx.DrawString("Invoice Price", boldFont, XBrushes.Black, new XPoint(50, tableStartY));
            gfx.DrawString("Paid", boldFont, XBrushes.Black, new XPoint(200, tableStartY));
            gfx.DrawString("Shortage to pay", boldFont, XBrushes.Black, new XPoint(350, tableStartY));
            gfx.DrawString("Total", boldFont, XBrushes.Black, new XPoint(500, tableStartY));

            gfx.DrawString(facturaDTO.MontoACobrar?.ToString("C"), font, XBrushes.Black, new XPoint(50, tableStartY + 20));
            gfx.DrawString(ordenDTO.Cobrado?.ToString("C"), font, XBrushes.Black, new XPoint(200, tableStartY + 20));
            gfx.DrawString(ordenDTO.FaltanteCobrar?.ToString("C"), font, XBrushes.Black, new XPoint(350, tableStartY + 20));
            gfx.DrawString(ordenDTO.CosteTotal?.ToString("C"), font, XBrushes.Black, new XPoint(500, tableStartY + 20));
          

            // Guardar el documento en el archivo especificado
            document.Save(filePath);
            document.Close();
        }
    }
}
