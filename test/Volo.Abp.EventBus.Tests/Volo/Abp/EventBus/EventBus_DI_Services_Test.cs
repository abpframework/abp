using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class EventBus_DI_Services_Test : EventBusTestBase
    {
        [Fact]
        public async Task Should_Automatically_Register_EventHandlers_From_Services()
        {
            await EventBus.TriggerAsync(new MySimpleEventData(1));
            await EventBus.TriggerAsync(new MySimpleEventData(2));
            await EventBus.TriggerAsync(new MySimpleEventData(3));
            await EventBus.TriggerAsync(new MySimpleEventData(4));

            GetRequiredService<MySimpleEventDataHandler>().TotalData.ShouldBe(10);
            GetRequiredService<MySimpleAsyncEventDataHandler>().TotalData.ShouldBe(10);
        }
    }
}
