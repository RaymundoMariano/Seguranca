using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Services;
using Seguranca.Core.Persistence.Contexts;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulosController : ControllerBase
    {
        private readonly IModuloService _moduloService;

        public ModulosController(IModuloService moduloService)
        {
            _moduloService = moduloService;
        }

        #region GetModulo
        // GET: api/Modulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Modulo>>> GetModulo()
        {
            try
            {
                var modulos = await _moduloService.ObterAsync();
                return modulos.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Modulos/5
        [HttpGet("{moduloId}")]
        public async Task<ActionResult<Modulo>> GetModulo(int moduloId)
        {
            try
            {
                return await _moduloService.ObterAsync(moduloId);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutModulo
        // PUT: api/Modulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{moduloId}")]
        public async Task<IActionResult> PutModulo(int moduloId, Modulo modulo)
        {
            try
            {
                if (moduloId != modulo.ModuloId) return BadRequest();
                await _moduloService.UpdateAsync(moduloId, modulo);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostModulo
        // POST: api/Modulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Modulo>> PostModulo(Modulo modulo)
        {
            try
            {
                await _moduloService.InsereAsync(modulo);
                return CreatedAtAction("GetModulo", new { moduloId = modulo.ModuloId }, modulo);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region DeleteModulo
        // DELETE: api/Modulos/5
        [HttpDelete("{moduloId}")]
        public async Task<IActionResult> DeleteModulo(int moduloId)
        {
            try
            {
                await _moduloService.RemoveAsync(moduloId);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
