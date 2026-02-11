using System;
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
