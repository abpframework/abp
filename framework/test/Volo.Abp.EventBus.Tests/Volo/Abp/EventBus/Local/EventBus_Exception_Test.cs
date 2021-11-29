using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_Exception_Test : EventBusTestBase
{
    [Fact]
    public async Task Should_Throw_Single_Exception_If_Only_One_Of_Handlers_Fails()
    {
        LocalEventBus.Subscribe<MySimpleEventData>(eventData => throw new Exception("This exception is intentionally thrown!"));

        var appException = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await LocalEventBus.PublishAsync(new MySimpleEventData(1));
        });

        appException.Message.ShouldBe("This exception is intentionally thrown!");
    }

    [Fact]
    public async Task Should_Throw_Aggregate_Exception_If_More_Than_One_Of_Handlers_Fail()
    {
        LocalEventBus.Subscribe<MySimpleEventData>(
            eventData => throw new Exception("This exception is intentionally thrown #1!"));

        LocalEventBus.Subscribe<MySimpleEventData>(
            eventData => throw new Exception("This exception is intentionally thrown #2!"));

        var aggrException = await Assert.ThrowsAsync<AggregateException>(async () =>
        {
            await LocalEventBus.PublishAsync(new MySimpleEventData(1));
        });

        aggrException.InnerExceptions.Count.ShouldBe(2);
        aggrException.InnerExceptions[0].Message.ShouldBe("This exception is intentionally thrown #1!");
        aggrException.InnerExceptions[1].Message.ShouldBe("This exception is intentionally thrown #2!");
    }
}
