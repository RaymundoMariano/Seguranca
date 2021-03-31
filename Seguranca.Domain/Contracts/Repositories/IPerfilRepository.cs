using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
        Task<IEnumerable<Perfil>> ObterAsyncFull();
        Task<Perfil> ObterAsyncFull(int id);
    }
}
