using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.DTO;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

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
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuario()
        {
            try
            {
                var usuarios = await _usuarioService.ObterAsync();
                return _mapper.Map<List<UsuarioDTO>>(usuarios);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Usuarios/5
        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _usuarioService.ObterAsync(usuarioId);
                return _mapper.Map<UsuarioDTO>(usuario);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("UsuarioId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostUsuario
        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioModel>> PostUsuario(UsuarioModel usuarioModel)
        {
            var senha = usuarioModel.Senha;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var usuario = _mapper.Map<Usuario>(usuarioModel);
                await _usuarioService.InsereAsync(usuario);

                usuarioModel = _mapper.Map<UsuarioModel>(usuario);
                usuarioModel.Senha = senha;
                return CreatedAtAction("GetUsuario", new { usuarioId = usuarioModel.UsuarioId }, usuarioModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("UsuarioId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        ////#region PutUsuario
        ////// PUT: api/Usuarios/5
        ////// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        ////[HttpPut("{usuarioId}")]
        ////public async Task<IActionResult> PutUsuario(int usuarioId, UsuarioModel usuarioModel)
        ////{
        ////    var senha = usuarioModel.Senha;
        ////    try
        ////    {
        ////        if (usuarioId != usuarioModel.UsuarioId) return BadRequest();

        ////        var usuario = _mapper.Map<Usuario>(usuarioModel);
        ////        await _usuarioService.UpdateAsync(usuarioId, usuario);

        ////        usuarioModel = _mapper.Map<UsuarioModel>(usuario);
        ////        usuarioModel.Senha = senha;
        ////        return CreatedAtAction("GetUsuario", new { usuarioId = usuarioModel.UsuarioId }, usuarioModel);
        ////    }
        ////    catch (ServiceException ex)
        ////    {
        ////        ModelState.AddModelError("UsuarioId", ex.Message);
        ////        return BadRequest(ModelState);
        ////    }
        ////    catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        ////}
        ////#endregion

        #region DeleteUsuario
        // DELETE: api/Usuarios/5
        [HttpDelete("{usuarioId}")]
        public async Task<IActionResult> DeleteUsuario(int usuarioId)
        {
            try
            {
                await _usuarioService.RemoveAsync(usuarioId);
                return NoContent();
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
