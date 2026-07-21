using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
        using var subscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(2));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(3));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
        Assert.Equal(3, MySimpleDistributedTransientEventHandler.DisposeCount);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_EventName()
    {
        using var subscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();
        await DistributedEventBus.PublishAsync(eventName, new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        });
        await DistributedEventBus.PublishAsync(eventName, new { Value = 3 });

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
        Assert.Equal(3, MySimpleDistributedTransientEventHandler.DisposeCount);
    }

    [Fact]
    public async Task Should_Handle_Dynamic_Handler_When_Published_With_EventName()
    {
        var handleCount = 0;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = DistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
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
    public async Task Should_Handle_Dynamic_Handler_When_Published_With_DynamicEventData()
    {
        var handleCount = 0;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = DistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                handleCount++;
                d.Data.ShouldNotBeNull();
                await Task.CompletedTask;
            })));

        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new MySimpleEventData(1)));
        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        }));
        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new { Value = 3 }));

        Assert.Equal(3, handleCount);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_DynamicEventData()
    {
        using var subscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new MySimpleEventData(1)));
        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        }));
        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new { Value = 3 }));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
    }

    [Fact]
    public async Task Should_Trigger_Both_Typed_And_Dynamic_Handlers_For_Typed_Event()
    {
        using var typedSubscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        var dynamicHandleCount = 0;

        using var dynamicSubscription = DistributedEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
        {
            dynamicHandleCount++;
            await Task.CompletedTask;
        })));

        await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(2));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(3));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
        Assert.Equal(3, dynamicHandleCount);
    }

    [Fact]
    public async Task Should_Trigger_Both_Handlers_For_Mixed_Typed_And_Dynamic_Publish()
    {
        using var typedSubscription = DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        var dynamicHandleCount = 0;

        using var dynamicSubscription = DistributedEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
        {
            dynamicHandleCount++;
            await Task.CompletedTask;
        })));

        await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new Dictionary<string, object>()
        {
            {"Value", 2}
        }));
        await DistributedEventBus.PublishAsync(new DynamicEventData(eventName, new { Value = 3 }));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
        Assert.Equal(3, dynamicHandleCount);
    }

    [Fact]
    public async Task Should_Unsubscribe_Dynamic_Handler()
    {
        var handleCount = 0;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        var handler = new ActionEventHandler<DynamicEventData>(async (d) =>
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
    public async Task Should_Not_Throw_For_Unknown_Event_Name()
    {
        // Publishing to an unknown event name should not throw (consistent with typed PublishAsync behavior)
        await DistributedEventBus.PublishAsync("NonExistentEvent", new { Value = 1 });
    }

    [Fact]
    public async Task Should_Convert_DynamicEventData_To_Typed_Object()
    {
        MySimpleEventData? receivedData = null;

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
    public async Task Should_Subscribe_With_IDistributedEventHandler()
    {
        var handleCount = 0;
        var eventName = "MyEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = DistributedEventBus.Subscribe(eventName,
            new TestDynamicDistributedEventHandler(() => handleCount++));

        await DistributedEventBus.PublishAsync(eventName, new { Value = 1 });
        await DistributedEventBus.PublishAsync(eventName, new { Value = 2 });

        Assert.Equal(2, handleCount);
    }

    [Fact]
    public async Task Should_Handle_Multiple_Dynamic_Events_Independently()
    {
        var countA = 0;
        var countB = 0;
        var eventNameA = "EventA-" + Guid.NewGuid().ToString("N");
        var eventNameB = "EventB-" + Guid.NewGuid().ToString("N");

        using var subA = DistributedEventBus.Subscribe(eventNameA,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                countA++;
                await Task.CompletedTask;
            })));

        using var subB = DistributedEventBus.Subscribe(eventNameB,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                countB++;
                await Task.CompletedTask;
            })));

        await DistributedEventBus.PublishAsync(eventNameA, new { Value = 1 });
        await DistributedEventBus.PublishAsync(eventNameB, new { Value = 2 });
        await DistributedEventBus.PublishAsync(eventNameA, new { Value = 3 });

        Assert.Equal(2, countA);
        Assert.Equal(1, countB);
    }

    [Fact]
    public async Task Should_Receive_EventName_In_DynamicEventData()
    {
        string? receivedEventName = null;
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");

        using var subscription = DistributedEventBus.Subscribe(eventName,
            new SingleInstanceHandlerFactory(new ActionEventHandler<DynamicEventData>(async (d) =>
            {
                receivedEventName = d.EventName;
                await Task.CompletedTask;
            })));

        await DistributedEventBus.PublishAsync(eventName, new { Value = 1 });

        receivedEventName.ShouldBe(eventName);
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

    [Fact]
    public async Task IocEventHandlerFactory_Should_Create_New_Scope_And_Dispose_Handler_Per_Event()
    {
        var handleCount = 0;
        var disposeCount = 0;
        var eventName = "IocEvent-" + Guid.NewGuid().ToString("N");

        var services = new ServiceCollection();
        services.AddSingleton<ITestCounterService>(
            new TestCounterService(() => handleCount++, () => disposeCount++));
        services.AddTransient<DynamicIocEventHandlerWithCounter>();
        using var provider = services.BuildServiceProvider();
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        using var subscription = DistributedEventBus.Subscribe(
            eventName,
            new IocEventHandlerFactory(scopeFactory, typeof(DynamicIocEventHandlerWithCounter)));

        await DistributedEventBus.PublishAsync(eventName, new { Value = 1 });
        await DistributedEventBus.PublishAsync(eventName, new { Value = 2 });
        await DistributedEventBus.PublishAsync(eventName, new { Value = 3 });

        // Handler is invoked exactly once per event.
        Assert.Equal(3, handleCount);
        // Handler is disposed at least once per event (the scope is always cleaned up).
        Assert.True(disposeCount >= handleCount);
    }

    [Fact]
    public async Task IocEventHandlerFactory_Should_Resolve_DI_Services_In_Handler_Constructor()
    {
        var callCount = 0;
        var eventName = "IocEvent-" + Guid.NewGuid().ToString("N");

        var services = new ServiceCollection();
        services.AddSingleton<ITestCounterService>(new TestCounterService(() => callCount++));
        services.AddTransient<DynamicIocEventHandlerWithService>();
        using var provider = services.BuildServiceProvider();
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        using var subscription = DistributedEventBus.Subscribe(
            eventName,
            new IocEventHandlerFactory(scopeFactory, typeof(DynamicIocEventHandlerWithService)));

        await DistributedEventBus.PublishAsync(eventName, new { Value = 1 });
        await DistributedEventBus.PublishAsync(eventName, new { Value = 2 });

        // The handler resolved ITestCounterService via constructor injection
        Assert.Equal(2, callCount);
    }

    class TestDynamicDistributedEventHandler : IDistributedEventHandler<DynamicEventData>
    {
        private readonly Action _onHandle;

        public TestDynamicDistributedEventHandler(Action onHandle)
        {
            _onHandle = onHandle;
        }

        public Task HandleEventAsync(DynamicEventData eventData)
        {
            _onHandle();
            return Task.CompletedTask;
        }
    }

    interface ITestCounterService
    {
        void OnHandle();
        void OnDispose();
    }

    class TestCounterService : ITestCounterService
    {
        private readonly Action _onHandle;
        private readonly Action? _onDispose;

        public TestCounterService(Action onHandle, Action? onDispose = null)
        {
            _onHandle = onHandle;
            _onDispose = onDispose;
        }

        public void OnHandle() => _onHandle();
        public void OnDispose() => _onDispose?.Invoke();
    }

    class DynamicIocEventHandlerWithCounter : IDistributedEventHandler<DynamicEventData>, IDisposable
    {
        private readonly ITestCounterService _counterService;

        public DynamicIocEventHandlerWithCounter(ITestCounterService counterService)
        {
            _counterService = counterService;
        }

        public Task HandleEventAsync(DynamicEventData eventData)
        {
            _counterService.OnHandle();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _counterService.OnDispose();
        }
    }

    class DynamicIocEventHandlerWithService : IDistributedEventHandler<DynamicEventData>
    {
        private readonly ITestCounterService _counterService;

        public DynamicIocEventHandlerWithService(ITestCounterService counterService)
        {
            _counterService = counterService;
        }

        public Task HandleEventAsync(DynamicEventData eventData)
        {
            _counterService.OnHandle();
            return Task.CompletedTask;
        }
    }
}
