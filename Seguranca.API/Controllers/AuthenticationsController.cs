using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Auth.Requests;
using Seguranca.Domain.Auth.Responses;
using Seguranca.Domain.Contracts.Clients.Auth;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SEG.MVC.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IRegisterClient _registerClient;
        private readonly ILoginClient _loginClient;
        private readonly ITrocaSenhaClient _trocaSenhaClient;
        private readonly IUsuarioService _usuarioService;
        private readonly IModuloService _moduloService;
        public AuthenticationsController(
            IRegisterClient registerClient, 
            ILoginClient loginClient, 
            ITrocaSenhaClient trocaSenhaClient,
            IUsuarioService usuarioService,
            IModuloService moduloService)
        {
            _registerClient = registerClient;
            _loginClient = loginClient;
            _trocaSenhaClient = trocaSenhaClient;
            _usuarioService = usuarioService;
            _moduloService = moduloService;
        }

        #region Register
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ResultResponse>> Register([FromBody] RegisterRequest user)
        {
            try
            {
                var modulo = await _moduloService.ObterAsync(user.Modulo);

                var result = ObterResult(await _registerClient.RegisterAsync(user));
                if (!result.Succeeded) return BadRequest(result);

                var register = JsonConvert.DeserializeObject<RegisterResponse>(result.ObjectRetorno.ToString());

                var usuario = new Usuario()
                { Nome = register.UserName, Email = register.Email, CreatedSystem = true };
                await _usuarioService.InsereAsync(usuario);

                register.Seguranca = await _usuarioService.ObterIds(register.UserName, user.Modulo);
                result.ObjectRetorno = register;
                return Ok(result);
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<ResultResponse>> Login([FromBody] LoginRequest user)
        {
            try
            {
                var modulo = await _moduloService.ObterAsync(user.Modulo);

                var result = ObterResult(await _loginClient.LoginAsync(user));
                if (!result.Succeeded) return BadRequest(result);

                var register = JsonConvert.DeserializeObject<RegisterResponse>(result.ObjectRetorno.ToString());
                register.Seguranca = await _usuarioService.ObterIds(register.UserName, user.Modulo);
                result.ObjectRetorno = register;
                return Ok(result);
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region TrocaSenha
        [HttpPost]
        [Route("TrocaSenha")]
        public async Task<IActionResult> TrocaSenha([FromBody] TrocaSenhaRequest user)
        {
            var result = ObterResult(await _trocaSenhaClient.TrocaSenhaAsync(user));
            if (result.Succeeded) return Ok(result);
            return BadRequest(result);
        }
        #endregion

        #region ObterResult
        private ResultResponse ObterResult(HttpResponseMessage httpResponse)
        {
            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
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
