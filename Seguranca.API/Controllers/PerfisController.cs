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
    public class PerfisController : ControllerBase
    {
        private readonly IPerfilService _perfilService;

        public PerfisController(IPerfilService perfilService)
        {
            _perfilService = perfilService;
        }

        #region GetPerfil
        // GET: api/Perfis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetPerfil()
        {
            try
            {
                var perfis = await _perfilService.ObterAsync();
                return perfis.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        
        // GET: api/Perfis/5
        [HttpGet("{perfilId}")]
        public async Task<ActionResult<Perfil>> GetPerfil(int perfilId)
        {
            try
            {
                return await _perfilService.ObterAsync(perfilId);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutPerfil
        // PUT: api/Perfis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{perfilId}")]
        public async Task<IActionResult> PutPerfil(int perfilId, Perfil perfil)
        {
            try
            {
                if (perfilId != perfil.PerfilId) return BadRequest();
                await _perfilService.UpdateAsync(perfilId, perfil);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostPerfil
        // POST: api/Perfis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Perfil>> PostPerfil(Perfil perfil)
        {
            try
            {
                await _perfilService.InsereAsync(perfil);
                return CreatedAtAction("GetPerfil", new { perfilId = perfil.PerfilId }, perfil);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region DeletePerfil
        // DELETE: api/Perfis/5
        [HttpDelete("{perfilId}")]
        public async Task<IActionResult> DeletePerfil(int perfilId)
        {
            try
            {
                await _perfilService.RemoveAsync(perfilId);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
