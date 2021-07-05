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
        Task<Usuario> ObterAsync(string email);
        Task<IEnumerable<Usuario>> GetFullAsync();
        Task<Usuario> GetFullAsync(int id);
        Task<Usuario> GetFullAsync(string nome);
        Task InsereAsync(Usuario usuario);
        Task<SegurancaModel> ObterIds(string usuario, string modulo);
    }
}
