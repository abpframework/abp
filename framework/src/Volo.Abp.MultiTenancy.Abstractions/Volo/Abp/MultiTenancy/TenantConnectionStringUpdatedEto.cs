using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.Abp.MultiTenancy;

[Serializable]
[EventName("abp.multi_tenancy.tenant.connection_string.updated")]
public class TenantConnectionStringUpdatedEto : EtoBase
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string ConnectionStringName { get; set; }

    public string OldValue { get; set; }

    public string NewValue { get; set; }
}
