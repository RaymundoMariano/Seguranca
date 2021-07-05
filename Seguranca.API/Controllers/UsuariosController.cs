using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Models;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Enums;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRestricaoUsuarioService _restricaoUsuarioService;
        private readonly IPerfilUsuarioService _perfilUsuarioService;
        private readonly IMapper _mapper;

        public UsuariosController(
            IUsuarioService usuarioService, 
            IRestricaoUsuarioService restricaoUsuarioService,
            IPerfilUsuarioService perfilUsuarioService,
            IMapper mapper)
        {
            _usuarioService = usuarioService;
            _restricaoUsuarioService = restricaoUsuarioService;
            _perfilUsuarioService = perfilUsuarioService;
            _mapper = mapper;
        }

        #region GetUsuario
        // GET: api/Usuarios
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetUsuario()
        {
            try
            {
                var usuarios = await _usuarioService.GetFullAsync();
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<UsuarioModel>>(usuarios),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }

        // GET: api/Usuarios/5
        [HttpGet("{usuarioId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.GetFullAsync(usuarioId);
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<UsuarioModel>(usuario),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region GetRestricoes
        [Route("GetRestricoes")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetRestricoes(int usuarioId)
        {
            try
            {
                var resultResponse = await _restricaoUsuarioService.ObterRestricoesAsync(usuarioId);
                resultResponse.ObjectRetorno = _mapper.Map<List<RestricaoUsuarioModel>>((List<RestricaoUsuario>)resultResponse.ObjectRetorno);
                return resultResponse;
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostRestricoes
        [Route("PostRestricoes")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostRestricoes(int usuarioId, List<RestricaoUsuarioModel> restricoesModel)
        {
            try
            {
                var restricoes = _mapper.Map<List<RestricaoUsuario>>(restricoesModel);
                var resultResponse = await _restricaoUsuarioService.AtualizarRestricoesAsync(usuarioId, restricoes);
                return resultResponse;
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region GetPerfis
        [Route("GetPerfis")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetPerfis(int usuarioId)
        {
            try
            {
                var resultResponse = await _perfilUsuarioService.ObterPerfisAsync(usuarioId);
                resultResponse.ObjectRetorno = _mapper.Map<List<PerfilUsuarioModel>>((List<PerfilUsuario>)resultResponse.ObjectRetorno);
                return resultResponse;
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostPerfis
        [Route("PostPerfis")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostPerfis(int usuarioId, List<PerfilUsuarioModel> perfisModel)
        {
            try
            {
                var perfis = _mapper.Map<List<PerfilUsuario>>(perfisModel);
                var resultResponse = await _perfilUsuarioService.AtualizarPerfisAsync(usuarioId, perfis);
                return resultResponse;
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Erro
        private ActionResult<ResultResponse> Erro(ETipoErro erro, string mensagem)
        {
            return (new ResultResponse()
            {
                Succeeded = false,
                ObjectRetorno = null,
                ObjectResult = (erro == ETipoErro.Fatal)
                    ? (int)EObjectResult.ErroFatal : (int)EObjectResult.BadRequest,
                Errors = (mensagem == null)
                    ? new List<string>() : new List<string> { mensagem }
            });
        }
        #endregion
    }
}
