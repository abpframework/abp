using System.Threading.Tasks;
using Volo.Abp.EventBus.Boxes;

namespace Volo.Abp.EventBus.Distributed;

public interface IInboxProcessorProvider
{
    Task<IInboxProcessor> GetOrNullAsync(InboxConfig inboxConfig);
}
