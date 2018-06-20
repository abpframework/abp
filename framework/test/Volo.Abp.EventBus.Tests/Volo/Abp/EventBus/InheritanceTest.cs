using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class InheritanceTest : EventBusTestBase
    {
        [Fact]
        public async Task Should_Handle_Events_For_Derived_Classes()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            await EventBus.TriggerAsync(new MySimpleEventData(1)); //Should handle directly registered class
            await EventBus.TriggerAsync(new MySimpleEventData(2)); //Should handle directly registered class
            await EventBus.TriggerAsync(new MyDerivedEventData(3)); //Should handle derived class too
            await EventBus.TriggerAsync(new MyDerivedEventData(4)); //Should handle derived class too

            Assert.Equal(10, totalData);
        }

        [Fact]
        public async Task Should_Not_Handle_Events_For_Base_Classes()
        {
            var totalData = 0;

            EventBus.Register<MyDerivedEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            await EventBus.TriggerAsync(new MySimpleEventData(1)); //Should not handle
            await EventBus.TriggerAsync(new MySimpleEventData(2)); //Should not handle
            await EventBus.TriggerAsync(new MyDerivedEventData(3)); //Should handle
            await EventBus.TriggerAsync(new MyDerivedEventData(4)); //Should handle

            Assert.Equal(7, totalData);
        }   
    }
}