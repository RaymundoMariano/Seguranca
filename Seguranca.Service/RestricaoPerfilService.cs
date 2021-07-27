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
    public class RestricaoPerfilService : IRestricaoPerfilService
    {
        private readonly IRestricaoPerfilRepository _restricaoPerfilRepository;
        private readonly IPerfilService _perfilService;
        private readonly IModuloService _moduloService;
        public RestricaoPerfilService(
            IRestricaoPerfilRepository restricaoPerfilRepository,
            IPerfilService perfilService,
            IModuloService moduloService)
        {
            _restricaoPerfilRepository = restricaoPerfilRepository;
            _perfilService = perfilService;
            _moduloService = moduloService;
        }

        #region ObterAsync
        public async Task<IEnumerable<RestricaoPerfil>> ObterAsync()
        {
            try 
            { 
                return await _restricaoPerfilRepository.ObterAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<RestricaoPerfil> ObterAsync(int id)
        {
            try
            {
                var restricaoPerfil = await _restricaoPerfilRepository.ObterAsync(id);
                if (restricaoPerfil == null) throw new ServiceException(
                    $"Restriçao de perfil com Id = {id} não foi encontrada");

                return restricaoPerfil;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(RestricaoPerfil restricaoPerfil)
        {
            try
            {
                _restricaoPerfilRepository.Insere(restricaoPerfil);
                await _restricaoPerfilRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, RestricaoPerfil restricaoPerfil)
        {
            try
            {
                if (id != restricaoPerfil.Id) throw new ServiceException(
                    $"Id informado {id} é diferente do Id de restrição de perfil {restricaoPerfil.Id}");

                _restricaoPerfilRepository.Update(restricaoPerfil);
                await _restricaoPerfilRepository.UnitOfWork.SaveChangesAsync();
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
                var restricaoPerfil = await ObterAsync(id);
                if (restricaoPerfil == null) throw new ServiceException(
                    $"Restrição de perfil com Id = {id} não foi encontrada");

                _restricaoPerfilRepository.Remove(restricaoPerfil);
                await _restricaoPerfilRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterRestricoesAsync 
        public async Task<ResultResponse> ObterRestricoesAsync(int perfilId)
        {
            try
            {
                var perfil = await _perfilService.ObterAsync(perfilId);

                var restricoes = new List<RestricaoPerfil>();

                foreach (var modulo in await _moduloService.ObterAsync())
                {
                    if (modulo.Nome == "SegurancaNet") continue;

                    var rp = await _restricaoPerfilRepository
                        .ObterAsync(perfil.PerfilId, modulo.ModuloId, null, null);
                    if (rp != null)
                    {
                        var restricao = await MontarRestricao(perfil, modulo, null, null);
                        restricoes.Add(restricao);
                        continue;
                    }

                    if (modulo.ModulosFormulario.Count == 0)
                    {
                        var restricao = await MontarRestricao(perfil, modulo, null, null);
                        restricoes.Add(restricao);
                    }
                    else
                    {
                        foreach (var mf in modulo.ModulosFormulario)
                        {
                            rp = await _restricaoPerfilRepository
                                .ObterAsync(perfil.PerfilId, mf.ModuloId, mf.FormularioId, null);
                            if (rp != null)
                            {
                                var restricao = await MontarRestricao(perfil, modulo, mf.Formulario, null);
                                restricoes.Add(restricao);
                                continue;
                            }

                            if (mf.Formulario.FormulariosEvento.Count == 0)
                            {
                                var restricao = await MontarRestricao(perfil, modulo, mf.Formulario, null);
                                restricoes.Add(restricao);
                            }
                            else
                            {
                                foreach (var fe in mf.Formulario.FormulariosEvento)
                                {
                                    var restricao = await MontarRestricao(perfil, modulo, mf.Formulario, fe.Evento);
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
        public async Task<ResultResponse> AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfil> restricoes)
        {
            try
            {
                var perfil = await _perfilService.ObterAsync(perfilId);

                if (perfil.CreatedSystem) throw new ServiceException(
                    $"O perfil {perfil.Nome} foi criado pelo sistema. Operação inválida!");
                
                foreach (var restricao in restricoes)
                {
                    int? moduloId = (restricao.Modulo == null) ? null : restricao.ModuloId;
                    int? formularioId = (restricao.Formulario == null) ? null : restricao.FormularioId;
                    int? eventoId = (restricao.Evento == null) ? null : restricao.EventoId;

                    var rp = await _restricaoPerfilRepository.ObterAsync(restricao.Id);
                    if (rp == null)
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
                            var rpp = (new RestricaoPerfil()
                            {
                                PerfilId = perfilId,
                                ModuloId = moduloId,
                                FormularioId = formularioId,
                                EventoId = eventoId
                            });
                            _restricaoPerfilRepository.Insere(rpp);
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
                            _restricaoPerfilRepository.Remove(rp);
                        }
                        else
                        {
                            rp.PerfilId = perfilId;
                            rp.FormularioId = (restricao.Formulario == null || !restricao.Formulario.Selected) ? null : formularioId;
                            rp.EventoId = (restricao.Evento == null || !restricao.Evento.Selected) ? null : eventoId;

                            rp.ModuloId = moduloId;

                            if (restricao.Formulario != null && !restricao.Formulario.Selected)
                            {
                                rp.FormularioId = null;
                                rp.EventoId = null;
                            }

                            if (restricao.Evento != null && !restricao.Evento.Selected)
                            {
                                rp.EventoId = null;
                            }

                            if (!restricao.Modulo.Selected)
                            {
                                _restricaoPerfilRepository.Remove(rp);
                            }
                            else
                            {
                                _restricaoPerfilRepository.Update(rp);
                            }
                        }
                        await _restricaoPerfilRepository.UnitOfWork.SaveChangesAsync();
                    }
                }
                await _restricaoPerfilRepository.UnitOfWork.SaveChangesAsync();

                await RemoveRestricoesExcedentes(perfilId);

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
        private async Task<RestricaoPerfil> MontarRestricao(
            Perfil perfil, Modulo modulo, Formulario formulario, Evento evento)
        {
            int? moduloId = (modulo == null) ? null : modulo.ModuloId;
            int? formularioId = (formulario == null) ? null : formulario.FormularioId;
            int? eventoId = (evento == null) ? null : evento.EventoId;

            var rp = await _restricaoPerfilRepository.ObterAsync(perfil.PerfilId, moduloId, formularioId, eventoId);
            if (rp == null)
            {
                rp = new RestricaoPerfil();
                rp.PerfilId = perfil.PerfilId;
                rp.ModuloId = moduloId;
                rp.FormularioId = formularioId;
                rp.EventoId = eventoId;

                rp.Perfil = perfil;
                rp.Modulo = new Modulo();
                rp.Modulo.Nome = (modulo == null) ? null : modulo.Nome;
                rp.Modulo.Selected = false;

                rp.Formulario = (formulario == null) ? null : new Formulario();
                if (rp.Formulario != null)
                {
                    rp.Formulario.Nome = (formulario == null) ? null : formulario.Nome;
                    rp.Formulario.Selected = false;
                }

                rp.Evento = (evento == null) ? null : new Evento();
                if (rp.EventoId != null)
                {
                    rp.Evento.Nome = (evento == null) ? null : evento.Nome;
                    rp.Evento.Selected = false;
                }
            }
            else
            {
                rp.Perfil = perfil;

                rp.Modulo = (modulo == null) ? null : modulo;
                rp.Modulo.Selected = (modulo != null) ? true : false;

                rp.Formulario = (formulario == null) ? null : formulario;
                if (rp.Formulario != null)
                {
                    rp.Formulario.Selected = (formulario != null) ? true : false;
                }

                rp.Evento = (evento == null) ? null : evento;
                if (rp.Evento != null)
                {
                    rp.Evento.Selected = (evento != null) ? true : false;
                }
            }
            return rp;
        }
        #endregion

        #region RemoveRestricoesExcedentes
        private async Task RemoveRestricoesExcedentes(int perfilId)
        {
            var restricoes = await _restricaoPerfilRepository.ObterAsync();
            restricoes = restricoes.Where(r => r.PerfilId == perfilId);
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
                _restricaoPerfilRepository.Remove(restricao);
            }
            await _restricaoPerfilRepository.UnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
