using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_NamedPublish_Test : EventBusTestBase
{
    [Fact]
    public async Task Should_Trigger_Typed_Handler_By_EventName()
    {
        var handler = new NamedEventTestDataHandler();
        LocalEventBus.Subscribe<NamedEventTestData>(new SingleInstanceHandlerFactory(handler));

        await LocalEventBus.PublishByNameAsync("MyNamedEvent", new { Value = 42, Name = "test" });

        handler.HandleCount.ShouldBe(1);
        handler.LastReceivedData.ShouldNotBeNull();
        handler.LastReceivedData!.Value.ShouldBe(42);
        handler.LastReceivedData.Name.ShouldBe("test");
    }

    [Fact]
    public async Task Should_Convert_Dictionary_To_EventType()
    {
        var handler = new NamedEventTestDataHandler();
        LocalEventBus.Subscribe<NamedEventTestData>(new SingleInstanceHandlerFactory(handler));

        await LocalEventBus.PublishByNameAsync("MyNamedEvent", new Dictionary<string, object>
        {
            ["Value"] = 99,
            ["Name"] = "from-dict"
        });

        handler.HandleCount.ShouldBe(1);
        handler.LastReceivedData.ShouldNotBeNull();
        handler.LastReceivedData!.Value.ShouldBe(99);
        handler.LastReceivedData.Name.ShouldBe("from-dict");
    }

    [Fact]
    public async Task Should_Trigger_AutoRegistered_Typed_Handler_By_FullTypeName()
    {
        var handler = GetRequiredService<MySimpleEventDataHandler>();
        var initialTotal = handler.TotalData;

        await LocalEventBus.PublishByNameAsync(
            "Volo.Abp.EventBus.MySimpleEventData",
            new { Value = 10 }
        );

        handler.TotalData.ShouldBe(initialTotal + 10);
    }

    [Fact]
    public async Task Should_Silently_Ignore_When_No_Handler()
    {
        await LocalEventBus.PublishByNameAsync("NonExistentEvent", new { Foo = "bar" });
    }

    [Fact]
    public void Should_Throw_When_Same_Name_Different_Type()
    {
        LocalEventBus.Subscribe<NamedEventTestData>(new SingleInstanceHandlerFactory(new NamedEventTestDataHandler()));

        var exception = Assert.Throws<AbpException>(() =>
        {
            LocalEventBus.Subscribe("MyNamedEvent", typeof(Dictionary<string, object>),
                new SingleInstanceHandlerFactory(new DictionaryEventHandler()));
        });

        exception.Message.ShouldContain("MyNamedEvent");
        exception.Message.ShouldContain("already mapped");
    }

    private class NamedEventTestDataHandler : ILocalEventHandler<NamedEventTestData>
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
