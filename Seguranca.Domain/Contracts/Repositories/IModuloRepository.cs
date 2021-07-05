using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IModuloRepository : IRepository<Modulo>
    {
        Task<IEnumerable<Modulo>> GetFullAsync();
        Task<Modulo> GetFullAsync(int id);
        Task<Modulo> GetFullAsync(string nome);

    }
}
