using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class TransientDisposableEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public async Task Should_Call_Handler_AndDispose()
        {
            EventBus.Register<MySimpleEventData, MySimpleTransientEventHandler>();
            EventBus.Register<MySimpleEventData, MySimpleTransientAsyncEventHandler>();

            await EventBus.TriggerAsync(new MySimpleEventData(1));
            await EventBus.TriggerAsync(new MySimpleEventData(2));
            await EventBus.TriggerAsync(new MySimpleEventData(3));

            Assert.Equal(3, MySimpleTransientEventHandler.HandleCount);
            Assert.Equal(3, MySimpleTransientEventHandler.DisposeCount);

            Assert.Equal(3, MySimpleTransientAsyncEventHandler.HandleCount);
            Assert.Equal(3, MySimpleTransientAsyncEventHandler.DisposeCount);
        }
    }
}