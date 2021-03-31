using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Service
{
    public class FormularioService : IFormularioService
    {
        private readonly IFormularioRepository _formularioRepository;

        public FormularioService(IFormularioRepository formularioRepository)
        {
            _formularioRepository = formularioRepository;
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
                var evento = await _formularioRepository.ObterAsync(formularioId);
                if (evento == null)
                    throw new ServiceException("Formulario não cadastrado - " + formularioId);
                return evento;
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
                    throw new ServiceException("Formulario não cadastrado - " + formularioId);

                _formularioRepository.Remove(formulario);
                await _formularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
