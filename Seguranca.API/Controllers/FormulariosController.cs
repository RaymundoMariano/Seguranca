using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Models;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormulariosController : ControllerBase
    {
        private readonly IFormularioService _formularioService;
        private readonly IMapper _mapper;

        public FormulariosController(IFormularioService formularioService, IMapper mapper)
        {
            _formularioService = formularioService;
            _mapper = mapper;
        }

        #region GetFormulario
        // GET: api/Formularios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormularioModel>>> GetFormulario()
        {
            try
            {
                var formularios = await _formularioService.ObterAsync();
                return _mapper.Map<List<FormularioModel>>(formularios);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Formularios/5
        [HttpGet("{formularioId}")]
        public async Task<ActionResult<FormularioModel>> GetFormulario(int formularioId)
        {
            try
            {
                var formulario = await _formularioService.ObterAsync(formularioId);
                return _mapper.Map<FormularioModel>(formulario);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("FormularioId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostFormulario
        // POST: api/Formularios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FormularioModel>> PostFormulario(FormularioModel formularioModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var formulario = _mapper.Map<Formulario>(formularioModel);
                await _formularioService.InsereAsync(formulario);

                formularioModel = _mapper.Map<FormularioModel>(formulario);
                return CreatedAtAction("Get", new { formularioId = formularioModel.FormularioId }, formularioModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("FormularioId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutFormulario
        // PUT: api/Formularios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{formularioId}")]
        public async Task<IActionResult> PutFormulario(int formularioId, FormularioModel formularioModel)
        {
            try
            {
                if (formularioId != formularioModel.FormularioId) return BadRequest();

                var formulario = _mapper.Map<Formulario>(formularioModel);
                await _formularioService.UpdateAsync(formularioId, formulario);

                formularioModel = _mapper.Map<FormularioModel>(formulario);
                return CreatedAtAction("GetFormulario", new { formularioId = formularioModel.FormularioId }, formularioModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("FormularioId", ex.Message);
                return BadRequest(ModelState);
            }
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
            catch (ServiceException ex)
            {
                ModelState.AddModelError("FormularioId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
