using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<IEnumerable<Usuario>> ObterAsyncFull();
        Task<Usuario> ObterAsyncFull(int id);
        Task<Usuario> ObterAsync(string email);
    }
}
