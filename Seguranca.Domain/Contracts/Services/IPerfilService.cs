using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IPerfilService : IService<Perfil>
    {
        Task<Perfil> ObterAsync(string nome);       
    }
}
