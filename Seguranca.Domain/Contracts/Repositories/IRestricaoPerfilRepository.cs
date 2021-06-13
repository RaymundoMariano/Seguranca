using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IRestricaoPerfilRepository : IRepository<RestricaoPerfil>
    {
        Task<RestricaoPerfil> ObterAsync(int perfilId, int? moduloID, int? formularioId, int? eventoId);
    }
}
