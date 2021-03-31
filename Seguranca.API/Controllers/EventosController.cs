using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;

namespace Seguranca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        #region GetEvento
        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEvento()
        {
            try
            {
                var eventos = await _eventoService.ObterAsync();
                return eventos.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Eventos/5
        [HttpGet("{eventoId}")]
        public async Task<ActionResult<Evento>> GetEvento(int eventoId)
        {
            try
            {
                return await _eventoService.ObterAsync(eventoId);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutEvento
        // PUT: api/Eventos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{eventoId}")]
        public async Task<IActionResult> PutEvento(int eventoId, Evento evento)
        {
            try
            {
                if (eventoId != evento.EventoId) return BadRequest();
                await _eventoService.UpdateAsync(eventoId, evento);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostEvento
        // POST: api/Eventos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            try
            {
                await _eventoService.InsereAsync(evento);
                return CreatedAtAction("GetEvento", new { eventoId = evento.EventoId }, evento);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
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
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
