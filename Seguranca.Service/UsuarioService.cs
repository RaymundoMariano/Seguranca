using Acessorio.Util;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Service
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
            try 
            { 
                return await _usuarioRepository.ObterAsync(); 
            }
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

        public async Task<Usuario> ObterAsync(string email)
        {
            try
            {
                return await _usuarioRepository.ObterAsync(email);
            }
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

                if (!Validacao.EmailValido(usuario.Email))
                    throw new ServiceException("Email inválido - " + usuario.Email);

                var user = await ObterAsync(usuario.Email);
                if (user != null)
                    throw new ServiceException("Já existe usuário com esse email - " + usuario.Email);

                _usuarioRepository.Insere(usuario);
                await _usuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        
    }
}
