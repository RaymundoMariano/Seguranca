using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Models;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
using Seguranca.Domain.Aplication.Responses;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IMapper _mapper;

        public EventosController(IEventoService eventoService, IMapper mapper)
        {
            _eventoService = eventoService;
            _mapper = mapper;
        }

        #region GetEvento
        // GET: api/Eventos
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetEvento()
        {
            try
            {
                var eventos = await _eventoService.ObterAsync();
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<EventoModel>>(eventos),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Eventos/5
        [HttpGet("{eventoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoService.ObterAsync(eventoId);
                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<EventoModel>(evento),
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

        #region PostEvento
        // POST: api/Eventos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostEvento(EventoModel eventoModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var evento = _mapper.Map<Evento>(eventoModel);
                await _eventoService.InsereAsync(evento);

                eventoModel = _mapper.Map<EventoModel>(evento);
                CreatedAtAction("GetEvento", new { eventoId = eventoModel.EventoId }, eventoModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = eventoModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = eventoModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutEvento
        // PUT: api/Eventos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{eventoId}")]
        public async Task<ActionResult<ResultResponse>> PutEvento(int eventoId, EventoModel eventoModel)
        {
            try
            {
                if (eventoId != eventoModel.EventoId) return BadRequest();

                var evento = _mapper.Map<Evento>(eventoModel);
                await _eventoService.UpdateAsync(eventoId, evento);

                eventoModel = _mapper.Map<EventoModel>(evento);
                CreatedAtAction("GetEvento", new { eventoId = eventoModel.EventoId }, eventoModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = eventoModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex)
            {
                return (new ResultResponse()
                {
                    Succeeded = false,
                    ObjectRetorno = eventoModel,
                    ObjectResult = (int)EObjectResult.BadRequest,
                    Errors = new List<string>() { ex.Message }
                });
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        

        #region DeleteEvento
        // DELETE: api/Eventos/5
        [HttpDelete("{eventoId}")]
        public async Task<ActionResult<ResultResponse>> DeleteEvento(int eventoId)
        {
            try
            {
                await _eventoService.RemoveAsync(eventoId);
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
    }
}
