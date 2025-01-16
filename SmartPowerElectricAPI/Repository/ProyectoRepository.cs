using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class ProyectoRepository : GenericRepository<Proyecto>, IProyectoRepository
    {
        public ProyectoRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
