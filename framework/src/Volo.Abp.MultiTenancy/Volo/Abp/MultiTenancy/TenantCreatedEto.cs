using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.MultiTenancy
{
    [EventName("abp.multi_tenancy.tenant.created")]
    public class TenantCreatedEto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
