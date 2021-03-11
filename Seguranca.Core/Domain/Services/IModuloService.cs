using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Services
{
    public interface IModuloService
    {
        Task<IEnumerable<Modulo>> ObterAsync();
        Task<Modulo> ObterAsync(int moduloId);
        Task InsereAsync(Modulo modulo);
        Task UpdateAsync(int moduloId, Modulo modulo);
        Task RemoveAsync(int moduloId);
    }
}
