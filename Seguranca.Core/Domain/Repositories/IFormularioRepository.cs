using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Repositories
{
    public interface IFormularioRepository : IRepository
    {
        Task<IEnumerable<Formulario>> ObterAsync();
        Task<Formulario> ObterAsync(int formularioId);
        void Insere(Formulario formulario);
        void Update(Formulario formulario);
        void Remove(Formulario formularioId);
        bool Exists(int formularioId);
    }
}
