using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<EventoModel>>> GetEvento()
        {
            try
            {
                var eventos = await _eventoService.ObterAsync();
                return _mapper.Map<List<EventoModel>>(eventos);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Eventos/5
        [HttpGet("{eventoId}")]
        public async Task<ActionResult<EventoModel>> GetEvento(int eventoId)
        {
            try
            {
                var evento = await _eventoService.ObterAsync(eventoId);
                return _mapper.Map<EventoModel>(evento);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("EventoId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostEvento
        // POST: api/Eventos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventoModel>> PostEvento(EventoModel eventoModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var evento = _mapper.Map<Evento>(eventoModel);
                await _eventoService.InsereAsync(evento);

                eventoModel = _mapper.Map<EventoModel>(evento);
                return CreatedAtAction("GetEvento", new { eventoId = eventoModel.EventoId }, eventoModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("EventoId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutEvento
        // PUT: api/Eventos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> PutEvento(int eventoId, EventoModel eventoModel)
        {
            try
            {
                if (eventoId != eventoModel.EventoId) return BadRequest();

                var evento = _mapper.Map<Evento>(eventoModel);
                await _eventoService.UpdateAsync(eventoId, evento);

                eventoModel = _mapper.Map<EventoModel>(evento);
                return CreatedAtAction("GetEvento", new { eventoId = eventoModel.EventoId }, eventoModel);
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("EventoId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        

        #region DeleteEvento
        // DELETE: api/Eventos/5
        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> DeleteEvento(int eventoId)
        {
            try
            {
                await _eventoService.RemoveAsync(eventoId);
                return NoContent();
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError("EventoId", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
