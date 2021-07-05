using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IFormularioEventoService : IService<FormularioEvento>
    {
        Task<ResultResponse> ObterEventosAsync(int formularioId);
        Task<ResultResponse> AtualizarEventosAsync(int formularioId, List<Evento> eventos);
    }
}
