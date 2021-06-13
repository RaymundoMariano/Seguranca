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
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetFormulario()
        {
            try
            {
                var formularios = await _formularioService.ObterAsync();
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<FormularioModel>>(formularios),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Formularios/5
        [HttpGet("{formularioId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetFormulario(int formularioId)
        {
            try
            {
                var formulario = await _formularioService.ObterAsync(formularioId);
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<FormularioModel>(formulario),
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

        #region PostFormulario
        // POST: api/Formularios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostFormulario(FormularioModel formularioModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var formulario = _mapper.Map<Formulario>(formularioModel);
                await _formularioService.InsereAsync(formulario);

                formularioModel = _mapper.Map<FormularioModel>(formulario);
                CreatedAtAction("GetFormulario", new { formularioId = formularioModel.FormularioId }, formularioModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = formularioModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = formularioModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutFormulario
        // PUT: api/Formularios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{formularioId}")]
        public async Task<ActionResult<ResultResponse>> PutFormulario(int formularioId, FormularioModel formularioModel)
        {
            try
            {
                if (formularioId != formularioModel.FormularioId) return BadRequest();

                var formulario = _mapper.Map<Formulario>(formularioModel);
                await _formularioService.UpdateAsync(formularioId, formulario);

                formularioModel = _mapper.Map<FormularioModel>(formulario);
                CreatedAtAction("GetFormulario", new { formularioId = formularioModel.FormularioId }, formularioModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = formularioModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = formularioModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        

        #region DeleteFormulario
        // DELETE: api/Fomularios/5
        [HttpDelete("{formularioId}")]
        public async Task<ActionResult<ResultResponse>> DeleteFormulario(int formularioId)
        {
            try
            {
                await _formularioService.RemoveAsync(formularioId);
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

        #region GetEventos
        [Route("GetEventos")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetEventos(int formularioId)
        {
            var resultResponse = await _formularioService.ObterEventosAsync(formularioId);
            if (resultResponse.Succeeded)
            {
                resultResponse.ObjectRetorno = _mapper.Map<List<EventoModel>>((List<Evento>)resultResponse.ObjectRetorno);
            }
            else
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion

        #region PostEventos
        [Route("PostEventos")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostEventos(int formularioId, List<EventoModel> eventosModel)
        {
            var eventos = _mapper.Map<List<Evento>>(eventosModel);

            var resultResponse = await _formularioService.AtualizarEventosAsync(formularioId, eventos);
            if (!resultResponse.Succeeded)
            {
                resultResponse.ObjectResult = (int)EObjectResult.BadRequest;
            }
            return resultResponse;
        }
        #endregion
    }
}
