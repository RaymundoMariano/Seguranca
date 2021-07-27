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
    public class FormularioEventoService : IFormularioEventoService
    {
        private readonly IFormularioEventoRepository _formularioEventoRepository;
        private readonly IFormularioService _formularioService;
        private readonly IEventoService _eventoService;
        public FormularioEventoService(
            IFormularioEventoRepository formularioEventoRepository,
            IFormularioService formularioService,
            IEventoService eventoService)
        {
            _formularioEventoRepository = formularioEventoRepository;
            _formularioService = formularioService;
            _eventoService = eventoService;
        }

        #region ObterAsync
        public async Task<IEnumerable<FormularioEvento>> ObterAsync()
        {
            try 
            { 
                return await _formularioEventoRepository.ObterAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<FormularioEvento> ObterAsync(int id)
        {
            try
            {
                var formularioEvento = await _formularioEventoRepository.ObterAsync(id);
                if (formularioEvento == null) throw new ServiceException(
                    $"Formulário evento com Id = {id} não foi encontrado");

                return formularioEvento;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(FormularioEvento formularioEvento)
        {
            try
            {
                _formularioEventoRepository.Insere(formularioEvento);
                await _formularioEventoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, FormularioEvento formularioEvento)
        {
            try
            {
                if (id != formularioEvento.EventoId) throw new ServiceException(
                    $"Id informado {id} é diferente do Id do formulário evento {formularioEvento.EventoId}");

                if (formularioEvento.CreatedSystem) throw new ServiceException(
                    $"O formulário evento com Id = {id} foi criado pelo sistema. Alteração inválida");

                _formularioEventoRepository.Update(formularioEvento);
                await _formularioEventoRepository.UnitOfWork.SaveChangesAsync();
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
                var formularioEvento = await ObterAsync(id);
                if (formularioEvento == null) throw new ServiceException(
                    $"O formulário evento com Id = {id} não foi encontrado");

                if (formularioEvento.CreatedSystem) throw new ServiceException(
                    $"O formulário evento com Id = {id} foi criado pelo sistema. Exclusão inválida");

                _formularioEventoRepository.Remove(formularioEvento);
                await _formularioEventoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterEventosAsync 
        public async Task<ResultResponse> ObterEventosAsync(int formularioId)
        {
            try
            {
                var formulario = await _formularioService.ObterAsync(formularioId);

                var eventos = await _eventoService.ObterAsync();

                foreach (var evento in eventos)
                {
                    var fv = formulario.FormulariosEvento.FirstOrDefault(fv =>
                        fv.FormularioId == formularioId && fv.EventoId == evento.EventoId);
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
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region AtualizarEventosAsync
        public async Task<ResultResponse> AtualizarEventosAsync(int formularioId, List<Evento> eventos)
        {
            try
            {
                var formulario = await _formularioService.ObterAsync(formularioId);

                if (formulario.CreatedSystem) throw new ServiceException(
                    $"O formulário {formulario.Nome} foi criado pelo sistema. Operação inválida!");

                foreach (var evento in eventos)
                {
                    var fv = formulario.FormulariosEvento.FirstOrDefault(fv => fv.FormularioId == formularioId && fv.EventoId == evento.EventoId);
                    if (fv == null)
                    {
                        if (evento.Selected)
                        {
                            fv = (new FormularioEvento()
                            {
                                FormularioId = formularioId,
                                EventoId = evento.EventoId
                            });
                            await InsereAsync(fv);
                        }
                    }
                    else
                    {
                        if (!evento.Selected)
                        {
                            await RemoveAsync(fv.Id);
                        }
                    }
                }

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
