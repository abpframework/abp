using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class LocalEventBus_Dynamic_Test : EventBusTestBase
{
    [Fact]
    public async Task Should_Handle_Dynamic_Handler_With_EventName()
    {
        var handleCount = 0;
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = LocalEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                handleCount++;
                d.EventName.ShouldBe(eventName);
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync(eventName, new { Value = 1 });
        await LocalEventBus.PublishAsync(eventName, new { Value = 2 });

        handleCount.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_EventName()
    {
        var handleCount = 0;

        using var subscription = LocalEventBus.Subscribe<MySimpleEventData>(async (data) =>
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

        using var subscription = LocalEventBus.Subscribe<MySimpleEventData>(async (data) =>
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
    public async Task Should_Trigger_Both_Typed_And_Dynamic_Handlers()
    {
        var typedHandleCount = 0;
        var dynamicHandleCount = 0;

        using var typedSubscription = LocalEventBus.Subscribe<MySimpleEventData>(async (data) =>
        {
            typedHandleCount++;
            await Task.CompletedTask;
        });

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        using var dynamicSubscription = LocalEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                dynamicHandleCount++;
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync(new MySimpleEventData(1));

        typedHandleCount.ShouldBe(1);
        dynamicHandleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Unsubscribe_Dynamic_Handler()
    {
        var handleCount = 0;
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");

        var handler = new ActionEventHandler<DynamicEventData>(async (d) =>
        {
            handleCount++;
            await Task.CompletedTask;
        });
        var factory = new SingleInstanceHandlerFactory(handler);

        var disposable = LocalEventBus.Subscribe(eventName, factory);

        await LocalEventBus.PublishAsync(eventName, new { Value = 1 });
        handleCount.ShouldBe(1);

        disposable.Dispose();

        await LocalEventBus.PublishAsync(eventName, new { Value = 2 });
        handleCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Not_Throw_For_Unknown_Event_Name()
    {
        // Publishing to an unknown event name should not throw (consistent with typed PublishAsync behavior)
        await LocalEventBus.PublishAsync("NonExistentEvent", new { Value = 1 });
    }

    [Fact]
    public async Task Should_Access_Data_In_Dynamic_Handler()
    {
        object? receivedData = null;
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = LocalEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                receivedData = d.Data;
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync(eventName, new { Name = "Hello", Count = 42 });

        receivedData.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Receive_Typed_Data_Via_Typed_Handler_From_Dynamic_Publish()
    {
        MySimpleEventData? receivedData = null;
        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        using var subscription = LocalEventBus.Subscribe<MySimpleEventData>(async (d) =>
        {
            receivedData = d;
            await Task.CompletedTask;
        });

        await LocalEventBus.PublishAsync(eventName, new MySimpleEventData(99));

        receivedData.ShouldNotBeNull();
        receivedData.Value.ShouldBe(99);
    }

    [Fact]
    public async Task Should_Unsubscribe_All_Dynamic_Handlers()
    {
        var handleCount = 0;
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");

        LocalEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                handleCount++;
                await Task.CompletedTask;
            })));

        LocalEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                handleCount++;
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync(eventName, new { Value = 1 });
        handleCount.ShouldBe(2);

        LocalEventBus.UnsubscribeAll(eventName);

        // After UnsubscribeAll, publishing still works (key exists) but no handlers are invoked
        await LocalEventBus.PublishAsync(eventName, new { Value = 2 });
        handleCount.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Handle_Multiple_Dynamic_Events_Independently()
    {
        var countA = 0;
        var countB = 0;
        var eventNameA = "EventA-" + Guid.NewGuid().ToString("N");
        var eventNameB = "EventB-" + Guid.NewGuid().ToString("N");

        using var subA = LocalEventBus.Subscribe(eventNameA,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                countA++;
                await Task.CompletedTask;
            })));

        using var subB = LocalEventBus.Subscribe(eventNameB,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                countB++;
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync(eventNameA, new { Value = 1 });
        await LocalEventBus.PublishAsync(eventNameB, new { Value = 2 });
        await LocalEventBus.PublishAsync(eventNameA, new { Value = 3 });

        countA.ShouldBe(2);
        countB.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Receive_EventName_In_DynamicEventData()
    {
        string? receivedEventName = null;
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = LocalEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                receivedEventName = d.EventName;
                await Task.CompletedTask;
            })));

        await LocalEventBus.PublishAsync(eventName, new { Value = 1 });

        receivedEventName.ShouldBe(eventName);
    }

    [Fact]
    public async Task Should_Convert_Anonymous_Object_To_Typed_Handler()
    {
        MySimpleEventData? receivedData = null;
        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        using var subscription = LocalEventBus.Subscribe<MySimpleEventData>(async (d) =>
        {
            receivedData = d;
            await Task.CompletedTask;
        });

        await LocalEventBus.PublishAsync(eventName, new { Value = 77 });

        receivedData.ShouldNotBeNull();
        receivedData.Value.ShouldBe(77);
    }
}
