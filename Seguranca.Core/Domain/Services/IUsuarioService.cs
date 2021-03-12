using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ObterAsync();
        Task<Usuario> ObterAsync(int usuarioId);
        Task InsereAsync(Usuario usuario);
        Task UpdateAsync(int usuarioId, Usuario usuario);
        Task RemoveAsync(int usuarioId);
    }
}
