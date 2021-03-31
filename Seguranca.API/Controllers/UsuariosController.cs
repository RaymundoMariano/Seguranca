using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        #region GetUsuario
        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            try
            {
                var usuarios = await _usuarioService.ObterAsync();
                return usuarios.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Usuarios/5
        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int usuarioId)
        {
            try
            {
                return await _usuarioService.ObterAsync(usuarioId);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutUsuario
        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{usuarioId}")]
        public async Task<IActionResult> PutUsuario(int usuarioId, Usuario usuario)
        {
            try
            {
                if (usuarioId != usuario.UsuarioId) return BadRequest();
                await _usuarioService.UpdateAsync(usuarioId, usuario);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostUsuario
        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                await _usuarioService.InsereAsync(usuario);
                return CreatedAtAction("GetUsuario", new { usuarioId = usuario.UsuarioId }, usuario);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

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
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
