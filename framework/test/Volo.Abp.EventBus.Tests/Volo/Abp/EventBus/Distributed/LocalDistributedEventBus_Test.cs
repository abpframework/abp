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

            await DistributedEventBus.PublishAsync(new MySimpleEventData(1)).ConfigureAwait(false);
            await DistributedEventBus.PublishAsync(new MySimpleEventData(2)).ConfigureAwait(false);
            await DistributedEventBus.PublishAsync(new MySimpleEventData(3)).ConfigureAwait(false);

            Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
            Assert.Equal(3, MySimpleDistributedTransientEventHandler.DisposeCount);
        }
    }
}
