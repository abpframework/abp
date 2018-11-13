using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local
{
    public class EventBus_DI_Services_Test : EventBusTestBase
    {
        [Fact]
        public async Task Should_Automatically_Register_EventHandlers_From_Services()
        {
            await LocalEventBus.PublishAsync(new MySimpleEventData(1));
            await LocalEventBus.PublishAsync(new MySimpleEventData(2));
            await LocalEventBus.PublishAsync(new MySimpleEventData(3));
            await LocalEventBus.PublishAsync(new MySimpleEventData(4));

            GetRequiredService<MySimpleEventDataHandler>().TotalData.ShouldBe(10);
        }
    }
}
