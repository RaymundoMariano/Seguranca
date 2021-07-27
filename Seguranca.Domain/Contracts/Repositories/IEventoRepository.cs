using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IEventoRepository : IRepository<Evento>
    {
        Task<IEnumerable<Evento>> GetFullAsync();
        Task<Evento> GetFullAsync(int id);
    }
}
