using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IModuloService : IService<Modulo>
    {
        Task<IEnumerable<Modulo>> GetFullAsync();
        Task<Modulo> GetFullAsync(int moduloId);
        Task<Modulo> GetFullAsync(string nome);
    }
}
