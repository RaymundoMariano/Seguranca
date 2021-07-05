using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IModuloFormularioService : IService<ModuloFormulario>
    {
        Task<ResultResponse> ObterFormulariosAsync(int moduloId);
        Task<ResultResponse> AtualizarFormulariosAsync(int moduloId, List<Formulario> fomularios);
    }
}
