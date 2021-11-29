using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus.Boxes
{
    public interface IInboxProcessor
    {
        Task StartAsync(InboxConfig inboxConfig, CancellationToken cancellationToken = default);
        
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}