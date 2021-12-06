using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow;

public interface ISupportsSavingChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
