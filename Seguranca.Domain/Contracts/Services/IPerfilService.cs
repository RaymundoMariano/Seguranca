using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IPerfilService : IService<Perfil>
    {
        Task<ResultResponse> ObterRestricoesAsync(int perfilId);
        Task<ResultResponse> AtualizarRestricoesAsync(int perfilId, List<RestricaoPerfil> restricoes);
    }
}
