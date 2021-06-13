using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObterAsync();
        Task<Usuario> ObterAsync(int id);
        Task InsereAsync(Usuario usuario);
        Task<Usuario> ObterAsync(string email);
        Task<ResultResponse> ObterRestricoesAsync(int usuarioId);
        Task<ResultResponse> AtualizarRestricoesAsync(int usuarioId, List<RestricaoUsuario> restricoes);
        Task<ResultResponse> ObterPerfisAsync(int usuarioId);
        Task<ResultResponse> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuario> perfis);
    }
}
