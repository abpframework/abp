using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Distributed;

public class DistributedEventBus_NamedSubscribe_Test : LocalDistributedEventBusTestBase
{
    [Fact]
    public async Task Should_Call_Named_Handler()
    {
        var handler = new DictionaryDistributedEventHandler();
        DistributedEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler);

        await DistributedEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object>
        {
            ["OrderId"] = 123
        });

        handler.HandleCount.ShouldBe(1);
        handler.LastReceivedData.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Route_Same_PayloadType_By_Name()
    {
        var orderCreatedHandler = new DictionaryDistributedEventHandler();
        var orderCancelledHandler = new DictionaryDistributedEventHandler();

        DistributedEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", orderCreatedHandler);
        DistributedEventBus.Subscribe<Dictionary<string, object>>("OrderCancelled", orderCancelledHandler);

        await DistributedEventBus.PublishByNameAsync("OrderCreated", new Dictionary<string, object>
        {
            ["OrderId"] = 1
        });

        await DistributedEventBus.PublishByNameAsync("OrderCancelled", new Dictionary<string, object>
        {
            ["OrderId"] = 2
        });

        orderCreatedHandler.HandleCount.ShouldBe(1);
        orderCancelledHandler.HandleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Work_With_UnitOfWork()
    {
        var handler = new DictionaryDistributedEventHandler();
        DistributedEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler);

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            await DistributedEventBus.PublishByNameAsync(
                "OrderCreated",
                new Dictionary<string, object> { ["OrderId"] = 1 },
                onUnitOfWorkComplete: true
            );

            handler.HandleCount.ShouldBe(0);

            await uow.CompleteAsync();

            handler.HandleCount.ShouldBe(1);
        }
    }

    [Fact]
    public void Should_Throw_When_Same_Name_Different_Type()
    {
        DistributedEventBus.Subscribe<NamedEventTestData>(
            new SingleInstanceHandlerFactory(new NamedEventTestDataHandler()));

        var exception = Assert.Throws<AbpException>(() =>
        {
            DistributedEventBus.Subscribe("MyNamedEvent", typeof(Dictionary<string, object>),
                new SingleInstanceHandlerFactory(new DictionaryDistributedEventHandler()));
        });

        exception.Message.ShouldContain("MyNamedEvent");
        exception.Message.ShouldContain("already mapped");
    }

    [Fact]
    public async Task Should_Not_Trigger_Named_Handler_When_Publishing_By_Type()
    {
        var handler = new DictionaryDistributedEventHandler();
        DistributedEventBus.Subscribe<Dictionary<string, object>>("OrderCreated", handler);

        await DistributedEventBus.PublishAsync(
            typeof(Dictionary<string, object>),
            new Dictionary<string, object> { ["OrderId"] = 1 },
            onUnitOfWorkComplete: false,
            useOutbox: false
        );

        handler.HandleCount.ShouldBe(0);
    }

    private class NamedEventTestDataHandler : IDistributedEventHandler<NamedEventTestData>
    {
        public Task HandleEventAsync(NamedEventTestData eventData)
        {
            return Task.CompletedTask;
        }
    }

    private class DictionaryDistributedEventHandler : IDistributedEventHandler<Dictionary<string, object>>
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
