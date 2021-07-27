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
    public class ModuloFormularioService : IModuloFormularioService
    {
        private readonly IModuloFormularioRepository _moduloFormularioRepository;
        private readonly IFormularioService _formularioService;
        private readonly IModuloService _moduloService;
        public ModuloFormularioService(
            IModuloFormularioRepository moduloFormularioRepository,
            IFormularioService formularioService,
            IModuloService moduloService)
        {
            _moduloFormularioRepository = moduloFormularioRepository;
            _formularioService = formularioService;
            _moduloService = moduloService;
        }

        #region ObterAsync
        public async Task<IEnumerable<ModuloFormulario>> ObterAsync()
        {
            try 
            {
                return await _moduloFormularioRepository.ObterAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<ModuloFormulario> ObterAsync(int id)
        {
            try
            {
                var moduloFormulario = await _moduloFormularioRepository.ObterAsync(id);
                if (moduloFormulario == null)
                    throw new ServiceException($"Modulo formulário com Id = {id} não foi encontrado");
                return moduloFormulario;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(ModuloFormulario moduloFormulario)
        {
            try
            {
                _moduloFormularioRepository.Insere(moduloFormulario);
                await _moduloFormularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, ModuloFormulario moduloFormulario)
        {
            try
            {
                if (id != moduloFormulario.FormularioId) throw new ServiceException(
                    $"Id informado {id} é diferente do Id do modulo formulario {moduloFormulario.Id}");
                
                if (moduloFormulario.CreatedSystem) throw new ServiceException(
                    $"O modulo formulário com Id = {id} foi criado pelo sistema. Alteração inválida!");

                _moduloFormularioRepository.Update(moduloFormulario);
                await _moduloFormularioRepository.UnitOfWork.SaveChangesAsync();
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
                var moduloFormulario = await ObterAsync(id);
                if (moduloFormulario == null) throw new ServiceException(
                    $"O modulo formulário com Id = {id} não foi encontrado");

                if (moduloFormulario.CreatedSystem) throw new ServiceException(
                    $"O modulo formulário com Id = {id} foi criado pelo sistema. Exclusão inválida!");

                _moduloFormularioRepository.Remove(moduloFormulario);
                await _moduloFormularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterFormulariosAsync 
        public async Task<ResultResponse> ObterFormulariosAsync(int moduloId)
        {
            try
            {
                var modulo = await _moduloService.ObterAsync(moduloId);

                var formularios = await _formularioService.ObterAsync();

                foreach (var formulario in formularios)
                {
                    var mf = modulo.ModulosFormulario.FirstOrDefault(mf => mf.ModuloId == moduloId && mf.FormularioId == formulario.FormularioId);
                    if (mf == null)
                    {
                        formulario.Selected = false;
                    }
                    else
                    {
                        formulario.Selected = true;
                    }
                }

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = formularios,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region AtualizarFormulariosAsync
        public async Task<ResultResponse> AtualizarFormulariosAsync(int moduloId, List<Formulario> formularios)
        {
            try
            {
                var modulo = await _moduloService.ObterAsync(moduloId);

                if (modulo.CreatedSystem) throw new ServiceException(
                    $"O módulo {modulo.Nome} foi criado pelo sistema. Operação inválida!");

                foreach (var formulario in formularios)
                {
                    var mf = modulo.ModulosFormulario.FirstOrDefault(mf => mf.ModuloId == moduloId && mf.FormularioId == formulario.FormularioId);
                    if (mf == null)
                    {
                        if (formulario.Selected)
                        {
                            mf = (new ModuloFormulario()
                            {
                                ModuloId = moduloId,
                                FormularioId = formulario.FormularioId
                            });
                            await InsereAsync(mf);
                        }
                    }
                    else
                    {
                        if (!formulario.Selected)
                        {
                            await RemoveAsync(mf.Id);
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
