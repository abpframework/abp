using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus.Distributed
{
    public class LocalDistributedEventBus_Test : LocalDistributedEventBusTestBase
    {
        [Fact]
        public async Task Should_Call_Handler_AndDispose()
        {
            DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

            await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
            await DistributedEventBus.PublishAsync(new MySimpleEventData(2));
            await DistributedEventBus.PublishAsync(new MySimpleEventData(3));

            Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
            Assert.Equal(3, MySimpleDistributedTransientEventHandler.DisposeCount);
        }
    }
}
