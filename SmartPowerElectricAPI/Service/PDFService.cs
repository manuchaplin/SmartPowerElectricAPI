
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;


using PageSizeCore = PdfSharpCore.PageSize;

namespace SmartPowerElectricAPI.Service
{
    public class PDFService
    {
        public void GenerarFacturaPdf(string filePath, int numeroOrden, double precioTotal, double pagado)
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

            gfx.DrawString("Número de Orden: " + numeroOrden, font, XBrushes.Black, new XPoint(50, 80));
            gfx.DrawString("Precio Total: " + precioTotal.ToString("C"), font, XBrushes.Black, new XPoint(50, 120));
            gfx.DrawString("Pagado: " + pagado.ToString("C"), font, XBrushes.Black, new XPoint(50, 160));

            // Guardar el documento en el archivo especificado
            document.Save(filePath);
            document.Close();
        }
    }
}
