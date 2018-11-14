using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus.Distributed
{
    public class LocalDistributedEventBus_Test : LocalDistributedEventBusTestBase
    {
        [Fact]
        public async Task Should_Call_Handler_AndDispose()
        {
            LocalEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

            await LocalEventBus.PublishAsync(new MySimpleEventData(1));
            await LocalEventBus.PublishAsync(new MySimpleEventData(2));
            await LocalEventBus.PublishAsync(new MySimpleEventData(3));

            Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
            Assert.Equal(3, MySimpleDistributedTransientEventHandler.DisposeCount);
        }
    }
}
