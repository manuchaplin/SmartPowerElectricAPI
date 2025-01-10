using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
