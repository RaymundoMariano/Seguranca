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
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Usuario>> ObterAsync()
        {
            try { return await _usuarioRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Usuario> ObterAsync(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterAsync(usuarioId);
                if (usuario == null)
                    throw new ServiceException("Usuario não cadastrado - " + usuarioId);
                return usuario;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Usuario usuario)
        {
            try
            {
                var usuarios = await ObterAsync();
                if (usuarios.Contains(usuario))
                    throw new ServiceException("Usuario já cadastrado - " + usuario.Nome);

                _usuarioRepository.Insere(usuario);
                await _usuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int usuarioId, Usuario usuario)
        {
            try
            {
                if (usuarioId != usuario.UsuarioId)
                { throw new ServiceException(usuarioId + " Diferente " + usuario.UsuarioId); }

                _usuarioRepository.Update(usuario);
                try { await _usuarioRepository.UnitOfWork.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_usuarioRepository.Exists(usuarioId))
                    {
                        throw new ServiceException("Usuario não encontrado - " + usuarioId);
                    }
                    throw;
                }
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int usuarioId)
        {
            try
            {
                var usuario = await ObterAsync(usuarioId);
                if (usuario == null)
                    throw new ServiceException("Usuario não cadastrado - " + usuarioId);

                _usuarioRepository.Remove(usuario);
                await _usuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
