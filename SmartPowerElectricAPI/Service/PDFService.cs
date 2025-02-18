
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;


using PageSizeCore = PdfSharpCore.PageSize;
using SmartPowerElectricAPI.Models;
using SmartPowerElectricAPI.DTO;

namespace SmartPowerElectricAPI.Service
{
    public class PDFService
    {
        public void GenerarFacturaPdf(string filePath, FacturaDTO facturaDTO,OrdenDTO ordenDTO)
        {
            // Crear un nuevo documento PDF
            PdfDocument document = new PdfDocument();

            // Crear una página en el documento
            PdfPage page = document.AddPage();

            // Crear un objeto para dibujar sobre la página
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Definir una fuente para el texto
            XFont font = new XFont("Arial", 12);

            // Escribir texto en la página
            gfx.DrawString("Factura", font, XBrushes.Black, new XPoint(200, 40));

            gfx.DrawString("Factura: " + facturaDTO.NumeroFactura, font, XBrushes.Black, new XPoint(50, 80));
            gfx.DrawString("Proyecto: " + ordenDTO.NombreProyecto, font, XBrushes.Black, new XPoint(50, 120));
            gfx.DrawString("Número de Orden: " + ordenDTO.NumeroOrden, font, XBrushes.Black, new XPoint(50, 160));
            gfx.DrawString("Precio Total: " + ordenDTO.CosteTotal?.ToString("C"), font, XBrushes.Black, new XPoint(50, 200));
            gfx.DrawString("Pagado: " + ordenDTO.Cobrado?.ToString("C"), font, XBrushes.Black, new XPoint(50, 240));
            gfx.DrawString("Faltante por cobrar: " + ordenDTO.FaltanteCobrar?.ToString("C"), font, XBrushes.Black, new XPoint(50, 280));
            gfx.DrawString("Importe de factura: " + facturaDTO.MontoACobrar?.ToString("C"), font, XBrushes.Black, new XPoint(50, 320));

            // Guardar el documento en el archivo especificado
            document.Save(filePath);
            document.Close();
        }
    }
}
