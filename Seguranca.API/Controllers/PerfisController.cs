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
    public class PerfisController : ControllerBase
    {
        private readonly IPerfilService _perfilService;
        private readonly IMapper _mapper;

        public PerfisController(IPerfilService perfilService, IMapper mapper)
        {
            _perfilService = perfilService;
            _mapper = mapper;
        }

        #region GetPerfil
        // GET: api/Perfis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerfilModel>>> GetPerfil()
        {
            try
            {
                var perfis = await _perfilService.ObterAsync();
                return _mapper.Map<List<PerfilModel>>(perfis);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        
        // GET: api/Perfis/5
        [HttpGet("{perfilId}")]
        public async Task<ActionResult<PerfilModel>> GetPerfil(int perfilId)
        {
            try
            {
                var perfil = await _perfilService.ObterAsync(perfilId);
                return _mapper.Map<PerfilModel>(perfil);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("PerfilId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostPerfil
        // POST: api/Perfis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PerfilModel>> PostPerfil(PerfilModel perfilModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var perfil = _mapper.Map<Perfil>(perfilModel);
                await _perfilService.InsereAsync(perfil);

                perfilModel = _mapper.Map<PerfilModel>(perfil);
                return CreatedAtAction("GetPerfil", new { perfilId = perfilModel.PerfilId }, perfilModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("PerfilId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutPerfil
        // PUT: api/Perfis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{perfilId}")]
        public async Task<IActionResult> PutPerfil(int perfilId, PerfilModel perfilModel)
        {
            try
            {
                if (perfilId != perfilModel.PerfilId) return BadRequest();

                var perfil = _mapper.Map<Perfil>(perfilModel);
                await _perfilService.UpdateAsync(perfilId, perfil);

                perfilModel = _mapper.Map<PerfilModel>(perfil);
                return CreatedAtAction("GetPerfil", new { perfilId = perfilModel.PerfilId }, perfilModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("PerfilId", ex.Message);
                return BadRequest(ModelState);
            }
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
            catch (ServiceException ex)
            {
                ModelState.AddModelError("PerfilId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
