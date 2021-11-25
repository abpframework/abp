using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_MultiTenancy_Test : EventBusTestBase
{
    [Fact]
    public async Task Should_Change_TenantId_If_EventData_Is_MultiTenant()
    {
        var tenantId = Guid.NewGuid();
        var handler = new MyEventHandler(GetRequiredService<ICurrentTenant>());

        LocalEventBus.Subscribe<EntityChangedEventData<MyEntity>>(handler);

        await LocalEventBus.PublishAsync(new EntityCreatedEventData<MyEntity>(new MyEntity(tenantId)));

        handler.TenantId.ShouldBe(tenantId);
    }

    public class MyEntity : Entity, IMultiTenant
    {
        public override object[] GetKeys()
        {
            return new object[0];
        }

        public MyEntity()
        {

        }

        public MyEntity(Guid? tenantId)
        {
            TenantId = tenantId;
        }

        public Guid? TenantId { get; }
    }

    public class MyEventHandler : ILocalEventHandler<EntityChangedEventData<MyEntity>>
    {
        private readonly ICurrentTenant _currentTenant;

        public MyEventHandler(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public Guid? TenantId { get; set; }

        public Task HandleEventAsync(EntityChangedEventData<MyEntity> eventData)
        {
            TenantId = _currentTenant.Id;
            return Task.CompletedTask;
        }
    }
}
