using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class EventBus_Exception_Test : EventBusTestBase
    {
        [Fact]
        public async Task Should_Throw_Single_Exception_If_Only_One_Of_Handlers_Fails()
        {
            EventBus.Register<MySimpleEventData>(eventData => throw new Exception("This exception is intentionally thrown!"));

            var appException = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await EventBus.TriggerAsync(new MySimpleEventData(1));
            });

            appException.Message.ShouldBe("This exception is intentionally thrown!");
        }

        [Fact]
        public async Task Should_Throw_Aggregate_Exception_If_More_Than_One_Of_Handlers_Fail()
        {
            EventBus.Register<MySimpleEventData>(
                eventData => throw new Exception("This exception is intentionally thrown #1!"));

            EventBus.Register<MySimpleEventData>(
                eventData => throw new Exception("This exception is intentionally thrown #2!"));

            var aggrException = await Assert.ThrowsAsync<AggregateException>(async () =>
            {
                await EventBus.TriggerAsync(new MySimpleEventData(1));
            });

            aggrException.InnerExceptions.Count.ShouldBe(2);
            aggrException.InnerExceptions[0].Message.ShouldBe("This exception is intentionally thrown #1!");
            aggrException.InnerExceptions[1].Message.ShouldBe("This exception is intentionally thrown #2!");
        }
    }
}