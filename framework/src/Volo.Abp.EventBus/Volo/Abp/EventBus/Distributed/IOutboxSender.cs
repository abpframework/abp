using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus.Boxes;

public interface IOutboxSender
{
    Task StartAsync(OutboxConfig outboxConfig, CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);
}
