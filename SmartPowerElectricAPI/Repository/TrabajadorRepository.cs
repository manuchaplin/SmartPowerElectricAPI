using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class TrabajadorRepository : GenericRepository<Trabajador>, ITrabajadorRepository
    {
        public TrabajadorRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
