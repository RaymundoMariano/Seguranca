using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Services
{
    public interface IFormularioService
    {
        Task<IEnumerable<Formulario>> ObterAsync();
        Task<Formulario> ObterAsync(int formularioId);
        Task InsereAsync(Formulario formulario);
        Task UpdateAsync(int formularioId, Formulario formulario);
        Task RemoveAsync(int formularioId);
    }
}
