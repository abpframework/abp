using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus.Local
{
    public class TransientDisposableEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public async Task Should_Call_Handler_AndDispose()
        {
            LocalEventBus.Subscribe<MySimpleEventData, MySimpleTransientEventHandler>();

            await LocalEventBus.PublishAsync(new MySimpleEventData(1)).ConfigureAwait(false);
            await LocalEventBus.PublishAsync(new MySimpleEventData(2)).ConfigureAwait(false);
            await LocalEventBus.PublishAsync(new MySimpleEventData(3)).ConfigureAwait(false);

            Assert.Equal(3, MySimpleTransientEventHandler.HandleCount);
            Assert.Equal(3, MySimpleTransientEventHandler.DisposeCount);
        }
    }
}