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
        Task<Usuario> ObterAsync(string email);
        Task<IEnumerable<Usuario>> GetFullAsync();
        Task<Usuario> GetFullAsync(int id);
        Task<Usuario> GetFullAsync(string nome);
        void Insere(Usuario usuario);
    }
}
