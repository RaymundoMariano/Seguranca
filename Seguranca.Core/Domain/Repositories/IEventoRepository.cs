using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Repositories
{
    public interface IEventoRepository : IRepository, IUnitOfWork
    {
        Task<IEnumerable<Evento>> ObterAsync();
        Task<Evento> ObterAsync(int eventoId);
        void Insere (Evento evento);
        void Update(Evento evento);
        void Remove(Evento evento);
        bool Exists(int eventoId);
    }
}
