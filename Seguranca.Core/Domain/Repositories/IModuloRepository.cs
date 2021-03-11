using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Repositories
{
    public interface IModuloRepository : IRepository
    {
        Task<IEnumerable<Modulo>> ObterAsync();
        Task<Modulo> ObterAsync(int moduloId);
        void Insere(Modulo modulo);
        void Update(Modulo modulo);
        void Remove(Modulo moduloId);
        bool Exists(int moduloId);
    }
}
