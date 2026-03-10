using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Distributed;

public class LocalDistributedEventBus_Test : LocalDistributedEventBusTestBase
{
    public LocalDistributedEventBus_Test()
    {
        MySimpleDistributedTransientEventHandler.HandleCount = 0;
        MySimpleDistributedTransientEventHandler.DisposeCount = 0;
        MySimpleDistributedSingleInstanceEventHandler.TenantId = null;
    }

    [Fact]
    public async Task Should_Call_Handler_AndDispose()
    {
        var handleCount = 0;
        var disposeCount = 0;
        var factory = new CountingDistributedEventHandlerFactory(
            () => handleCount++,
            () => disposeCount++);

        using var subscription = DistributedEventBus.Subscribe(typeof(MySimpleEventData), factory);

        await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(2));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(3));

        Assert.Equal(3, handleCount);
        Assert.Equal(3, disposeCount);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_EventName()
    {
        var handleCount = 0;
        var disposeCount = 0;
        var factory = new CountingDistributedEventHandlerFactory(
            () => handleCount++,
            () => disposeCount++);

        using var subscription = DistributedEventBus.Subscribe(typeof(MySimpleEventData), factory);

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();
        await DistributedEventBus.PublishAsync(eventName, new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        });
        await DistributedEventBus.PublishAsync(eventName, new { Value = 3 });

        Assert.Equal(3, handleCount);
        Assert.Equal(3, disposeCount);
    }

    [Fact]
    public async Task Should_Handle_Anonymous_Handler_When_Published_With_EventName()
    {
        var handleCount = 0;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = DistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                handleCount++;
                await Task.CompletedTask;
            })));

        await DistributedEventBus.PublishAsync(eventName, new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        });
        await DistributedEventBus.PublishAsync(eventName, new { Value = 3 });
        await DistributedEventBus.PublishAsync(eventName, new[] { 1, 2, 3 });

        Assert.Equal(4, handleCount);
    }

    [Fact]
    public async Task Should_Handle_Anonymous_Handler_When_Published_With_AnonymousEventData()
    {
        var handleCount = 0;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = DistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                handleCount++;
                AnonymousEventDataConverter.ConvertToLooseObject(d).ShouldNotBeNull();
                await Task.CompletedTask;
            })));

        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new MySimpleEventData(1)));
        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        }));
        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new { Value = 3 }));

        Assert.Equal(3, handleCount);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_AnonymousEventData()
    {
        using var subscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new MySimpleEventData(1)));
        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        }));
        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new { Value = 3 }));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
    }

    [Fact]
    public async Task Should_Trigger_Both_Typed_And_Anonymous_Handlers_For_Typed_Event()
    {
        using var typedSubscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        var anonymousHandleCount = 0;

        using var anonymousSubscription = DistributedEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
        {
            anonymousHandleCount++;
            await Task.CompletedTask;
        })));

        await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(2));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(3));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
        Assert.Equal(3, anonymousHandleCount);
    }

    [Fact]
    public async Task Should_Trigger_Both_Handlers_For_Mixed_Typed_And_Anonymous_Publish()
    {
        using var typedSubscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        var anonymousHandleCount = 0;

        using var anonymousSubscription = DistributedEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
        {
            anonymousHandleCount++;
            await Task.CompletedTask;
        })));

        await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        }));
        await DistributedEventBus.PublishAsync(new AnonymousEventData(eventName, new { Value = 3 }));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
        Assert.Equal(3, anonymousHandleCount);
    }

    [Fact]
    public async Task Should_Unsubscribe_Anonymous_Handler()
    {
        var handleCount = 0;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        var handler = new ActionEventHandler<AnonymousEventData>(async (d) =>
        {
            handleCount++;
            await Task.CompletedTask;
        });
        var factory = new SingleInstanceHandlerFactory(handler);

        var disposable = DistributedEventBus.Subscribe(eventName, factory);

        await DistributedEventBus.PublishAsync(eventName, new { Value = 1 });
        Assert.Equal(1, handleCount);

        disposable.Dispose();

        await DistributedEventBus.PublishAsync(eventName, new { Value = 2 });
        Assert.Equal(1, handleCount);
    }

    [Fact]
    public async Task Should_Ignore_Unknown_Event_Name()
    {
        await DistributedEventBus.PublishAsync("NonExistentEvent", new { Value = 1 });
    }

    [Fact]
    public async Task Should_Convert_AnonymousEventData_To_Typed_Object()
    {
        MySimpleEventData receivedData = null!;

        using var subscription = DistributedEventBus.Subscribe<MySimpleEventData>(async (data) =>
        {
            receivedData = data;
            await Task.CompletedTask;
        });

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();
        await DistributedEventBus.PublishAsync(eventName, new { Value = 42 });

        receivedData.ShouldNotBeNull();
        receivedData.Value.ShouldBe(42);
    }

    [Fact]
    public async Task Should_Rehydrate_Anonymous_Event_From_Outbox_Using_Raw_Json()
    {
        var localDistributedEventBus = GetRequiredService<LocalDistributedEventBus>();
        AnonymousEventData receivedData = null!;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = localDistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async data =>
            {
                receivedData = data;
                await Task.CompletedTask;
            })));

        var outgoingEvent = new OutgoingEventInfo(
            Guid.NewGuid(),
            eventName,
            Encoding.UTF8.GetBytes("{\"Value\":42}"),
            DateTime.UtcNow);

        await localDistributedEventBus.PublishFromOutboxAsync(outgoingEvent, new OutboxConfig("Test") { DatabaseName = "Test" });

        receivedData.ShouldNotBeNull();
        receivedData.EventName.ShouldBe(eventName);
        AnonymousEventDataConverter.GetJsonData(receivedData).ShouldBe("{\"Value\":42}");
        AnonymousEventDataConverter.ConvertToTypedObject<MySimpleEventData>(receivedData).Value.ShouldBe(42);
    }

    [Fact]
    public async Task Should_Rehydrate_Anonymous_Event_From_Inbox_Using_Raw_Json()
    {
        var localDistributedEventBus = GetRequiredService<LocalDistributedEventBus>();
        AnonymousEventData receivedData = null!;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = localDistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async data =>
            {
                receivedData = data;
                await Task.CompletedTask;
            })));

        var incomingEvent = new IncomingEventInfo(
            Guid.NewGuid(),
            Guid.NewGuid().ToString("N"),
            eventName,
            Encoding.UTF8.GetBytes("\"hello\""),
            DateTime.UtcNow);

        await localDistributedEventBus.ProcessFromInboxAsync(incomingEvent, new InboxConfig("Test") { DatabaseName = "Test" });

        receivedData.ShouldNotBeNull();
        receivedData.EventName.ShouldBe(eventName);
        AnonymousEventDataConverter.GetJsonData(receivedData).ShouldBe("\"hello\"");
        AnonymousEventDataConverter.ConvertToTypedObject<string>(receivedData).ShouldBe("hello");
    }

    [Fact]
    public async Task Should_Change_TenantId_If_EventData_Is_MultiTenant()
    {
        var tenantId = Guid.NewGuid();

        using var subscription = DistributedEventBus.Subscribe<MySimpleEventData>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

        await DistributedEventBus.PublishAsync(new MySimpleEventData(3, tenantId));

        Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
    }

    [Fact]
    public async Task Should_Change_TenantId_If_Generic_EventData_Is_MultiTenant()
    {
        var tenantId = Guid.NewGuid();

        using var subscription = DistributedEventBus.Subscribe<EntityCreatedEto<MySimpleEventData>>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

        await DistributedEventBus.PublishAsync(new MySimpleEventData(3, tenantId));

        Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
    }

    [Fact]
    public async Task Should_Get_TenantId_From_EventEto_Extra_Property()
    {
        var tenantId = Guid.NewGuid();

        using var subscription = DistributedEventBus.Subscribe<MySimpleEto>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

        await DistributedEventBus.PublishAsync(new MySimpleEto
        {
            Properties =
            {
                {"TenantId", tenantId.ToString()}
            }
        });

        Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
    }

    [Fact]
    public async Task DistributedEventSentAndReceived_Test()
    {
        var localEventBus = GetRequiredService<ILocalEventBus>();

        using var distributedEventSentSubscription = localEventBus.Subscribe<DistributedEventSent, DistributedEventHandles>();
        using var distributedEventReceivedSubscription = localEventBus.Subscribe<DistributedEventReceived, DistributedEventHandles>();

        using var subscription = DistributedEventBus.Subscribe<MyEventDate, MyEventHandle>();

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            MyEventDate.Order = string.Empty;
            await DistributedEventBus.PublishAsync(new MyEventDate(), onUnitOfWorkComplete: false);

            MyEventDate.Order.ShouldBe(nameof(DistributedEventSent) + nameof(DistributedEventReceived) + nameof(MyEventHandle));

            MyEventDate.Order = string.Empty;
            await DistributedEventBus.PublishAsync(new MyEventDate(), onUnitOfWorkComplete: true);
            MyEventDate.Order.ShouldBe(string.Empty);

            await uow.CompleteAsync();

           MyEventDate.Order.ShouldBe(nameof(DistributedEventSent) + nameof(DistributedEventReceived) + nameof(MyEventHandle));
        }
    }

    class MyEventDate
    {
        public static string Order { get; set; } = string.Empty;
    }

    class MyEventHandle : IDistributedEventHandler<MyEventDate>
    {
        public Task HandleEventAsync(MyEventDate eventData)
        {
            MyEventDate.Order += nameof(MyEventHandle);
            return Task.CompletedTask;
        }
    }

    class DistributedEventHandles : ILocalEventHandler<DistributedEventSent>, ILocalEventHandler<DistributedEventReceived>
    {
        public Task HandleEventAsync(DistributedEventSent eventData)
        {
            MyEventDate.Order += nameof(DistributedEventSent);
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(DistributedEventReceived eventData)
        {
            MyEventDate.Order += nameof(DistributedEventReceived);
            return Task.CompletedTask;
        }
    }

    private sealed class CountingDistributedEventHandlerFactory : IEventHandlerFactory
    {
        private readonly Action _handleAction;
        private readonly Action _disposeAction;

        public CountingDistributedEventHandlerFactory(Action handleAction, Action disposeAction)
        {
            _handleAction = handleAction;
            _disposeAction = disposeAction;
        }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            var wasHandled = false;
            return new EventHandlerDisposeWrapper(
                new CountingDistributedEventHandler(
                    _handleAction,
                    () => wasHandled = true),
                () =>
                {
                    if (wasHandled)
                    {
                        _disposeAction();
                    }
                }
            );
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories.Contains(this);
        }
    }

    private sealed class CountingDistributedEventHandler : IDistributedEventHandler<MySimpleEventData>
    {
        private readonly Action _handleAction;
        private readonly Action _markHandled;

        public CountingDistributedEventHandler(Action handleAction, Action markHandled)
        {
            _handleAction = handleAction;
            _markHandled = markHandled;
        }

        public Task HandleEventAsync(MySimpleEventData eventData)
        {
            _markHandled();
            _handleAction();
            return Task.CompletedTask;
        }
    }

}
