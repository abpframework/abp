using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Distributed;

public class LocalDistributedEventBus_Outbox_MultiTenancy_Test : AbpIntegratedTest<EventBusOutboxTestModule>
{
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly ICurrentTenant _currentTenant;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly FakeEventOutbox _outbox;

    public LocalDistributedEventBus_Outbox_MultiTenancy_Test()
    {
        _distributedEventBus = GetRequiredService<LocalDistributedEventBus>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _outbox = GetRequiredService<FakeEventOutbox>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public async Task Should_Write_The_Publish_Tenant_To_The_Outbox_Record()
    {
        var tenantId = Guid.NewGuid();
        var eventName = "TestEvent-" + Guid.NewGuid().ToString("N");

        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            using (_currentTenant.Change(tenantId))
            {
                await _distributedEventBus.PublishAsync(eventName, new { Value = 1 });
            }

            _currentTenant.Id.ShouldBeNull();

            await uow.CompleteAsync();
        }

        var outgoingEvent = _outbox.Events.ShouldHaveSingleItem();
        outgoingEvent.EventName.ShouldBe(eventName);
        outgoingEvent.GetTenantId().ShouldBe(tenantId);
    }

    [Fact]
    public async Task Should_Not_Write_A_Tenant_To_The_Outbox_Record_Of_A_Typed_Event()
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            using (_currentTenant.Change(Guid.NewGuid()))
            {
                await _distributedEventBus.PublishAsync(new MySimpleEventData(1));
            }

            await uow.CompleteAsync();
        }

        var outgoingEvent = _outbox.Events.ShouldHaveSingleItem();
        outgoingEvent.GetTenantId().ShouldBeNull();
    }
}
