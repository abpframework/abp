using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Distributed;

public class LocalDistributedEventBus_Test : LocalDistributedEventBusTestBase
{
    [Fact]
    public async Task Should_Call_Handler_AndDispose()
    {
        DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        await DistributedEventBus.PublishAsync(new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(2));
        await DistributedEventBus.PublishAsync(new MySimpleEventData(3));

        Assert.Equal(3, MySimpleDistributedTransientEventHandler.HandleCount);
        Assert.Equal(3, MySimpleDistributedTransientEventHandler.DisposeCount);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_EventName()
    {
        DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

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
    public async Task Should_Handle_Anonymous_Handler_When_Published_With_EventName()
    {
        var handleCount = 0;

        DistributedEventBus.Subscribe("MyEvent",
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                handleCount++;
                await Task.CompletedTask;
            })));

        await DistributedEventBus.PublishAsync("MyEvent", new MySimpleEventData(1));
        await DistributedEventBus.PublishAsync("MyEvent", new Dictionary<string, object>()
        {
            {"Value", 2}
        });
        await DistributedEventBus.PublishAsync("MyEvent", new { Value = 3 });
        await DistributedEventBus.PublishAsync("MyEvent", new[] { 1, 2, 3 });

        Assert.Equal(4, handleCount);
    }

    [Fact]
    public async Task Should_Handle_Anonymous_Handler_When_Published_With_AnonymousEventData()
    {
        var handleCount = 0;

        DistributedEventBus.Subscribe("MyEvent",
            new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
            {
                handleCount++;
                d.ConvertToTypedObject().ShouldNotBeNull();
                await Task.CompletedTask;
            })));

        await DistributedEventBus.PublishAsync(new AnonymousEventData("MyEvent", new MySimpleEventData(1)));
        await DistributedEventBus.PublishAsync(new AnonymousEventData("MyEvent", new Dictionary<string, object>()
        {
            {"Value", 2}
        }));
        await DistributedEventBus.PublishAsync(new AnonymousEventData("MyEvent", new { Value = 3 }));

        Assert.Equal(3, handleCount);
    }

    [Fact]
    public async Task Should_Handle_Typed_Handler_When_Published_With_AnonymousEventData()
    {
        DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

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
        DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        var anonymousHandleCount = 0;

        DistributedEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
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
        DistributedEventBus.Subscribe<MySimpleEventData, MySimpleDistributedTransientEventHandler>();

        var eventName = EventNameAttribute.GetNameOrDefault<MySimpleEventData>();

        var anonymousHandleCount = 0;

        DistributedEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new ActionEventHandler<AnonymousEventData>(async (d) =>
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

        var handler = new ActionEventHandler<AnonymousEventData>(async (d) =>
        {
            handleCount++;
            await Task.CompletedTask;
        });
        var factory = new SingleInstanceHandlerFactory(handler);

        var disposable = DistributedEventBus.Subscribe("MyEvent", factory);

        await DistributedEventBus.PublishAsync("MyEvent", new { Value = 1 });
        Assert.Equal(1, handleCount);

        disposable.Dispose();

        await Assert.ThrowsAsync<AbpException>(() =>
            DistributedEventBus.PublishAsync("MyEvent", new { Value = 2 }));
        Assert.Equal(1, handleCount);
    }

    [Fact]
    public async Task Should_Throw_For_Unknown_Event_Name()
    {
        await Assert.ThrowsAsync<AbpException>(() =>
            DistributedEventBus.PublishAsync("NonExistentEvent", new { Value = 1 }));
    }

    [Fact]
    public async Task Should_Convert_AnonymousEventData_To_Typed_Object()
    {
        MySimpleEventData? receivedData = null;

        DistributedEventBus.Subscribe<MySimpleEventData>(async (data) =>
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
    public async Task Should_Change_TenantId_If_EventData_Is_MultiTenant()
    {
        var tenantId = Guid.NewGuid();

        DistributedEventBus.Subscribe<MySimpleEventData>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

        await DistributedEventBus.PublishAsync(new MySimpleEventData(3, tenantId));

        Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
    }

    [Fact]
    public async Task Should_Change_TenantId_If_Generic_EventData_Is_MultiTenant()
    {
        var tenantId = Guid.NewGuid();

        DistributedEventBus.Subscribe<EntityCreatedEto<MySimpleEventData>>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

        await DistributedEventBus.PublishAsync(new MySimpleEventData(3, tenantId));

        Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
    }

    [Fact]
    public async Task Should_Get_TenantId_From_EventEto_Extra_Property()
    {
        var tenantId = Guid.NewGuid();

        DistributedEventBus.Subscribe<MySimpleEto>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

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

        localEventBus.Subscribe<DistributedEventSent, DistributedEventHandles>();
        localEventBus.Subscribe<DistributedEventReceived, DistributedEventHandles>();

        DistributedEventBus.Subscribe<MyEventDate, MyEventHandle>();

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

}
