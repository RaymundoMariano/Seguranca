using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IRestricaoUsuarioRepository : IRepository<RestricaoUsuario>
    {
        Task<RestricaoUsuario> ObterAsync(int usuarioId, int? moduloID, int? formularioId, int? eventoId);
    }
}
