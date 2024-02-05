using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.MultiQueue.Subscriber;

namespace Volo.Abp.MultiQueue;

[QueueHandler(MultiQueueTestConst.ConfigKey, MultiQueueTestConst.Topic, typeof(JsonQueueResult<ReceiveEventData>))]
public class JsonReceiveHandler : ILocalEventHandler<JsonQueueResult<ReceiveEventData>>, ISingletonDependency
{

    public static ReceiveEventData Result;
    public static int ReceiveCount;

    public async Task HandleEventAsync(JsonQueueResult<ReceiveEventData> eventData)
    {
        ReceiveCount++;
        Result = eventData.Data;
        await Task.CompletedTask;
    }
}
