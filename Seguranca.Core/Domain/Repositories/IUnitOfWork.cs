using System.Threading.Tasks;

namespace Seguranca.Core.Domain.Repositories
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
