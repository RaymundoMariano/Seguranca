using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IFormularioService : IService<Formulario>
    {
        Task<IEnumerable<Formulario>> GetFullAsync();
        Task<Formulario> GetFullAsync(int id);
    }
}
