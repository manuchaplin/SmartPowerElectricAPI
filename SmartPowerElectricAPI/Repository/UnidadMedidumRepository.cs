using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class UnidadMedidumRepository : GenericRepository<UnidadMedidum>, IUnidadMedidumRepository
    {
        public UnidadMedidumRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
