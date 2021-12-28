using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class ActionBasedEventHandlerTest : EventBusTestBase
{
    [Fact]
    public async Task Should_Call_Action_On_Event_With_Correct_Source()
    {
        var totalData = 0;

        LocalEventBus.Subscribe<MySimpleEventData>(
            eventData =>
            {
                totalData += eventData.Value;
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new MySimpleEventData(1));
        await LocalEventBus.PublishAsync(new MySimpleEventData(2));
        await LocalEventBus.PublishAsync(new MySimpleEventData(3));
        await LocalEventBus.PublishAsync(new MySimpleEventData(4));

        Assert.Equal(10, totalData);
    }

    [Fact]
    public async Task Should_Call_Handler_With_Non_Generic_Trigger()
    {
        var totalData = 0;

        LocalEventBus.Subscribe<MySimpleEventData>(
            eventData =>
            {
                totalData += eventData.Value;
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(typeof(MySimpleEventData), new MySimpleEventData(1));
        await LocalEventBus.PublishAsync(typeof(MySimpleEventData), new MySimpleEventData(2));
        await LocalEventBus.PublishAsync(typeof(MySimpleEventData), new MySimpleEventData(3));
        await LocalEventBus.PublishAsync(typeof(MySimpleEventData), new MySimpleEventData(4));

        Assert.Equal(10, totalData);
    }

    [Fact]
    public async Task Should_Not_Call_Action_After_Unregister_1()
    {
        var totalData = 0;

        var registerDisposer = LocalEventBus.Subscribe<MySimpleEventData>(
            eventData =>
            {
                totalData += eventData.Value;
                return Task.CompletedTask;
            });

        await LocalEventBus.PublishAsync(new MySimpleEventData(1));
        await LocalEventBus.PublishAsync(new MySimpleEventData(2));
        await LocalEventBus.PublishAsync(new MySimpleEventData(3));

        registerDisposer.Dispose();

        await LocalEventBus.PublishAsync(new MySimpleEventData(4));

        Assert.Equal(6, totalData);
    }

    [Fact]
    public async Task Should_Not_Call_Action_After_Unregister_2()
    {
        var totalData = 0;

        var action = new Func<MySimpleEventData, Task>(
            eventData =>
            {
                totalData += eventData.Value;
                return Task.CompletedTask;
            });

        LocalEventBus.Subscribe(action);

        await LocalEventBus.PublishAsync(new MySimpleEventData(1));
        await LocalEventBus.PublishAsync(new MySimpleEventData(2));
        await LocalEventBus.PublishAsync(new MySimpleEventData(3));

        LocalEventBus.Unsubscribe(action);

        await LocalEventBus.PublishAsync(new MySimpleEventData(4));

        Assert.Equal(6, totalData);
    }

    [Fact]
    public async Task Should_Call_Action_On_Event_With_Correct_Source_Async()
    {
        int totalData = 0;

        LocalEventBus.Subscribe<MySimpleEventData>(
            async eventData =>
            {
                await Task.Delay(20);
                Interlocked.Add(ref totalData, eventData.Value);
                await Task.Delay(20);
            });

        await LocalEventBus.PublishAsync(new MySimpleEventData(1));
        await LocalEventBus.PublishAsync(new MySimpleEventData(2));
        await LocalEventBus.PublishAsync(new MySimpleEventData(3));
        await LocalEventBus.PublishAsync(new MySimpleEventData(4));

        Assert.Equal(10, totalData);
    }
}
