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
        private readonly IMapper _mapper;

        public UsuariosController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
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
                var usuarios = await _usuarioService.ObterAsync();
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<UsuarioModel>>(usuarios),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Usuarios/5
        [HttpGet("{usuarioId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.ObterAsync(usuarioId);
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<UsuarioModel>(usuario),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.NotFound,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region GetRestricoes
        [Route("GetRestricoes")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetRestricoes(int usuarioId)
        {
            var resultResponse = await _usuarioService.ObterRestricoesAsync(usuarioId);
            if (resultResponse.Succeeded)
            {
                resultResponse.ObjectRetorno = _mapper.Map<List<RestricaoUsuarioModel>>((List<RestricaoUsuario>)resultResponse.ObjectRetorno);
            }
            else
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion

        #region PostRestricoes
        [Route("PostRestricoes")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostRestricoes(int usuarioId, List<RestricaoUsuarioModel> restricoesModel)
        {
            var restricoes = _mapper.Map<List<RestricaoUsuario>>(restricoesModel);

            var resultResponse = await _usuarioService.AtualizarRestricoesAsync(usuarioId, restricoes);
            if (!resultResponse.Succeeded)
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion

        #region GetPerfis
        [Route("GetPerfis")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetPerfis(int usuarioId)
        {
            var resultResponse = await _usuarioService.ObterPerfisAsync(usuarioId);
            if (resultResponse.Succeeded)
            {
                resultResponse.ObjectRetorno = _mapper.Map<List<PerfilUsuarioModel>>((List<PerfilUsuario>)resultResponse.ObjectRetorno);
            }
            else
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion

        #region PostPerfis
        [Route("PostPerfis")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostPerfis(int usuarioId, List<PerfilUsuarioModel> perfisModel)
        {
            var perfis = _mapper.Map<List<PerfilUsuario>>(perfisModel);

            var resultResponse = await _usuarioService.AtualizarPerfisAsync(usuarioId, perfis);
            if (!resultResponse.Succeeded)
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion
    }
}
