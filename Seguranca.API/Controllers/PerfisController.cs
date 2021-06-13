using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Models;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Enums;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetPerfil()
        {
            try
            {
                var perfis = await _perfilService.ObterAsync();
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<PerfilModel>>(perfis),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        
        // GET: api/Perfis/5
        [HttpGet("{perfilId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetPerfil(int perfilId)
        {
            try
            {
                var perfil = await _perfilService.ObterAsync(perfilId);
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<PerfilModel>(perfil),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.NotFound,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostPerfil
        // POST: api/Perfis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostPerfil(PerfilModel perfilModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var perfil = _mapper.Map<Perfil>(perfilModel);
                await _perfilService.InsereAsync(perfil);

                perfilModel = _mapper.Map<PerfilModel>(perfil);
                CreatedAtAction("GetPerfil", new { perfilId = perfilModel.PerfilId }, perfilModel);
                
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = perfilModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = perfilModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutPerfil
        // PUT: api/Perfis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{perfilId}")]
        public async Task<ActionResult<ResultResponse>> PutPerfil(int perfilId, PerfilModel perfilModel)
        {
            try
            {
                if (perfilId != perfilModel.PerfilId) return BadRequest();

                var perfil = _mapper.Map<Perfil>(perfilModel);
                await _perfilService.UpdateAsync(perfilId, perfil);

                perfilModel = _mapper.Map<PerfilModel>(perfil);
                CreatedAtAction("GetPerfil", new { perfilId = perfilModel.PerfilId }, perfilModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = perfilModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = perfilModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region DeletePerfil
        // DELETE: api/Perfis/5
        [HttpDelete("{perfilId}")]
        public async Task<ActionResult<ResultResponse>> DeletePerfil(int perfilId)
        {
            try
            {
                await _perfilService.RemoveAsync(perfilId);
                NoContent();
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region GetRestricoes
        [Route("GetRestricoes")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetRestricoes(int perfilId)
        {
            var resultResponse = await _perfilService.ObterRestricoesAsync(perfilId);
            if (resultResponse.Succeeded)
            {
                resultResponse.ObjectRetorno = _mapper.Map<List<RestricaoPerfilModel>>((List<RestricaoPerfil>)resultResponse.ObjectRetorno);
            }
            else
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion

        #region PostRestricoes
        [Route("PostRestricoes")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostRestricoes(int perfilId, List<RestricaoPerfilModel> restricoesModel)
        {
            var restricoes = _mapper.Map<List<RestricaoPerfil>>(restricoesModel);

            var resultResponse = await _perfilService.AtualizarRestricoesAsync(perfilId, restricoes);
            if (!resultResponse.Succeeded)
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion
    }
}
