using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Repositories.Seedwork;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class FuncaoRepositoryEFC : IFuncaoRepository
    {
        protected readonly SegurancaContextEFC _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;
        public FuncaoRepositoryEFC(SegurancaContextEFC segurancaContext)
        {
            _segurancaContext = segurancaContext;
        }

        #region GetFullAsync
        public async Task<IEnumerable<Funcao>> GetFullAsync()
        {
            return await _segurancaContext.Funcao
                    .AsNoTracking()
                    .Include(p => p.Perfil)
                    .ToListAsync();
        }

        public async Task<Funcao> GetFullAsync(int FuncaoId)
        {
            return await _segurancaContext.Funcao
                    .AsNoTracking()
                    .Include(p => p.Perfil)
                    .FirstOrDefaultAsync(p => p.FuncaoId == FuncaoId);
        }
        #endregion
    }
}
