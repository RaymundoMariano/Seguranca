using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IFormularioRepository : IRepository<Formulario>
    {
        Task<IEnumerable<Formulario>> GetFullAsync();
        Task<Formulario> GetFullAsync(int id);        
    }
}
