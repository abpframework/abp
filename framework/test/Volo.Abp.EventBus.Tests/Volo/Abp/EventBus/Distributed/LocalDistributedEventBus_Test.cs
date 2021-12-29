using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MultiTenancy;
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
    public async Task Should_Publish_Event_On_Uow_Completed_If_OnUnitOfWorkComplete_And_UseOutBox()
    {
        var tenantId = Guid.NewGuid();

        DistributedEventBus.Subscribe<MySimpleEto>(GetRequiredService<MySimpleDistributedSingleInstanceEventHandler>());

        var eto = new MySimpleEto { Properties = { { "TenantId", tenantId.ToString() } } };
        
        using (var uow = UnitOfWorkManager.Begin(new AbpUnitOfWorkOptions()))
        {
            uow.OnCompleted(() =>
            {
                Assert.Null(MySimpleDistributedSingleInstanceEventHandler.TenantId);
                return Task.CompletedTask;
            });
            
            await DistributedEventBus.PublishAsync(
                eventData: eto,
                onUnitOfWorkComplete: true,
                useOutbox: true);
            
            Assert.Null(MySimpleDistributedSingleInstanceEventHandler.TenantId);

            await uow.CompleteAsync();
            
            Assert.Equal(tenantId, MySimpleDistributedSingleInstanceEventHandler.TenantId);
        }

    }
}
