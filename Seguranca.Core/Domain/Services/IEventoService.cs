using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Services
{
    public interface IEventoService
    {
        Task<IEnumerable<Evento>> ObterAsync();
        Task<Evento> ObterAsync(int eventoId);
        Task InsereAsync(Evento evento);
        Task UpdateAsync(int eventoId, Evento evento);
        Task RemoveAsync(int eventoId);
    }
}
