using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
        Task<Perfil> GetFullAsync(string nome);
        Task<IEnumerable<Perfil>> GetFullAsync();
        Task<Perfil> GetFullAsync(int id);
    }
}
