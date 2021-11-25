using System.Threading.Tasks;
using Rebus.Handlers;

namespace Volo.Abp.EventBus.Rebus;

public class RebusDistributedEventHandlerAdapter<TEventData> : IHandleMessages<TEventData>
{
    protected RebusDistributedEventBus RebusDistributedEventBus { get; }

    public RebusDistributedEventHandlerAdapter(RebusDistributedEventBus rebusDistributedEventBus)
    {
        RebusDistributedEventBus = rebusDistributedEventBus;
    }

    public async Task Handle(TEventData message)
    {
        await RebusDistributedEventBus.ProcessEventAsync(message.GetType(), message);
    }
}
