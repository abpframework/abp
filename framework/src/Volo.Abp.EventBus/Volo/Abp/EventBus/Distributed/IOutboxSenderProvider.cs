using System.Threading.Tasks;
using Volo.Abp.EventBus.Boxes;

namespace Volo.Abp.EventBus.Distributed;

public interface IOutboxSenderProvider
{
    Task<IOutboxSender> GetOrNullAsync(OutboxConfig outboxConfig);
}
