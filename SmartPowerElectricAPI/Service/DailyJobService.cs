using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using SixLabors.Fonts;

namespace SmartPowerElectricAPI.Service
{
    public class DailyJobService : BackgroundService
    {
        private readonly ILogger<DailyJobService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public DailyJobService(ILogger<DailyJobService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "Logs.txt");               
                var now = DateTime.Now;
                //var nextRun = now.Date.AddHours(20).AddMinutes(44); // Hoy a las 10:00 AM
                var nextRun = now.Date.AddHours(12); // Hoy a las 10:00 AM

                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1); // Si ya pasó, agendar para mañana
                }

                var delay = nextRun - now;
                File.AppendAllText(filePath, $"Próxima ejecución programada en {nextRun}" + Environment.NewLine);
                _logger.LogInformation($"Próxima ejecución programada en {nextRun}");

                await Task.Delay(delay, stoppingToken); // Esperar hasta la hora programada

                await CallEndpoint(); // Llamar al controlador
            }
        }

        private async Task CallEndpoint()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "Logs.txt");
            try
            {
               
                var client = _httpClientFactory.CreateClient();
                var url = "https://smartpowerelectric.azurewebsites.net/api/DocumentoCaducar/sendEmailDocumentExpiration"; // Ajusta la URL de la API
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    File.AppendAllText(filePath, $"Job ejecutado correctamente.{DateTime.Now}" + Environment.NewLine);
                    _logger.LogInformation("Job ejecutado correctamente.");
                }
                else
                {
                    File.AppendAllText(filePath, $"Error en la ejecución del job: {response.StatusCode}{DateTime.Now}" + Environment.NewLine);
                    _logger.LogError($"Error en la ejecución del job: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(filePath, $"Error al llamar la API: {ex.Message}{DateTime.Now}" + Environment.NewLine);
                _logger.LogError($"Error al llamar la API: {ex.Message}");
            }
        }
    }
}
