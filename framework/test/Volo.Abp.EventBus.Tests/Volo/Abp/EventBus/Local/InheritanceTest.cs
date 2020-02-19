using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus.Local
{
    public class InheritanceTest : EventBusTestBase
    {
        [Fact]
        public async Task Should_Handle_Events_For_Derived_Classes()
        {
            var totalData = 0;

            LocalEventBus.Subscribe<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            await LocalEventBus.PublishAsync(new MySimpleEventData(1)); //Should handle directly registered class
            await LocalEventBus.PublishAsync(new MySimpleEventData(2)); //Should handle directly registered class
            await LocalEventBus.PublishAsync(new MyDerivedEventData(3)); //Should handle derived class too
            await LocalEventBus.PublishAsync(new MyDerivedEventData(4)); //Should handle derived class too

            Assert.Equal(10, totalData);
        }

        [Fact]
        public async Task Should_Not_Handle_Events_For_Base_Classes()
        {
            var totalData = 0;

            LocalEventBus.Subscribe<MyDerivedEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            await LocalEventBus.PublishAsync(new MySimpleEventData(1)); //Should not handle
            await LocalEventBus.PublishAsync(new MySimpleEventData(2)); //Should not handle
            await LocalEventBus.PublishAsync(new MyDerivedEventData(3)); //Should handle
            await LocalEventBus.PublishAsync(new MyDerivedEventData(4)); //Should handle

            Assert.Equal(7, totalData);
        }   
    }
}