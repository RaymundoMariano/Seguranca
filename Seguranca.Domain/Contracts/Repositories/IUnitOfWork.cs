using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
