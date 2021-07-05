using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IPerfilUsuarioService : IService<PerfilUsuario>
    {
        Task<PerfilUsuario> ObterAsync(int usuarioId, int? moduloId);
        Task<ResultResponse> ObterPerfisAsync(int usuarioId);
        Task<ResultResponse> AtualizarPerfisAsync(int usuarioId, List<PerfilUsuario> perfis);
    }
}
