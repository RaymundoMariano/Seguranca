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
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;
        private readonly IRestricaoPerfilRepository _restricaoPerfilRepository;
        private readonly IModuloRepository _moduloRepository;

        public PerfilService(
            IPerfilRepository perfilRepository,
            IRestricaoPerfilRepository restricaoPerfilRepository,
            IModuloRepository moduloRepository)
        {
            _perfilRepository = perfilRepository;
            _restricaoPerfilRepository = restricaoPerfilRepository;
            _moduloRepository = moduloRepository;
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
                var perfil = await _perfilRepository.ObterAsyncFull(perfilId);
                if (perfil == null)
                    throw new ServiceException($"Perfil com Id = {perfilId} não foi encontrado");
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
                    throw new ServiceException(perfilId + " Diferente " + perfil.PerfilId);

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
                if (perfil == null)
                    throw new ServiceException($"Perfil com Id = {perfilId} não foi encontrado");
                if (perfil.CreatedSystem)
                    throw new ServiceException($"Perfil com Id = {perfilId} foi criado pelo sistema");
                _perfilRepository.Remove(perfil);
                await _perfilRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ObterRestricoesAsync 
        public async Task<ResultResponse> ObterRestricoesAsync(int perfilId)
        {
            var result = GetPerfil(perfilId).Result;
            if (!result.Succeeded) return result;
            var perfil = (Perfil)result.ObjectRetorno;

            var modulos = await _moduloRepository.ObterAsyncFull();

            var restricoes = new List<RestricaoPerfil>();

            foreach (var modulo in modulos)
            {
                var rp = await _restricaoPerfilRepository.ObterAsync(perfil.PerfilId, modulo.ModuloId, null, null);
                if (rp != null)
                {
                    var restricao = await MontarRestricao(perfil, modulo, null, null);
                    restricoes.Add(restricao);
                    continue;
                }

                if (modulo.ModuloFormulario.Count == 0)
                {
                    var restricao = await MontarRestricao(perfil, modulo, null, null);
                    restricoes.Add(restricao);
                }
                else
                {
                    foreach (var mf in modulo.ModuloFormulario)
                    {
                        rp = await _restricaoPerfilRepository.ObterAsync(perfil.PerfilId, mf.ModuloId, mf.FormularioId, null);
                        if (rp != null)
                        {
                            var restricao = await MontarRestricao(perfil, modulo, mf.Formulario, null);
                            restricoes.Add(restricao);
                            continue;
                        }

                        if (mf.Formulario.FormularioEvento.Count == 0)
                        {
                            var restricao = await MontarRestricao(perfil, modulo, mf.Formulario, null);
                            restricoes.Add(restricao);
                        }
                        else
                        {
                            foreach (var fe in mf.Formulario.FormularioEvento)
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
        #endregion

        #region AtualizarRestricoesAsync
        public async Task<ResultResponse> AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfil> restricoes)
        {
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
        #endregion        

        #region GetPerfil
        private async Task<ResultResponse> GetPerfil(int perfilId)
        {
            var perfil = await _perfilRepository.ObterAsyncFull(perfilId);
            if (perfil == null)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.NotFound,
                    Errors = new List<string>() { $"Perfil com Id = {perfilId} não foi encontrado" }
                });
            }
            else
            {
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = perfil,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
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
