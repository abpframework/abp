using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.Abp.MultiTenancy;

[Serializable]
[EventName("abp.multi_tenancy.tenant")]
public class TenantEto : IEntityEto<Guid>, IHasEntityVersion
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public int EntityVersion { get; set; }
}
