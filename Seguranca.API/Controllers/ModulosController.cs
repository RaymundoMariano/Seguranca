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
        private readonly IModuloFormularioService _moduloFormularioService;
        private readonly IMapper _mapper;

        public ModulosController(
            IModuloService moduloService, 
            IModuloFormularioService moduloFormularioService, 
            IMapper mapper)
        {
            _moduloService = moduloService;
            _moduloFormularioService = moduloFormularioService;
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
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
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
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region GetFormularios
        [Route("GetFormularios")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetFormularios(int moduloId)
        {
            try
            {
                var resultResponse = await _moduloFormularioService.ObterFormulariosAsync(moduloId);
                resultResponse.ObjectRetorno = _mapper.Map<List<FormularioModel>>((List<Formulario>)resultResponse.ObjectRetorno);
                return resultResponse;
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostFormularios
        [Route("PostFormularios")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostFormularios(int moduloId, List<FormularioModel> formulariosModel)
        {
            try
            {
                var formularios = _mapper.Map<List<Formulario>>(formulariosModel);
                var resultResponse = await _moduloFormularioService.AtualizarFormulariosAsync(moduloId, formularios);
                return resultResponse;
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
