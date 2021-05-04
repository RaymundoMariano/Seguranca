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
    public class ModulosController : ControllerBase
    {
        private readonly IModuloService _moduloService;
        private readonly IMapper _mapper;

        public ModulosController(IModuloService moduloService, IMapper mapper)
        {
            _moduloService = moduloService;
            _mapper = mapper;
        }

        #region GetModulo
        // GET: api/Modulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuloModel>>> GetModulo()
        {
            try
            {
                var modulos = await _moduloService.ObterAsync();
                return _mapper.Map<List<ModuloModel>>(modulos);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Modulos/5
        [HttpGet("{moduloId}")]
        public async Task<ActionResult<ModuloModel>> GetModulo(int moduloId)
        {
            try
            {
                var modulo = await _moduloService.ObterAsync(moduloId);
                return _mapper.Map<ModuloModel>(modulo);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("ModuloId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostModulo
        // POST: api/Modulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuloModel>> PostModulo(ModuloModel moduloModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var modulo = _mapper.Map<Modulo>(moduloModel);
                await _moduloService.InsereAsync(modulo);

                moduloModel = _mapper.Map<ModuloModel>(modulo);
                return CreatedAtAction("GetModulo", new { moduloId = moduloModel.ModuloId }, moduloModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("ModuloId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutModulo
        // PUT: api/Modulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{moduloId}")]
        public async Task<IActionResult> PutModulo(int moduloId, ModuloModel moduloModel)
        {
            try
            {
                if (moduloId != moduloModel.ModuloId) return BadRequest();

                var modulo = _mapper.Map<Modulo>(moduloModel);
                await _moduloService.UpdateAsync(moduloId, modulo);

                moduloModel = _mapper.Map<ModuloModel>(modulo);
                return CreatedAtAction("GetModulo", new { moduloId = moduloModel.ModuloId }, moduloModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("ModuloId", ex.Message);
                return BadRequest(ModelState);
            }
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
            catch (ServiceException ex)
            {
                ModelState.AddModelError("ModuloId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
