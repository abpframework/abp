using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface IDatabaseApi
    {
        Task SaveChangesAsync();

        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task CommitAsync(); //TODO: Add CancellationToken to CommitAsync?
    }
}