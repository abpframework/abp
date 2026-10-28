using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
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

        await PublishWhileChurningAsync(
            () => LocalEventBus.Subscribe<MySimpleEventData>(_ => Task.CompletedTask).Dispose(),
            () => LocalEventBus.PublishAsync(new MySimpleEventData(1), onUnitOfWorkComplete: false));

        handledCount.ShouldBe(PublishCount);
    }

    [Fact]
    public async Task Should_Publish_While_Dynamic_Handlers_Of_The_Same_Event_Subscribe_And_Unsubscribe()
    {
        var handledCount = 0;
        using var subscription = LocalEventBus.Subscribe<MySimpleEventData>(_ =>
        {
            Interlocked.Increment(ref handledCount);
            return Task.CompletedTask;
        });

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        await PublishWhileChurningAsync(
            () => LocalEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(
                new ActionEventHandler<DynamicEventData>(_ => Task.CompletedTask))).Dispose(),
            () => LocalEventBus.PublishAsync(new MySimpleEventData(1), onUnitOfWorkComplete: false));

        handledCount.ShouldBe(PublishCount);
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

        await PublishWhileChurningAsync(
            () => LocalEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(
                new ActionEventHandler<DynamicEventData>(_ => Task.CompletedTask))).Dispose(),
            () => LocalEventBus.PublishAsync(eventName, new { Value = 1 }, onUnitOfWorkComplete: false));

        handledCount.ShouldBe(PublishCount);
    }

    private static async Task PublishWhileChurningAsync(Action subscribeAndUnsubscribe, Func<Task> publish)
    {
        var churnStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var churnCount = 0;

        using var cts = new CancellationTokenSource();
        var churn = Task.Run(() =>
        {
            churnStarted.TrySetResult();
            while (!cts.IsCancellationRequested)
            {
                subscribeAndUnsubscribe();
                Interlocked.Increment(ref churnCount);
            }
        });

        try
        {
            await churnStarted.Task.WaitAsync(TimeSpan.FromSeconds(10));

            for (var i = 0; i < PublishCount; i++)
            {
                await publish();
            }
        }
        finally
        {
            cts.Cancel();
            await churn;
        }

        churnCount.ShouldBeGreaterThan(0);
    }
}
