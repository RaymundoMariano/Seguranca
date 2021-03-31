using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IRepository<T> : IUnit where T : Entity
    {
        Task<IEnumerable<T>> ObterAsync();
        Task<T> ObterAsync(int id);
        void Insere(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
