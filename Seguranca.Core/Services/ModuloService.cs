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
            try { return await _moduloRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Modulo> ObterAsync(int moduloId)
        {
            try
            {
                var modulo = await _moduloRepository.ObterAsync(moduloId);
                if (modulo == null)
                    throw new ServiceException("Modulo não cadastrado - " + moduloId);
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
                try { await _moduloRepository.UnitOfWork.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_moduloRepository.Exists(moduloId))
                    {
                        throw new ServiceException("Modulo não encontrado - " + moduloId);
                    }
                    throw;
                }
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
                    throw new ServiceException("Modulo não cadastrado - " + moduloId);

                _moduloRepository.Remove(modulo);
                await _moduloRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
