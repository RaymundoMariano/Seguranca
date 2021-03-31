using Seguranca.Domain.Entities;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IUsuarioService : IService<Usuario>
    {
        Usuario Obter(string email);
    }
}
