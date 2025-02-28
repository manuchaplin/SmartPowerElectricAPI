using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SendDocumentJob
{
    public class Program
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task Main(string[] args)  // Hacer Main async para poder usar await
        {
            await SendDocumentExpirationAsync();
        }

        public static async Task SendDocumentExpirationAsync()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs.txt");

            try
            {
                File.AppendAllText(filePath, $"Hora actual: {DateTime.Now}" + Environment.NewLine);

                var url = "https://smartpowerelectric.azurewebsites.net/api/DocumentoCaducar/sendEmailDocumentExpiration";

                // Realizar la petición HTTP
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    File.AppendAllText(filePath, "Solicitud exitosa." + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText(filePath, $"Error en la solicitud: {response.StatusCode}" + Environment.NewLine);
                }
            }
            catch (HttpRequestException ex)
            {
                File.AppendAllText(filePath, $"HttpRequestException: {ex.Message} - {DateTime.Now}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                File.AppendAllText(filePath, $"Exception: {ex.Message} - {DateTime.Now}" + Environment.NewLine);
            }
        }
    }
}
