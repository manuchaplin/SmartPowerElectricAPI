using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class TipoMaterialRepository : GenericRepository<TipoMaterial>, ITipoMaterialRepository
    {
        public TipoMaterialRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
