using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EventBus.Distributed
{
    public class MySimpleDistributedSingleInstanceEventHandler : IDistributedEventHandler<MySimpleEventData>, IDistributedEventHandler<EntityCreatedEto<MySimpleEventData>>, IDistributedEventHandler<MySimpleEto>, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;

        public MySimpleDistributedSingleInstanceEventHandler(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public static Guid? TenantId { get; set; }

        public Task HandleEventAsync(MySimpleEventData eventData)
        {
            TenantId = _currentTenant.Id;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityCreatedEto<MySimpleEventData> eventData)
        {
            TenantId = _currentTenant.Id;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(MySimpleEto eventData)
        {
            var tenantIdString = eventData.Properties.GetOrDefault("TenantId").ToString();
            TenantId = tenantIdString != null ? new Guid(tenantIdString) : _currentTenant.Id;
            return Task.CompletedTask;
        }
    }
}
