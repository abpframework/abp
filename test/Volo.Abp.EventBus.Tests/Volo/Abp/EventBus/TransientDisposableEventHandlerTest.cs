using Xunit;

namespace Volo.Abp.EventBus
{
    public class TransientDisposableEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public void Should_Call_Handler_AndDispose()
        {
            EventBus.Register<MySimpleEventData, MySimpleTransientEventHandler>();
            EventBus.Register<MySimpleEventData, MySimpleTransientAsyncEventHandler>();

            EventBus.Trigger(new MySimpleEventData(1));
            EventBus.Trigger(new MySimpleEventData(2));
            EventBus.Trigger(new MySimpleEventData(3));

            Assert.Equal(3, MySimpleTransientEventHandler.HandleCount);
            Assert.Equal(3, MySimpleTransientEventHandler.DisposeCount);

            Assert.Equal(3, MySimpleTransientAsyncEventHandler.HandleCount);
            Assert.Equal(3, MySimpleTransientAsyncEventHandler.DisposeCount);
        }
    }
}