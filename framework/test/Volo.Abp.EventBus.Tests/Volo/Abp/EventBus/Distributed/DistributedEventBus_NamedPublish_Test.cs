using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Distributed;

public class DistributedEventBus_NamedPublish_Test : LocalDistributedEventBusTestBase
{
    [Fact]
    public async Task Should_Trigger_Typed_Handler_By_EventName()
    {
        var handler = new NamedEventTestDataDistributedHandler();
        DistributedEventBus.Subscribe<NamedEventTestData>(new SingleInstanceHandlerFactory(handler));

        await DistributedEventBus.PublishByNameAsync("MyNamedEvent", new { Value = 42, Name = "test" });

        handler.HandleCount.ShouldBe(1);
        handler.LastReceivedData.ShouldNotBeNull();
        handler.LastReceivedData!.Value.ShouldBe(42);
        handler.LastReceivedData.Name.ShouldBe("test");
    }

    [Fact]
    public async Task Should_Trigger_Typed_Handler_By_FullTypeName()
    {
        var handler = new MySimpleEventDataDistributedHandler();
        DistributedEventBus.Subscribe<MySimpleEventData>(new SingleInstanceHandlerFactory(handler));

        await DistributedEventBus.PublishByNameAsync(
            "Volo.Abp.EventBus.MySimpleEventData",
            new { Value = 5 }
        );

        handler.HandleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Silently_Ignore_When_No_Handler()
    {
        await DistributedEventBus.PublishByNameAsync("NonExistentEvent", new { Foo = "bar" });
    }

    private class NamedEventTestDataDistributedHandler : IDistributedEventHandler<NamedEventTestData>
    {
        public NamedEventTestData? LastReceivedData { get; private set; }
        public int HandleCount { get; private set; }

        public Task HandleEventAsync(NamedEventTestData eventData)
        {
            LastReceivedData = eventData;
            HandleCount++;
            return Task.CompletedTask;
        }
    }

    private class MySimpleEventDataDistributedHandler : IDistributedEventHandler<MySimpleEventData>
    {
        public int HandleCount { get; private set; }

        public Task HandleEventAsync(MySimpleEventData eventData)
        {
            HandleCount++;
            return Task.CompletedTask;
        }
    }
}
