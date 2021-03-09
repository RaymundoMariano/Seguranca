using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Repositories;
using Seguranca.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Core.Persistence.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly SegurancaContext _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;

        public EventoRepository(SegurancaContext segurancaContext) 
        {
            _segurancaContext = segurancaContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Evento>> ObterAsync()
        {
            return await _segurancaContext.Evento
                    .AsNoTracking()
                    .Include(e => e.FormularioEvento)
                    .Include(e => e.RestricaoPerfil)
                    .Include(e => e.RestricaoUsuario).ToListAsync();
         }

        public async Task<Evento> ObterAsync(int eventoId)
        {
            return await _segurancaContext.Evento
                    .AsNoTracking()
                    .Include(e => e.FormularioEvento)
                    .Include(e => e.RestricaoPerfil)
                    .Include(e => e.RestricaoUsuario)
                    .FirstAsync(e => e.EventoId == eventoId);
        }
        #endregion

        #region Insere
        public void Insere(Evento evento)
        {
            _segurancaContext.Evento.Add(evento);
        }
        #endregion

        #region Update
        public void Update(Evento evento)
        {
            _segurancaContext.Entry(evento).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(Evento evento)
        {
            _segurancaContext.Evento.Remove(evento);
        }
        #endregion

        #region Exists
        public bool Exists(int eventoId)
        {
            return _segurancaContext.Evento.Any(e => e.EventoId == eventoId);
        }
        #endregion
    }
}
