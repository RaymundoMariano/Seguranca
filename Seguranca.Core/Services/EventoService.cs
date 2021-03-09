using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Repositories;
using Seguranca.Core.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Seguranca.Core.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Evento>> ObterAsync()
        {
            try { return await _eventoRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Evento> ObterAsync(int eventoId)
        {
            try
            {
                var evento = await _eventoRepository.ObterAsync(eventoId);
                if (evento == null)
                    throw new ServiceException("Evento não cadastrado - " + eventoId);
                return evento;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Evento evento)
        {
            try
            {
                var eventos = await ObterAsync();
                if(eventos.Contains(evento))
                    throw new ServiceException("Evento já cadastrado - " + evento.Nome);

                _eventoRepository.Insere(evento);
                await _eventoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int eventoId, Evento evento)
        {
            try
            {
                if (eventoId != evento.EventoId) 
                    { throw new ServiceException(eventoId + " Diferente " + evento.EventoId); }

                _eventoRepository.Update(evento);
                try { await _eventoRepository.UnitOfWork.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_eventoRepository.Exists(eventoId))
                    {
                        throw new ServiceException("Evento não encontrado - " + eventoId);
                    }
                    throw;
                }
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int eventoId)
        {
            try
            {
                var evento = await ObterAsync(eventoId);
                if (evento == null)
                    throw new ServiceException("Evento não cadastrado - " + eventoId);

                _eventoRepository.Remove(evento);
                await _eventoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
