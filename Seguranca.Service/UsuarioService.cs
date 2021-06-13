using Acessorio.Util;
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
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRestricaoUsuarioRepository _restricaoUsuarioRepository;
        private readonly IModuloRepository _moduloRepository;
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;
        private readonly IPerfilRepository _perfilRepository;
        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IRestricaoUsuarioRepository restricaoUsuarioRepository,
            IModuloRepository moduloRepository,
            IPerfilUsuarioRepository perfilUsuarioRepository,
            IPerfilRepository perfilRepository)
        {
            _usuarioRepository = usuarioRepository;
            _restricaoUsuarioRepository = restricaoUsuarioRepository;
            _moduloRepository = moduloRepository;
            _perfilUsuarioRepository = perfilUsuarioRepository;
            _perfilRepository = perfilRepository;
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
                var usuario = await _usuarioRepository.ObterAsyncFull(usuarioId);
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

        #region ObterRestricoesAsync 
        public async Task<ResultResponse> ObterRestricoesAsync(int usuarioId)
        {
            var result = GetUsuario(usuarioId).Result;
            if (!result.Succeeded) return result;
            var usuario = (Usuario)result.ObjectRetorno;
            
            var modulos = await _moduloRepository.ObterAsyncFull();

            var restricoes = new List<RestricaoUsuario>();

            foreach (var modulo in modulos)
            {
                var ru = await _restricaoUsuarioRepository.ObterAsync(usuarioId, modulo.ModuloId, null, null);
                if (ru != null)
                {
                    var restricao = await MontarRestricao(usuario, modulo, null, null);
                    restricoes.Add(restricao);
                    continue;
                }

                if (modulo.ModuloFormulario.Count == 0)
                {
                    var restricao = await MontarRestricao(usuario, modulo, null, null);
                    restricoes.Add(restricao);
                }
                else
                {
                    foreach (var mf in modulo.ModuloFormulario)
                    {
                        ru = await _restricaoUsuarioRepository.ObterAsync(usuarioId, mf.ModuloId, mf.FormularioId, null);
                        if (ru != null)
                        {
                            var restricao = await MontarRestricao(usuario, modulo, mf.Formulario, null);
                            restricoes.Add(restricao);
                            continue;
                        }

                        if (mf.Formulario.FormularioEvento.Count == 0)
                        {
                            var restricao = await MontarRestricao(usuario, modulo, mf.Formulario, null);
                            restricoes.Add(restricao);
                        }
                        else
                        {
                            foreach (var fe in mf.Formulario.FormularioEvento)
                            {
                                var restricao = await MontarRestricao(usuario, modulo, mf.Formulario, fe.Evento);
                                restricoes.Add(restricao);
                            }
                        }
                    }
                }
            }

            return (new ResultResponse()
            {
                Succeeded = true,
                ObjectRetorno = restricoes,
                ObjectResult = (int)EObjectResult.OK,
                Errors = new List<string>()
            });
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task<ResultResponse> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuario> restricoes)
        {
            foreach (var restricao in restricoes)
            {
                int? moduloId = (restricao.Modulo == null) ? null : restricao.ModuloId;
                int? formularioId = (restricao.Formulario == null) ? null : restricao.FormularioId;
                int? eventoId = (restricao.Evento == null) ? null : restricao.EventoId;

                var ru = await _restricaoUsuarioRepository.ObterAsync(restricao.Id);
                if (ru == null)
                {
                    if ((restricao.Formulario != null && restricao.Formulario.Selected) &&
                        (restricao.Evento != null && !restricao.Evento.Selected))
                    {
                        eventoId = null;
                    }

                    if ((restricao.Modulo != null && restricao.Modulo.Selected) &&
                        (restricao.Formulario != null && !restricao.Formulario.Selected))
                    {
                        formularioId = null;
                        eventoId = null;
                    }

                    if ((restricao.Modulo != null && restricao.Modulo.Selected) ||
                        (restricao.Formulario != null && restricao.Formulario.Selected) ||
                        (restricao.Evento != null && restricao.Evento.Selected))
                    {
                        var ruu = (new RestricaoUsuario()
                        {
                            UsuarioId = usuarioId,
                            ModuloId = moduloId,
                            FormularioId = formularioId,
                            EventoId = eventoId
                        });
                        _restricaoUsuarioRepository.Insere(ruu);
                    }
                }
                else
                {
                    if ((restricao.Modulo != null && restricao.Modulo.Selected) &&
                        (restricao.Formulario != null && restricao.Formulario.Selected) &&
                        (restricao.Evento != null && restricao.Evento.Selected))
                    {
                        continue;
                    }

                    if ((restricao.Modulo == null || !restricao.Modulo.Selected) &&
                        (restricao.Formulario == null || !restricao.Formulario.Selected) &&
                        (restricao.Evento == null || !restricao.Evento.Selected))
                    {
                        _restricaoUsuarioRepository.Remove(ru);
                    }
                    else
                    {
                        ru.UsuarioId = usuarioId;
                        ru.FormularioId = (restricao.Formulario == null || !restricao.Formulario.Selected) ? null : formularioId;
                        ru.EventoId = (restricao.Evento == null || !restricao.Evento.Selected) ? null : eventoId;

                        ru.ModuloId = moduloId;

                        if (restricao.Formulario != null && !restricao.Formulario.Selected)
                        {
                            ru.FormularioId = null;
                            ru.EventoId = null;
                        }

                        if (restricao.Evento != null && !restricao.Evento.Selected)
                        {
                            ru.EventoId = null;
                        }

                        if (!restricao.Modulo.Selected)
                        {
                            _restricaoUsuarioRepository.Remove(ru);
                        }
                        else
                        {
                            _restricaoUsuarioRepository.Update(ru);
                        }
                    }
                    await _restricaoUsuarioRepository.UnitOfWork.SaveChangesAsync();
                }
            }
            await _restricaoUsuarioRepository.UnitOfWork.SaveChangesAsync();

            await RemoveRestricoesExcedentes(usuarioId);

            return (new ResultResponse()
            {
                Succeeded = true,
                ObjectRetorno = null,
                ObjectResult = (int)EObjectResult.OK,
                Errors = new List<string>()
            });
        }
        #endregion        

        #region ObterPerfisAsync 
        public async Task<ResultResponse> ObterPerfisAsync(int usuarioId)
        {
            var modulos = await _moduloRepository.ObterAsync();

            var perfis = new List<PerfilUsuario>();

            foreach (var modulo in modulos)
            {
                var pu = await _perfilUsuarioRepository.ObterAsync(usuarioId, modulo.ModuloId);
                if (pu == null)
                {
                    pu = new PerfilUsuario();
                    pu.UsuarioId = usuarioId;
                    pu.ModuloId = modulo.ModuloId;
                    pu.PerfilId = 1;

                    pu.Modulo = modulo;
                }
                pu.Perfil = await _perfilRepository.ObterAsync((int)pu.PerfilId);
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
        #endregion

        #region AtualizarPerfisAsync
        public async Task<ResultResponse> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuario> perfis)
        {
            foreach (var perfil in perfis)
            {
                var pu = await _perfilUsuarioRepository.ObterAsync(usuarioId, perfil.ModuloId);
                if (pu == null)
                {
                    pu = new PerfilUsuario();
                    pu.UsuarioId = usuarioId;
                    pu.ModuloId = perfil.ModuloId;
                    pu.PerfilId = (int)perfil.Perfil.PerfilId;

                    _perfilUsuarioRepository.Insere(pu);
                }
                else
                {
                    if (perfil.Perfil.PerfilId == pu.PerfilId)
                    {
                        continue;
                    }
                    else
                    {
                        pu.PerfilId = perfil.Perfil.PerfilId;
                        _perfilUsuarioRepository.Update(pu);
                    }
                }
            }
            await _perfilUsuarioRepository.UnitOfWork.SaveChangesAsync();

            return (new ResultResponse()
            {
                Succeeded = true,
                ObjectRetorno = null,
                ObjectResult = (int)EObjectResult.OK,
                Errors = new List<string>()
            });
        }
        #endregion        

        #region GetUsuario
        private async Task<ResultResponse> GetUsuario(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObterAsyncFull(usuarioId);
            if (usuario == null)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.NotFound,
                    Errors = new List<string>() { $"Usuário com Id = {usuarioId} não foi encontrado" }
                });
            }
            else
            {
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = usuario,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
        }
        #endregion

        #region MontarRestricao
        private async Task<RestricaoUsuario> MontarRestricao(
            Usuario usuario, Modulo modulo, Formulario formulario, Evento evento)
        {
            int? moduloId = (modulo == null) ? null : modulo.ModuloId;
            int? formularioId = (formulario == null) ? null : formulario.FormularioId;
            int? eventoId = (evento == null) ? null : evento.EventoId;

            var ru = await _restricaoUsuarioRepository.ObterAsync(usuario.UsuarioId, moduloId, formularioId, eventoId);
            if (ru == null)
            {
                ru = new RestricaoUsuario();
                ru.UsuarioId = usuario.UsuarioId;
                ru.ModuloId = moduloId;
                ru.FormularioId = formularioId;
                ru.EventoId = eventoId;

                ru.Usuario = usuario;
                ru.Modulo = new Modulo();
                ru.Modulo.Nome = (modulo == null) ? null : modulo.Nome;
                ru.Modulo.Selected = false;

                ru.Formulario = (formulario == null) ? null : new Formulario();
                if (ru.Formulario != null)
                {
                    ru.Formulario.Nome = (formulario == null) ? null : formulario.Nome;
                    ru.Formulario.Selected = false;
                }

                ru.Evento = (evento == null) ? null : new Evento();
                if (ru.EventoId != null)
                {
                    ru.Evento.Nome = (evento == null) ? null : evento.Nome;
                    ru.Evento.Selected = false;
                }
            }
            else
            {
                ru.Usuario = usuario;

                ru.Modulo = (modulo == null) ? null : modulo;
                ru.Modulo.Selected = (modulo != null) ? true : false;

                ru.Formulario = (formulario == null) ? null : formulario;
                if (ru.Formulario != null)
                {
                    ru.Formulario.Selected = (formulario != null) ? true : false;
                }

                ru.Evento = (evento == null) ? null : evento;
                if (ru.Evento != null)
                {
                    ru.Evento.Selected = (evento != null) ? true : false;
                }
            }
            return ru;
        }
        #endregion

        #region RemoveRestricoesExcedentes
        private async Task RemoveRestricoesExcedentes(int usuarioId)
        {
            var restricoes = await _restricaoUsuarioRepository.ObterAsync();
            restricoes = restricoes.Where(r => r.UsuarioId == usuarioId);
            var orderByResult = from s in restricoes
                                orderby s.ModuloId, s.FormularioId, s.EventoId
                                select s;
            int? moduloId = null;
            int? formularioId = int.MaxValue;
            foreach (var restricao in orderByResult)
            {
                if (moduloId != restricao.ModuloId || formularioId != restricao.FormularioId)
                {
                    if (moduloId == restricao.ModuloId && formularioId == null) goto Remove;
                    moduloId = restricao.ModuloId;
                    formularioId = restricao.FormularioId;
                    continue;
                }
            Remove:
                if (formularioId != null) continue;
                _restricaoUsuarioRepository.Remove(restricao);
            }
            await _restricaoUsuarioRepository.UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
