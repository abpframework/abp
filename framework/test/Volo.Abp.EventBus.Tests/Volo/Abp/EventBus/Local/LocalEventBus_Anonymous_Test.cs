using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class LocalEventBus_Anonymous_Test : EventBusTestBase
{
    [Fact]
    public async Task Should_Handle_Anonymous_Handler_With_EventName()
    {
        var handleCount = 0;

        LocalEventBus.Subscribe("TestEvent",
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                handleCount++;
                d.EventName.ShouldBe("TestEvent");
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync("TestEvent", new { Value = 1 });
        await LocalEventBus.PublishAsync("TestEvent", new { Value = 2 });

        handleCount.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_EventName()
    {
        var handleCount = 0;

        LocalEventBus.Subscribe<MySimpleEventData>(async (data) =>
        {
            handleCount++;
            await Task.CompletedTask;
        });

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();
        await LocalEventBus.PublishAsync(eventName, new MySimpleEventData(42));

        handleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Convert_Dictionary_To_Typed_Handler()
    {
        MySimpleEventData? receivedData = null;

        LocalEventBus.Subscribe<MySimpleEventData>(async (data) =>
        {
            receivedData = data;
            await Task.CompletedTask;
        });

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();
        await LocalEventBus.PublishAsync(eventName, new Dictionary<string, object>
        {
            { "Value", 42 }
        });

        receivedData.ShouldNotBeNull();
        receivedData.Value.ShouldBe(42);
    }

    [Fact]
    public async Task Should_Trigger_Both_Typed_And_Anonymous_Handlers()
    {
        var typedHandleCount = 0;
        var anonymousHandleCount = 0;

        LocalEventBus.Subscribe<MySimpleEventData>(async (data) =>
        {
            typedHandleCount++;
            await Task.CompletedTask;
        });

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        LocalEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                anonymousHandleCount++;
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync(new MySimpleEventData(1));

        typedHandleCount.ShouldBe(1);
        anonymousHandleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Unsubscribe_Anonymous_Handler()
    {
        var handleCount = 0;

        var handler = new ActionEventHandler<AnonymousEventData>(async (d) =>
        {
            handleCount++;
            await Task.CompletedTask;
        });
        var factory = new SingleInstanceHandlerFactory(handler);

        var disposable = LocalEventBus.Subscribe("TestEvent", factory);

        await LocalEventBus.PublishAsync("TestEvent", new { Value = 1 });
        handleCount.ShouldBe(1);

        disposable.Dispose();

        await Assert.ThrowsAsync<AbpException>(() =>
            LocalEventBus.PublishAsync("TestEvent", new { Value = 2 }));
        handleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Throw_For_Unknown_Event_Name()
    {
        await Assert.ThrowsAsync<AbpException>(() =>
            LocalEventBus.PublishAsync("NonExistentEvent", new { Value = 1 }));
    }

    [Fact]
    public async Task Should_ConvertToTypedObject_In_Anonymous_Handler()
    {
        object? receivedData = null;

        LocalEventBus.Subscribe("TestEvent",
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                receivedData = d.ConvertToTypedObject();
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync("TestEvent", new { Name = "Hello", Count = 42 });

        receivedData.ShouldNotBeNull();
        var dict = receivedData.ShouldBeOfType<Dictionary<string, object?>>();
        dict["Name"].ShouldBe("Hello");
        dict["Count"].ShouldBe(42L);
    }

    [Fact]
    public async Task Should_ConvertToTypedObject_Generic_In_Anonymous_Handler()
    {
        MySimpleEventData? receivedData = null;

        LocalEventBus.Subscribe("TestEvent",
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                receivedData = d.ConvertToTypedObject<MySimpleEventData>();
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync("TestEvent", new MySimpleEventData(99));

        receivedData.ShouldNotBeNull();
        receivedData.Value.ShouldBe(99);
    }
}
