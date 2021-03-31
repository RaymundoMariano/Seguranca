using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Service
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

        public async Task<Evento> ObterAsync(int id)
        {
            try
            {
                var evento = await _eventoRepository.ObterAsync(id);
                if (evento == null)
                    throw new ServiceException("Evento não cadastrado - " + id);
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
                if (eventos.Contains(evento))
                    throw new ServiceException("Evento já cadastrado - " + evento.Nome);

                _eventoRepository.Insere(evento);
                await _eventoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, Evento evento)
        {
            try
            {
                if (id != evento.EventoId)
                    throw new ServiceException(id + " Diferente " + evento.EventoId);

                _eventoRepository.Update(evento);
                await _eventoRepository.UnitOfWork.SaveChangesAsync();                
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int id)
        {
            try
            {
                var evento = await ObterAsync(id);
                if (evento == null)
                    throw new ServiceException("Evento não cadastrado - " + id);

                _eventoRepository.Remove(evento);
                await _eventoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
