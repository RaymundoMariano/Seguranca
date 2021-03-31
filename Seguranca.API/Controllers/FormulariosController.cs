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
    public class FormulariosController : ControllerBase
    {
        private readonly IFormularioService _formularioService;

        public FormulariosController(IFormularioService formularioService)
        {
            _formularioService = formularioService;
        }

        #region GetFormulario
        // GET: api/Formularios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Formulario>>> GetFormulario()
        {
            try
            {
                var formularios = await _formularioService.ObterAsync();
                return formularios.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Formularios/5
        [HttpGet("{formularioId}")]
        public async Task<ActionResult<Formulario>> GetFormulario(int formularioId)
        {
            try
            {
                return await _formularioService.ObterAsync(formularioId);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutFormulario
        // PUT: api/Formularios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{formularioId}")]
        public async Task<IActionResult> PutFormulario(int formularioId, Formulario formulario)
        {
            try
            {
                if (formularioId != formulario.FormularioId) return BadRequest();
                await _formularioService.UpdateAsync(formularioId, formulario);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostFormulario
        // POST: api/Formularios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Formulario>> PostFormulario(Formulario formulario)
        {
            try
            {
                await _formularioService.InsereAsync(formulario);
                return CreatedAtAction("GetFormulario", new { formularioId = formulario.FormularioId }, formulario);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region DeleteFormulario
        // DELETE: api/Fomularios/5
        [HttpDelete("{formularioId}")]
        public async Task<IActionResult> DeleteFormulario(int formularioId)
        {
            try
            {
                await _formularioService.RemoveAsync(formularioId);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
