using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class EventoRepositoryEFC : RepositoryEFC<Evento>, IEventoRepository
    {
        public EventoRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

        #region ObterAsyncFull
        public async Task<IEnumerable<Evento>> ObterAsyncFull()
        {
            return await _segurancaContext.Evento
                    .AsNoTracking()
                    .Include(e => e.FormularioEvento)
                    .Include(e => e.RestricaoPerfil)
                    .Include(e => e.RestricaoUsuario)
                    .ToListAsync();
        }

        public async Task<Evento> ObterAsyncFull(int eventoId)
        {
            return await _segurancaContext.Evento
                    .AsNoTracking()
                    .Include(e => e.FormularioEvento)
                    .Include(e => e.RestricaoPerfil)
                    .Include(e => e.RestricaoUsuario)
                    .FirstAsync(e => e.EventoId == eventoId);
        }
        #endregion
    }
}
