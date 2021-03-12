using Microsoft.EntityFrameworkCore;
using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Repositories;
using Seguranca.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Core.Services
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
            try { return await _perfilRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Perfil> ObterAsync(int perfilId)
        {
            try
            {
                var evento = await _perfilRepository.ObterAsync(perfilId);
                if (evento == null)
                    throw new ServiceException("Perfil não cadastrado - " + perfilId);
                return evento;
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
                if (perfis.Contains(perfil))
                    throw new ServiceException("Perfil já cadastrado - " + perfil.Nome);

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
                if (perfilId != perfil.PerfilId)
                { throw new ServiceException(perfilId + " Diferente " + perfil.PerfilId); }

                _perfilRepository.Update(perfil);
                try { await _perfilRepository.UnitOfWork.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_perfilRepository.Exists(perfilId))
                    {
                        throw new ServiceException("Perfil não encontrado - " + perfilId);
                    }
                    throw;
                }
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
                if (perfil == null)
                    throw new ServiceException("Perfil não cadastrado - " + perfilId);

                _perfilRepository.Remove(perfil);
                await _perfilRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
