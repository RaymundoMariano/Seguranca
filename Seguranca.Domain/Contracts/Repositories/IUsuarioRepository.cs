using Seguranca.Domain.Contracts.Repositories.Seedwork;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IUsuarioRepository : IUnit
    {
        Task<IEnumerable<Usuario>> ObterAsync();
        Task<Usuario> ObterAsync(int id);
        void Insere(Usuario usuario);
        Task<IEnumerable<Usuario>> ObterAsyncFull();
        Task<Usuario> ObterAsyncFull(int usuarioId);
        Task<Usuario> ObterAsync(string email);
    }
}
