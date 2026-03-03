using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_NamedSubscribe_Test : EventBusTestBase
{
    [Fact]
    public async Task Should_Call_Named_Handler()
    {
        var handler = new DictionaryEventHandler();
        LocalEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler);

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object>
        {
            ["OrderId"] = 123,
            ["Amount"] = 99.99
        });

        handler.HandleCount.ShouldBe(1);
        handler.LastReceivedData.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Route_Same_PayloadType_To_Different_Handlers_By_Name()
    {
        var orderCreatedHandler = new DictionaryEventHandler();
        var orderCancelledHandler = new DictionaryEventHandler();

        LocalEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", orderCreatedHandler);
        LocalEventBus.Subscribe<Dictionary<string, object>>("OrderCancelled", orderCancelledHandler);

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object>
        {
            ["OrderId"] = 1
        });

        await LocalEventBus.PublishByNameAsync("OrderCancelled", new Dictionary<string, object>
        {
            ["OrderId"] = 2
        });

        orderCreatedHandler.HandleCount.ShouldBe(1);
        orderCancelledHandler.HandleCount.ShouldBe(1);

        orderCreatedHandler.LastReceivedData["OrderId"].ShouldBe(1);
        orderCancelledHandler.LastReceivedData["OrderId"].ShouldBe(2);
    }

    [Fact]
    public async Task Should_Unsubscribe_Named_Handler()
    {
        var handler = new DictionaryEventHandler();
        var factory = new SingleInstanceHandlerFactory(handler);

        LocalEventBus.Subscribe("OrderCreated", typeof(Dictionary<string, object>), factory);

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object> { ["x"] = 1 });
        handler.HandleCount.ShouldBe(1);

        LocalEventBus.Unsubscribe("OrderCreated", typeof(Dictionary<string, object>), factory);

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object> { ["x"] = 2 });
        handler.HandleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_UnsubscribeAll_Named()
    {
        var handler1 = new DictionaryEventHandler();
        var handler2 = new DictionaryEventHandler();

        LocalEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler1);
        LocalEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler2);

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object> { ["x"] = 1 });
        handler1.HandleCount.ShouldBe(1);
        handler2.HandleCount.ShouldBe(1);

        LocalEventBus.UnsubscribeAll("OrderCreated");

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object> { ["x"] = 2 });
        handler1.HandleCount.ShouldBe(1);
        handler2.HandleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Dispose_Unsubscribe_Named_Handler()
    {
        var handler = new DictionaryEventHandler();
        var disposable = LocalEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler);

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object> { ["x"] = 1 });
        handler.HandleCount.ShouldBe(1);

        disposable.Dispose();

        await LocalEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object> { ["x"] = 2 });
        handler.HandleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Not_Trigger_Named_Handler_When_Publishing_By_Type()
    {
        var handler = new DictionaryEventHandler();
        LocalEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler);

        await LocalEventBus.PublishAsync(
            typeof(Dictionary<string, object>),
            new Dictionary<string, object> { ["OrderId"] = 123 },
            onUnitOfWorkComplete: false
        );

        handler.HandleCount.ShouldBe(0);
    }

    private class DictionaryEventHandler : ILocalEventHandler<Dictionary<string, object>>
    {
        public Dictionary<string, object>? LastReceivedData { get; private set; }
        public int HandleCount { get; private set; }

        public Task HandleEventAsync(Dictionary<string, object> eventData)
        {
            LastReceivedData = eventData;
            HandleCount++;
            return Task.CompletedTask;
        }
    }
}
