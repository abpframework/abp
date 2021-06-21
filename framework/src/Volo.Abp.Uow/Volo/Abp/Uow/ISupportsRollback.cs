using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface ISupportsRollback
    {
        Task RollbackAsync(CancellationToken cancellationToken);
    }
}
