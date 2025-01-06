using SmartPowerElectricAPI.Models;

namespace SmartPowerElectricAPI.Repository
{
    public class UsuarioRepository :GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository (SmartPowerElectricContext context):base(context) { }
    }
}
