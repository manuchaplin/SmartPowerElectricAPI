using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(SmartPowerElectricContext context) : base(context) { }
    }
}
