using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Repositories.Seedwork;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class RepositoryEFC<T> : IRepository<T> where T : Entity
    {
        protected readonly SegurancaContextEFC _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;
        public RepositoryEFC(SegurancaContextEFC segurancaContext)
        {
            _segurancaContext = segurancaContext;
        }
                        
        #region ObterAsync
        public async Task<IEnumerable<T>> ObterAsync()
        {
            return await _segurancaContext.Set<T>().ToListAsync();
        }

        public async Task<T> ObterAsync(int id)
        {
            return await _segurancaContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region Insere
        public void Insere(T entity)
        {
            _segurancaContext.Set<T>().Add(entity);
        }
        #endregion

        #region Update
        public void Update(T entity)
        {
            _segurancaContext.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(T entity)
        {
            _segurancaContext.Set<T>().Remove(entity);
        }
        #endregion        
    }
}
