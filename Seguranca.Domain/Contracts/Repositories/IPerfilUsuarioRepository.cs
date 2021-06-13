using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IPerfilUsuarioRepository : IRepository<PerfilUsuario>
    {
        Task<PerfilUsuario> ObterAsync(int usuarioId, int? moduloId);
    }
}
