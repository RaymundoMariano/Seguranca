using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IRestricaoUsuarioService : IService<RestricaoUsuario>
    {
        Task<ResultResponse> ObterRestricoesAsync(int usuarioId);
        Task<ResultResponse> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuario> restricoes);
    }
}
