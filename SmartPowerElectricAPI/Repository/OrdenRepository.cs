using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class OrdenRepository : GenericRepository<Orden>, IOrdenRepository
    {
        public OrdenRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
