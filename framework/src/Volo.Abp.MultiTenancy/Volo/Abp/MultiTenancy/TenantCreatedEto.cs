using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.Abp.MultiTenancy
{
    [Serializable]
    [EventName("abp.multi_tenancy.tenant.created")]
    public class TenantCreatedEto : EtoBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
