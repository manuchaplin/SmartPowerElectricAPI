namespace SmartPowerElectricAPI.Service
{
    public class FileService
    {
        public List<string> GetFilesFromBillTempDirectory()
        {
            // Obtiene la ruta completa a la carpeta "Assets/BillTemp"
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "BillTemp");

            // Verifica si el directorio existe
            if (Directory.Exists(directoryPath))
            {
                // Obtiene todos los archivos en el directorio
                var files = Directory.GetFiles(directoryPath).ToList();

                return files;
            }
            else
            {
                throw new DirectoryNotFoundException($"El directorio {directoryPath} no existe.");
            }
        }
    }
}
