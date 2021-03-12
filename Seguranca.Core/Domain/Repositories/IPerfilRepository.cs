using Seguranca.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Repositories
{
    public interface IPerfilRepository : IRepository
    {
        Task<IEnumerable<Perfil>> ObterAsync();
        Task<Perfil> ObterAsync(int perfilId);
        void Insere(Perfil perfil);
        void Update(Perfil perfil);
        void Remove(Perfil perfilId);
        bool Exists(int perfilId);
    }
}
