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
    public class PerfilUsuarioService : IPerfilUsuarioService
    {
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;
        private readonly IPerfilService _perfilService;
        private readonly IModuloService _moduloService;
        public PerfilUsuarioService(
            IPerfilUsuarioRepository perfilUsuarioRepository,
            IPerfilService perfilService,
            IModuloService moduloService)
        {
            _perfilUsuarioRepository = perfilUsuarioRepository;
            _perfilService = perfilService;
            _moduloService = moduloService;
        }

        #region ObterAsync
        public async Task<IEnumerable<PerfilUsuario>> ObterAsync()
        {
            try 
            { 
                return await _perfilUsuarioRepository.ObterAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<PerfilUsuario> ObterAsync(int id)
        {
            try
            {
                var perfilUsuario = await _perfilUsuarioRepository.ObterAsync(id);
                if (perfilUsuario == null) throw new ServiceException(
                    $"O perfil de usuário com Id = {id} não foi encontrado");

                return perfilUsuario;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<PerfilUsuario> ObterAsync(int usuarioId, int? moduloId)
        {
            try
            {
                return await _perfilUsuarioRepository.ObterAsync(usuarioId, moduloId);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(PerfilUsuario perfilUsuario)
        {
            try
            {
                _perfilUsuarioRepository.Insere(perfilUsuario);
                await _perfilUsuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, PerfilUsuario perfilUsuario)
        {
            try
            {
                if (id != perfilUsuario.Id) throw new ServiceException(
                    $"O Id informado {id} é diferente do Id do perfil de usuário {perfilUsuario.Id}");

                if (perfilUsuario.CreatedSystem) throw new ServiceException(
                    $"O perfil de usuário com Id = {id} foi criado pelo sistema. Alteração inválida!");

                _perfilUsuarioRepository.Update(perfilUsuario);
                await _perfilUsuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int id)
        {
            try
            {
                var perfilUsuario = await ObterAsync(id);
                if (perfilUsuario == null) throw new ServiceException(
                    $"O perfil de usuário com Id = {id} não foi encontrado");

                if (perfilUsuario.CreatedSystem) throw new ServiceException(
                    $"O perfil de usuário com Id = {id} foi criado pelo sistema. Exclusão inválida!");

                _perfilUsuarioRepository.Remove(perfilUsuario);
                await _perfilUsuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterPerfisAsync 
        public async Task<ResultResponse> ObterPerfisAsync(int usuarioId)
        {
            try
            {
                var perfis = new List<PerfilUsuario>();

                foreach (var modulo in await _moduloService.ObterAsync())
                {
                    var pu = await _perfilUsuarioRepository.ObterAsync(usuarioId, modulo.ModuloId);
                    if (pu == null)
                    {
                        pu = new PerfilUsuario();
                        pu.UsuarioId = usuarioId;
                        pu.ModuloId = modulo.ModuloId;
                        pu.PerfilId = (await _perfilService.ObterAsync("Sem Perfil")).PerfilId;
                    }
                    pu.Modulo = modulo;
                    pu.Perfil = await _perfilService.ObterAsync((int)pu.PerfilId);
                    if (pu.Perfil.Nome == "Master") continue;
                    perfis.Add(pu);
                }

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = perfis,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region AtualizarPerfisAsync
        public async Task<ResultResponse> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuario> perfis)
        {
            try
            {
                foreach (var perfil in perfis)
                {
                    var pu = await ObterAsync(usuarioId, perfil.ModuloId);
                    if (pu == null)
                    {
                        pu = new PerfilUsuario();
                        pu.UsuarioId = usuarioId;
                        pu.ModuloId = perfil.ModuloId;
                        pu.PerfilId = (int)perfil.Perfil.PerfilId;

                        await InsereAsync(pu);
                    }
                    else
                    {
                        if (perfil.Perfil.PerfilId == pu.PerfilId)
                        {
                            continue;
                        }
                        else
                        {
                            if (pu.CreatedSystem)
                            {
                                continue;
                            }
                            pu.PerfilId = perfil.Perfil.PerfilId;
                            await UpdateAsync(pu.Id, pu);
                        }
                    }
                }

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        
    }
}
