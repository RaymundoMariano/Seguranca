using Seguranca.Domain.Contracts.Repositories.Seedwork;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IFuncaoRepository : IUnit
    {
        Task<IEnumerable<Funcao>> GetFullAsync();
        Task<Funcao> GetFullAsync(int id);
    }
}
