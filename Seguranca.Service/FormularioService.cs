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

        #region GetFullAsync
        public async Task<IEnumerable<Formulario>> GetFullAsync()
        {
            try 
            { 
                return await _formularioRepository.GetFullAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Formulario> GetFullAsync(int formularioId)
        {
            try
            {
                var formulario = await _formularioRepository.GetFullAsync(formularioId);
                if (formulario == null)
                    throw new ServiceException($"Formulário com Id = {formularioId} não foi encontrado");
                return formulario;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterAsync
        public async Task<IEnumerable<Formulario>> ObterAsync()
        {
            try 
            { 
                return await _formularioRepository.ObterAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Formulario> ObterAsync(int formularioId)
        {
            try
            {
                var formulario = await _formularioRepository.ObterAsync(formularioId);
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
                if ((formularios.FirstOrDefault(f => f.Nome == formulario.Nome) != null))
                    throw new ServiceException($"Já existe formulario cadastrado com o nome: {formulario.Nome}");

                _formularioRepository.Insere(formulario);
                await _formularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int Id, Formulario formulario)
        {
            try
            {
                if (Id != formulario.FormularioId) throw new ServiceException(
                    $"Id informado é diferente do Id deste formulário {formulario.FormularioId}");
                
                if (formulario.CreatedSystem) throw new ServiceException(
                    $"O formulário {formulario.Nome} foi criado pelo sistema. Alteação inválida!");

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
                if (formulario == null) throw new ServiceException(
                    $"O formulário com Id = {formularioId} não foi encontrado");

                if (formulario.CreatedSystem) throw new ServiceException(
                    $"O formulário {formulario.Nome} foi criado pelo sistema. Exclusão inválida!");

                _formularioRepository.Remove(formulario);
                await _formularioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
