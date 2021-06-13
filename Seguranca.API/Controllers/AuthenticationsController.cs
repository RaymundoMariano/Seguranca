using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Seguranca.Domain.Auth.Requests;
using Seguranca.Domain.Auth.Responses;
using Seguranca.Domain.Contracts.Clients.Auth;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
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
        private readonly IUsuarioService _usuarioService;
        public AuthenticationsController(IRegisterClient registerClient, ILoginClient loginClient, IUsuarioService usuarioService)
        {
            _registerClient = registerClient;
            _loginClient = loginClient;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest user)
        {
            var register = await _registerClient.RegisterAsync(user);

            return await ObjectResult(ObterRegister(register), user);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest user)
        {
            var login = await _loginClient.LoginAsync(user);

            return await ObjectResult(ObterRegister(login), user);
        }

        private async Task<IActionResult> ObjectResult(RegisterResponse register, object user)
        {
            switch ((EObjectResult)register.ObjectResult)
            {
                case EObjectResult.BadRequest:
                    return BadRequest(register);

                case EObjectResult.JsonResult:
                    return new JsonResult(register);

                case EObjectResult.OK:
                    if (user.GetType().Name == "RegisterRequest")
                    {
                        var usuario = new Usuario() { Nome = register.UserName, Email = register.Email };
                        await _usuarioService.InsereAsync(usuario);
                    }
                    return Ok(register);

                default:
                    return BadRequest(register);
            }
        }

        private RegisterResponse ObterRegister(HttpResponseMessage httpResponse)
        {
            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<RegisterResponse>(conteudo);
        }
    }
}
