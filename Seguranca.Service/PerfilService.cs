using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Service
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;

        public PerfilService(IPerfilRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }
        
        #region ObterAsync
        public async Task<IEnumerable<Perfil>> ObterAsync()
        {
            try 
            {
                var perfis = await _perfilRepository.GetFullAsync();
                return perfis.Where(p => p.Nome != "Master").ToList();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Perfil> ObterAsync(int perfilId)
        {
            try
            {
                var perfil = await _perfilRepository.GetFullAsync(perfilId);
                if (perfil == null)
                    throw new ServiceException($"Perfil com Id = { perfilId } não foi encontrado");
                return perfil;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Perfil> ObterAsync(string nome)
        {
            try
            {
                var perfil = await _perfilRepository.GetFullAsync(nome);
                if (perfil == null)
                    throw new ServiceException($"Perfil com nome = { nome } não foi encontrado");
                return perfil;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Perfil perfil)
        {
            try
            {
                var perfis = await ObterAsync();
                if ((perfis.FirstOrDefault(p => p.Nome == perfil.Nome) != null))
                    throw new ServiceException($"Já existe perfil cadastrado com o nome: {perfil.Nome}");

                _perfilRepository.Insere(perfil);
                await _perfilRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int perfilId, Perfil perfil)
        {
            try
            {
                if (perfilId != perfil.PerfilId) throw new ServiceException(
                    $"Id informado {perfilId} é diferente do Id do perfil {perfil.PerfilId}");

                if (perfil.CreatedSystem) throw new ServiceException(
                    $"O perfil {perfil.Nome} foi criado pelo sistema. Alteração inválida!");

                _perfilRepository.Update(perfil);
                await _perfilRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int perfilId)
        {
            try
            {
                var perfil = await ObterAsync(perfilId);
                if (perfil == null) throw new ServiceException(
                    $"O perfil com Id = {perfilId} não foi encontrado");

                if (perfil.CreatedSystem) throw new ServiceException(
                    $"O perfil {perfil.Nome} foi criado pelo sistema. Exclusão inválida!");

                if (perfil.PerfisUsuario.Count != 0 || perfil.RestricoesPerfil.Count != 0) 
                    throw new ServiceException(
                        $"O perfil {perfil.Nome} está em uso. Exclusão inválida!");
                
                _perfilRepository.Remove(perfil);
                await _perfilRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        
    }    
}
