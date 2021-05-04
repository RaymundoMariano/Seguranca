using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Repositories.Seedwork
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
