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
    public class RestricaoUsuarioService : IRestricaoUsuarioService
    {
        private readonly IRestricaoUsuarioRepository _restricaoUsuarioRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IModuloService _moduloService;
        public RestricaoUsuarioService(
            IRestricaoUsuarioRepository restricaoUsuarioRepository,
            IUsuarioService usuarioService,
            IModuloService moduloService)
        {
            _restricaoUsuarioRepository = restricaoUsuarioRepository;
            _usuarioService = usuarioService;
            _moduloService = moduloService;
        }

        #region ObterAsync
        public async Task<IEnumerable<RestricaoUsuario>> ObterAsync()
        {
            try 
            {
                return await _restricaoUsuarioRepository.ObterAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<RestricaoUsuario> ObterAsync(int id)
        {
            try
            {
                var restricaoUsuario = await _restricaoUsuarioRepository.ObterAsync(id);
                if (restricaoUsuario == null) throw new ServiceException(
                    $"Restriçao de usuário com Id = {id} não foi encontrada");

                return restricaoUsuario;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(RestricaoUsuario restricaoUsuario)
        {
            try
            {
                _restricaoUsuarioRepository.Insere(restricaoUsuario);
                await _restricaoUsuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, RestricaoUsuario restricaoUsuario)
        {
            try
            {
                if (id != restricaoUsuario.Id) throw new ServiceException(
                    $"Id informado {id} é diferente do Id de restrição de usuário {restricaoUsuario.Id}");

                _restricaoUsuarioRepository.Update(restricaoUsuario);
                await _restricaoUsuarioRepository.UnitOfWork.SaveChangesAsync();
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
                var restricaoUsuario = await ObterAsync(id);
                if (restricaoUsuario == null) throw new ServiceException(
                    $"Restrição de usuário com Id = {id} não foi encontrada");

                _restricaoUsuarioRepository.Remove(restricaoUsuario);
                await _restricaoUsuarioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterRestricoesAsync 
        public async Task<ResultResponse> ObterRestricoesAsync(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.GetFullAsync(usuarioId);

                var restricoes = new List<RestricaoUsuario>();

                foreach (var modulo in await _moduloService.GetFullAsync())
                {
                    if (modulo.CreatedSystem) continue;

                    var ru = await _restricaoUsuarioRepository
                        .ObterAsync(usuarioId, modulo.ModuloId, null, null);
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
                            ru = await _restricaoUsuarioRepository
                                .ObterAsync(usuarioId, mf.ModuloId, mf.FormularioId, null);
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
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region AtualizarRestricoesAsync
        public async Task<ResultResponse> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuario> restricoes)
        {
            try
            {
                var usuario = await _usuarioService.GetFullAsync(usuarioId);

                var pu = usuario.PerfilUsuario.First(pf => pf.UsuarioId == usuarioId && pf.ModuloId == 1);
                if (pu.Perfil.CreatedSystem) throw new ServiceException(
                    $"O usuário {usuario.Nome} com perfil {pu.Perfil.Nome} foi criado pelo sistema. Operação inválida!");
                
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
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
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
