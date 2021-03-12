using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Services
{
    public interface IPerfilService
    {
        Task<IEnumerable<Perfil>> ObterAsync();
        Task<Perfil> ObterAsync(int perfilId);
        Task InsereAsync(Perfil perfil);
        Task UpdateAsync(int perfilId, Perfil perfil);
        Task RemoveAsync(int perfilId);
    }
}
