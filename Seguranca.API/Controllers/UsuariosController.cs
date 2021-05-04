using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Models;
using Seguranca.Domain.Contracts.Services;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<ActionResult<IEnumerable<UsuarioModel>>> GetUsuario()
        {
            try
            {
                var usuarios = await _usuarioService.ObterAsync();
                return _mapper.Map<List<UsuarioModel>>(usuarios);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Usuarios/5
        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<UsuarioModel>> GetUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.ObterAsync(usuarioId);
                return _mapper.Map<UsuarioModel>(usuario);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("UsuarioId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        
    }
}
