using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IPerfilService : IService<Perfil>
    {
        Task<Perfil> GetFullAsync(string nome);
        Task<IEnumerable<Perfil>> GetFullAsync();
        Task<Perfil> GetFullAsync(int id);       
    }
}
