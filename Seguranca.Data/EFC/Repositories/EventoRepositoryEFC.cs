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

        #region GetFullAsync
        public async Task<IEnumerable<Evento>> GetFullAsync()
        {
            return await _segurancaContext.Eventos
                    .AsNoTracking()
                    .Include(e => e.FormulariosEvento)
                    .Include(e => e.RestricoesPerfil)
                    .Include(e => e.RestricoesUsuario)
                    .ToListAsync();
        }

        public async Task<Evento> GetFullAsync(int eventoId)
        {
            return await _segurancaContext.Eventos
                    .AsNoTracking()
                    .Include(e => e.FormulariosEvento)
                    .Include(e => e.RestricoesPerfil)
                    .Include(e => e.RestricoesUsuario)
                    .FirstOrDefaultAsync(e => e.EventoId == eventoId);
        }
        #endregion
    }
}
