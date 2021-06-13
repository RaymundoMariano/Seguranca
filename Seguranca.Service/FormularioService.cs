using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Service
{
    public class FormularioService : IFormularioService
    {
        private readonly IFormularioRepository _formularioRepository;
        private readonly IFormularioEventoRepository _formularioEventoRepository;
        private readonly IEventoRepository _eventoRepository;

        public FormularioService(
            IFormularioRepository formularioRepository,
            IFormularioEventoRepository formularioEventoRepository,
            IEventoRepository eventoRepository)
        {
            _formularioRepository = formularioRepository;
            _formularioEventoRepository = formularioEventoRepository;
            _eventoRepository = eventoRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Formulario>> ObterAsync()
        {
            try { return await _formularioRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Formulario> ObterAsync(int formularioId)
        {
            try
            {
                var formulario = await _formularioRepository.ObterAsyncFull(formularioId);
                if (formulario == null)
                    throw new ServiceException($"Formulário com Id = {formularioId} não foi encontrado");
                return formulario;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Formulario formulario)
        {
            try
            {
                var formularios = await ObterAsync();
                if (formularios.Contains(formulario))
                    throw new ServiceException("Formulario já cadastrado - " + formulario.Nome);

                _formularioRepository.Insere(formulario);
                await _formularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int formularioId, Formulario formulario)
        {
            try
            {
                if (formularioId != formulario.FormularioId)
                    throw new ServiceException(formularioId + " Diferente " + formulario.FormularioId);

                _formularioRepository.Update(formulario);
                await _formularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int formularioId)
        {
            try
            {
                var formulario = await ObterAsync(formularioId);
                if (formulario == null)
                    throw new ServiceException($"Formulário com Id = {formularioId} não foi encontrado");
                if (formulario.CreatedSystem)
                    throw new ServiceException($"Formulário com Id = {formularioId} foi criado pelo sistema");
                _formularioRepository.Remove(formulario);
                await _formularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterEventosAsync 
        public async Task<ResultResponse> ObterEventosAsync(int formularioId)
        {
            var result = GetFormulario(formularioId).Result;
            if (!result.Succeeded) return result;
            var formulario = (Formulario)result.ObjectRetorno;

            var eventos = await _eventoRepository.ObterAsync();

            foreach (var evento in eventos)
            {
                var fv = formulario.FormularioEvento.FirstOrDefault(fv => fv.FormularioId == formularioId && fv.EventoId == evento.EventoId);
                if (fv == null)
                {
                    evento.Selected = false;
                }
                else
                {
                    evento.Selected = true;
                }
            }

            return (new ResultResponse()
            {
                Succeeded = true,
                ObjectRetorno = eventos,
                ObjectResult = (int)EObjectResult.OK,
                Errors = new List<string>()
            });
        }
        #endregion

        #region AtualizarEventosAsync
        public async Task<ResultResponse> AtualizarEventosAsync(int formularioId, List<Evento> eventos)
        {
            var result = GetFormulario(formularioId).Result;
            if (!result.Succeeded) return result;
            var formulario = (Formulario)result.ObjectRetorno;

            foreach (var evento in eventos)
            {
                var fv = formulario.FormularioEvento.FirstOrDefault(fv => fv.FormularioId == formularioId && fv.EventoId == evento.EventoId);
                if (fv == null)
                {
                    if (evento.Selected)
                    {
                        fv = (new FormularioEvento()
                        {
                            FormularioId = formularioId,
                            EventoId = evento.EventoId
                        });
                        _formularioEventoRepository.Insere(fv);
                    }
                }
                else
                {             
                    if (!evento.Selected)
                    {
                        _formularioEventoRepository.Remove(fv);
                    }
                }
            }
            await _formularioEventoRepository.UnitOfWork.SaveChangesAsync();

            return (new ResultResponse()
            {
                Succeeded = true,
                ObjectRetorno = null,
                ObjectResult = (int)EObjectResult.OK,
                Errors = new List<string>()
            });
        }
        #endregion

        #region GetFormulario
        private async Task<ResultResponse> GetFormulario(int formularioId)
        {
            var formulario = await _formularioRepository.ObterAsyncFull(formularioId);
            if (formulario == null)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.NotFound,
                    Errors = new List<string>() { $"Formulário com Id = {formularioId} não foi encontrado" }
                });
            }
            else
            {
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = formulario,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
        }
        #endregion
    }
}
