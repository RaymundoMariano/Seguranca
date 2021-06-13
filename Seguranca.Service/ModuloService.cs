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
    public class ModuloService : IModuloService
    {
        private readonly IModuloRepository _moduloRepository;
        private readonly IModuloFormularioRepository _moduloFormularioRepository;
        private readonly IFormularioRepository _formularioRepository;

        public ModuloService(
            IModuloRepository moduloRepository,
            IModuloFormularioRepository moduloFormularioRepository,
            IFormularioRepository formularioRepository)
        {
            _moduloRepository = moduloRepository;
            _moduloFormularioRepository = moduloFormularioRepository;
            _formularioRepository = formularioRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Modulo>> ObterAsync()
        {
            try { return await _moduloRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Modulo> ObterAsync(int moduloId)
        {
            try
            {
                var modulo = await _moduloRepository.ObterAsyncFull(moduloId);
                if (modulo == null)
                    throw new ServiceException($"Módulo com Id = {moduloId} não foi encontrado");
                return modulo;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Modulo modulo)
        {
            try
            {
                var modulos = await ObterAsync();
                if (modulos.Contains(modulo))
                    throw new ServiceException("Modulo já cadastrado - " + modulo.Nome);

                _moduloRepository.Insere(modulo);
                await _moduloRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int moduloId, Modulo modulo)
        {
            try
            {
                if (moduloId != modulo.ModuloId)
                { throw new ServiceException(moduloId + " Diferente " + modulo.ModuloId); }

                _moduloRepository.Update(modulo);
                await _moduloRepository.UnitOfWork.SaveChangesAsync();
                
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int moduloId)
        {
            try
            {
                var modulo = await ObterAsync(moduloId);
                if (modulo == null)
                    throw new ServiceException($"Módulo com Id = {moduloId} não foi encontrado");
                if (modulo.CreatedSystem)
                    throw new ServiceException($"Módulo com Id = {moduloId} foi criado pelo sistema");
                _moduloRepository.Remove(modulo);
                await _moduloRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterFormulariosAsync 
        public async Task<ResultResponse> ObterFormulariosAsync(int moduloId)
        {
            var result = GetModulo(moduloId).Result;
            if (!result.Succeeded) return result;
            var modulo = (Modulo)result.ObjectRetorno;

            var formularios = await _formularioRepository.ObterAsync();

            foreach (var formulario in formularios)
            {
                var mf = modulo.ModuloFormulario.FirstOrDefault(mf => mf.ModuloId == moduloId && mf.FormularioId == formulario.FormularioId);
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
        #endregion

        #region AtualizarFormulariosAsync
        public async Task<ResultResponse> AtualizarFormulariosAsync(int moduloId, List<Formulario> formularios)
        {
            var result = GetModulo(moduloId).Result;
            if (!result.Succeeded) return result;
            var modulo = (Modulo)result.ObjectRetorno;

            foreach (var formulario in formularios)
            {
                var mf = modulo.ModuloFormulario.FirstOrDefault(mf => mf.ModuloId == moduloId && mf.FormularioId == formulario.FormularioId);
                if (mf == null)
                {
                    if (formulario.Selected)
                    {
                        mf = (new ModuloFormulario()
                        {
                            ModuloId = moduloId,
                            FormularioId = formulario.FormularioId
                        });
                        _moduloFormularioRepository.Insere(mf);
                    }
                }
                else
                {
                    if (!formulario.Selected)
                    {
                        _moduloFormularioRepository.Remove(mf);
                    }
                }
            }
            await _moduloFormularioRepository.UnitOfWork.SaveChangesAsync();

            return (new ResultResponse()
            {
                Succeeded = true,
                ObjectRetorno = null,
                ObjectResult = (int)EObjectResult.OK,
                Errors = new List<string>()
            });
        }
        #endregion

        #region GetModulo
        private async Task<ResultResponse> GetModulo(int moduloId)
        {
            var modulo = await _moduloRepository.ObterAsyncFull(moduloId);
            if (modulo == null)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.NotFound,
                    Errors = new List<string>() { $"Módulo com Id = {moduloId} não foi encontrado" }
                });
            }
            else
            {
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = modulo,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
        }
        #endregion
    }
}
