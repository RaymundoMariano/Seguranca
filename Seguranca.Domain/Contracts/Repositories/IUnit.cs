namespace Seguranca.Domain.Contracts.Repositories
{
    public interface IUnit
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
