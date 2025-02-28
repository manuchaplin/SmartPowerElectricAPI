using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
    {
        public FacturaRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
