using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed;

public class DistributedEventHandles : ILocalEventHandler<DistributedEventSent>, ILocalEventHandler<DistributedEventReceived>
{
    public static int SentCount { get; set; }

    public static int ReceivedCount { get; set; }

    public Task HandleEventAsync(DistributedEventSent eventData)
    {
        SentCount++;
        return Task.CompletedTask;
    }

    public Task HandleEventAsync(DistributedEventReceived eventData)
    {
        ReceivedCount++;
        return Task.CompletedTask;
    }
}
