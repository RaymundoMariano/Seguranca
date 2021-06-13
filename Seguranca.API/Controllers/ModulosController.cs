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
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetModulo()
        {
            try
            {
                var modulos = await _moduloService.ObterAsync();
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<ModuloModel>>(modulos),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Modulos/5
        [HttpGet("{moduloId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetModulo(int moduloId)
        {
            try
            {
                var modulo = await _moduloService.ObterAsync(moduloId);
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<ModuloModel>(modulo),
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

        #region PostModulo
        // POST: api/Modulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostModulo(ModuloModel moduloModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var modulo = _mapper.Map<Modulo>(moduloModel);
                await _moduloService.InsereAsync(modulo);

                moduloModel = _mapper.Map<ModuloModel>(modulo);
                CreatedAtAction("GetModulo", new { moduloId = moduloModel.ModuloId }, moduloModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = moduloModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = moduloModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutModulo
        // PUT: api/Modulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{moduloId}")]
        public async Task<ActionResult<ResultResponse>> PutModulo(int moduloId, ModuloModel moduloModel)
        {
            try
            {
                if (moduloId != moduloModel.ModuloId) return BadRequest();

                var modulo = _mapper.Map<Modulo>(moduloModel);
                await _moduloService.UpdateAsync(moduloId, modulo);

                moduloModel = _mapper.Map<ModuloModel>(modulo);
                CreatedAtAction("GetModulo", new { moduloId = moduloModel.ModuloId }, moduloModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = moduloModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = moduloModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        

        #region DeleteModulo
        // DELETE: api/Modulos/5
        [HttpDelete("{moduloId}")]
        public async Task<ActionResult<ResultResponse>> DeleteModulo(int moduloId)
        {
            try
            {
                await _moduloService.RemoveAsync(moduloId);
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

        #region GetFormularios
        [Route("GetFormularios")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetFormularios(int moduloId)
        {
            var resultResponse = await _moduloService.ObterFormulariosAsync(moduloId);
            if (resultResponse.Succeeded)
            {
                resultResponse.ObjectRetorno = _mapper.Map<List<FormularioModel>>((List<Formulario>)resultResponse.ObjectRetorno);
            }
            else
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion

        #region PostFormularios
        [Route("PostFormularios")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostFormularios(int moduloId, List<FormularioModel> formulariosModel)
        {
            var formularios = _mapper.Map<List<Formulario>>(formulariosModel);

            var resultResponse = await _moduloService.AtualizarFormulariosAsync(moduloId, formularios);
            if (!resultResponse.Succeeded)
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion
    }
}
