using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class NominaRepository : GenericRepository<Nomina>, INominaRepository
    {
        public NominaRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
