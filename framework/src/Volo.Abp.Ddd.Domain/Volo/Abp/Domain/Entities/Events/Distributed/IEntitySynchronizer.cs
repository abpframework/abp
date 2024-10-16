using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

public interface IEntitySynchronizer<TSourceEntityEto>
{
    Task<bool> TryCreateOrUpdateEntityAsync(TSourceEntityEto eto, CancellationToken cancellationToken = default);

    Task<bool> TryDeleteEntityAsync(TSourceEntityEto eto, CancellationToken cancellationToken = default);
}