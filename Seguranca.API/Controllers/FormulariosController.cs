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
        private readonly IFormularioEventoService _formularioEventoService;
        private readonly IMapper _mapper;

        public FormulariosController(
            IFormularioService formularioService,
            IFormularioEventoService formularioEventoService,
            IMapper mapper)
        {
            _formularioService = formularioService;
            _formularioEventoService = formularioEventoService;
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
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region GetEventos
        [Route("GetEventos")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetEventos(int formularioId)
        {
            try
            {
                var resultResponse = await _formularioEventoService.ObterEventosAsync(formularioId);
                resultResponse.ObjectRetorno = _mapper.Map<List<EventoModel>>((List<Evento>)resultResponse.ObjectRetorno);
                return resultResponse;
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostEventos
        [Route("PostEventos")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostEventos(int formularioId, List<EventoModel> eventosModel)
        {
            try
            {
                var eventos = _mapper.Map<List<Evento>>(eventosModel);
                return await _formularioEventoService.AtualizarEventosAsync(formularioId, eventos);
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Erro
        private ActionResult<ResultResponse> Erro(ETipoErro erro, string mensagem)
        {
            return (new ResultResponse()
            {
                Succeeded = false,
                ObjectRetorno = null,
                ObjectResult = (erro == ETipoErro.Fatal) 
                    ? (int)EObjectResult.ErroFatal : (int)EObjectResult.BadRequest,
                Errors = (mensagem == null) 
                    ? new List<string>() : new List<string> { mensagem }
            });
        }
        #endregion
    }
}
