using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface IDatabaseApi
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}