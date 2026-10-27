using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_Concurrency_Test : EventBusTestBase
{
    private const int PublishCount = 10_000;

    [Fact]
    public async Task Should_Publish_While_Other_Handlers_Subscribe_And_Unsubscribe()
    {
        var handledCount = 0;
        using var subscription = LocalEventBus.Subscribe<MySimpleEventData>(_ =>
        {
            Interlocked.Increment(ref handledCount);
            return Task.CompletedTask;
        });

        using var cts = new CancellationTokenSource();
        var churn = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                LocalEventBus.Subscribe<MySimpleEventData>(_ => Task.CompletedTask).Dispose();
            }
        });

        try
        {
            for (var i = 0; i < PublishCount; i++)
            {
                await LocalEventBus.PublishAsync(new MySimpleEventData(1), onUnitOfWorkComplete: false);
            }
        }
        finally
        {
            cts.Cancel();
            await churn;
        }

        Assert.Equal(PublishCount, handledCount);
    }

    [Fact]
    public async Task Should_Publish_Dynamic_Event_While_Other_Handlers_Subscribe_And_Unsubscribe()
    {
        const string eventName = "MyConcurrencyTestDynamicEvent";
        var handledCount = 0;
        using var subscription = LocalEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(
            new ActionEventHandler<DynamicEventData>(_ =>
            {
                Interlocked.Increment(ref handledCount);
                return Task.CompletedTask;
            })));

        using var cts = new CancellationTokenSource();
        var churn = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                LocalEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(
                    new ActionEventHandler<DynamicEventData>(_ => Task.CompletedTask))).Dispose();
            }
        });

        try
        {
            for (var i = 0; i < PublishCount; i++)
            {
                await LocalEventBus.PublishAsync(eventName, new { Value = 1 }, onUnitOfWorkComplete: false);
            }
        }
        finally
        {
            cts.Cancel();
            await churn;
        }

        Assert.Equal(PublishCount, handledCount);
    }
}
