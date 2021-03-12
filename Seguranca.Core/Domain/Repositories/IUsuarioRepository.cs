using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository
    {
        Task<IEnumerable<Usuario>> ObterAsync();
        Task<Usuario> ObterAsync(int usuarioId);
        void Insere(Usuario usuario);
        void Update(Usuario usuario);
        void Remove(Usuario usuarioId);
        bool Exists(int usuarioId);
    }
}
