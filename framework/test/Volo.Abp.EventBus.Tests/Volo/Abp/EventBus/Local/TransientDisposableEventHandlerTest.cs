using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class TransientDisposableEventHandlerTest : EventBusTestBase
{
    [Fact]
    public async Task Should_Call_Handler_AndDispose()
    {
        LocalEventBus.Subscribe<MySimpleEventData, MySimpleTransientEventHandler>();

        await LocalEventBus.PublishAsync(new MySimpleEventData(1));
        await LocalEventBus.PublishAsync(new MySimpleEventData(2));
        await LocalEventBus.PublishAsync(new MySimpleEventData(3));

        Assert.Equal(3, MySimpleTransientEventHandler.HandleCount);
        Assert.Equal(3, MySimpleTransientEventHandler.DisposeCount);
    }
}
