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
        private readonly IPerfilUsuarioService _perfilUsuarioService;
        private readonly IModuloService _moduloSevice;
        private readonly IPerfilService _perfilSevice;
        public UsuarioService(
            IUsuarioRepository usuarioRepository, 
            IPerfilUsuarioService perfilUsuarioService,
            IModuloService moduloService,
            IPerfilService perfilService)
        {
            _usuarioRepository = usuarioRepository;
            _perfilUsuarioService = perfilUsuarioService;
            _moduloSevice = moduloService;
            _perfilSevice = perfilService;
        }

        #region GetFullAsync
        public async Task<Usuario> GetFullAsync(string nome)
        {
            try
            {
                var usuario = await _usuarioRepository.GetFullAsync(nome);
                if (usuario == null)
                    throw new ServiceException($"Usuário { nome } não foi encontrado");
                return usuario;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterAsync
        public async Task<IEnumerable<Usuario>> ObterAsync()
        {
            try
            {
                return await _usuarioRepository.GetFullAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Usuario> ObterAsync(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioRepository.GetFullAsync(usuarioId);
                if (usuario == null)
                    throw new ServiceException($"Usuário com Id = {usuarioId} não foi encontrado");
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
                if ((usuarios.FirstOrDefault(u => u.Nome == usuario.Nome) != null)) throw new ServiceException(
                    $"Já existe usuário cadastrado com o nome {usuario.Nome}");

                if (!Validacao.EmailValido(usuario.Email)) throw new ServiceException(
                    $"Email inválido: {usuario.Email}");

                var user = await ObterAsync(usuario.Email);
                if (user != null) throw new ServiceException(
                    $"Já existe usuário cadastrado com este email: {usuario.Email}");

                _usuarioRepository.Insere(usuario);
                await _usuarioRepository.UnitOfWork.SaveChangesAsync();

                var pu = new PerfilUsuario()
                {
                    UsuarioId = usuario.UsuarioId,
                    ModuloId = 1,
                    PerfilId = (await _perfilSevice.ObterAsync("Sem Perfil")).PerfilId
                };
                if (usuario.UsuarioId == 1)
                {
                    pu.PerfilId = (await _perfilSevice.ObterAsync("Master")).PerfilId;
                    pu.CreatedSystem = true;
                }
                await _perfilUsuarioService.InsereAsync(pu);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterIds
        public async Task<Seguranca.Domain.SegurancaModel> ObterIds(string nomeUsuario, string nomeModulo)
        {
            var usuario = await GetFullAsync(nomeUsuario);
            var modulo = await _moduloSevice.ObterAsync(nomeModulo);
            var perfil = await _perfilSevice.ObterAsync("Sem Perfil");

            var pu = usuario.PerfisUsuario.FirstOrDefault(pu =>
                pu.UsuarioId == usuario.UsuarioId && pu.ModuloId == modulo.ModuloId);
            if (pu == null)
            {
                pu = new PerfilUsuario() { PerfilId = perfil.PerfilId };
            }

            return new Domain.SegurancaModel()
            {
                UsuarioId = usuario.UsuarioId, ModuloId = modulo.ModuloId, PerfilId = (int)pu.PerfilId
            };
        }
        #endregion
    }
}
