using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Services
{
    public interface IFuncaoService
    {
        Task<IEnumerable<Funcao>> GetFullAsync();
        Task<Funcao> GetFullAsync(int id);
        Task<IEnumerable<Funcao>> GetFullAsync(EFuncao eFuncaoInicial, EFuncao eFuncaoFinal);
    }
}
