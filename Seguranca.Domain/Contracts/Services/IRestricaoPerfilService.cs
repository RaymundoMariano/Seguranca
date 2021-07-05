using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IRestricaoPerfilService : IService<RestricaoPerfil>
    {
        Task<ResultResponse> ObterRestricoesAsync(int usuarioId);
        Task<ResultResponse> AtualizarRestricoesAsync(int usuarioId, List<RestricaoPerfil> restricoes);
    }
}
