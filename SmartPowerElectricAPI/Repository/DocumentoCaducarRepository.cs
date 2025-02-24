using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class DocumentoCaducarRepository : GenericRepository<DocumentoCaducar>, IDocumentoCaducarRepository
    {
        public DocumentoCaducarRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
