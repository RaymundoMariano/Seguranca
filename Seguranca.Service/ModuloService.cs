using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Service
{
    public class ModuloService : IModuloService
    {
        private readonly IModuloRepository _moduloRepository;

        public ModuloService(IModuloRepository moduloRepository)
        {
            _moduloRepository = moduloRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Modulo>> ObterAsync()
        {
            try 
            { 
                return await _moduloRepository.GetFullAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Modulo> ObterAsync(int moduloId)
        {
            try
            {
                var modulo = await _moduloRepository.GetFullAsync(moduloId);
                if (modulo == null)
                    throw new ServiceException($"Módulo com Id = {moduloId} não foi encontrado");
                return modulo;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Modulo> ObterAsync(string nome)
        {
            try
            {
                var modulo = await _moduloRepository.GetFullAsync(nome);
                if (modulo == null)
                    throw new ServiceException($"O módulo {nome} não foi encontrado");
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
                if ((modulos.FirstOrDefault(m => m.Nome == modulo.Nome) != null))
                    throw new ServiceException($"Já existe módulo cadastrado com o nome: {modulo.Nome}");

                _moduloRepository.Insere(modulo);
                await _moduloRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int Id, Modulo modulo)
        {
            try
            {
                if (Id != modulo.ModuloId) throw new ServiceException(
                    $"Id informado é diferente do Id do módulo {modulo.ModuloId}");
                
                if (modulo.CreatedSystem) throw new ServiceException(
                    $"O módulo {modulo.Nome} foi criado pelo sistema. Alteração inválida!");

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
                if (modulo == null) throw new ServiceException(
                    $"O módulo com Id = {moduloId} não foi encontrado");

                if (modulo.CreatedSystem) throw new ServiceException(
                    $"O módulo {modulo.Nome} foi criado pelo sistema. Exclusão inválida!");

                if (modulo.PerfisUsuario.Count != 0 || modulo.ModulosFormulario.Count != 0 ||
                    modulo.RestricoesPerfil.Count != 0 || modulo.RestricoesUsuario.Count != 0)
                    throw new ServiceException(
                        $"O módulo {modulo.Nome} está em uso. Exclusão inválida!");

                _moduloRepository.Remove(modulo);
                await _moduloRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
