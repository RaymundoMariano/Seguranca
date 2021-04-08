using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task<Usuario> ObterAsync(string email);
    }
}
