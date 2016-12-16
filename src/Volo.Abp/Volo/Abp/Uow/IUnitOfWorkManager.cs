using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Current { get; }

        IUnitOfWork Begin();
    }

    public interface IDatabaseApi
    {
        Task SaveChangesAsync();

        Task CommitAsync();
    }
}