using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IModuloService : IService<Modulo>
    {
        Task<Modulo> ObterAsync(string nome);
    }
}
