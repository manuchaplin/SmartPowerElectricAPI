using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class UnidadMedidumRepository : GenericRepository<UnidadMedida>, IUnidadMedidumRepository
    {
        public UnidadMedidumRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
