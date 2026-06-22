using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Volo.Abp.MultiTenancy;

[Serializable]
[EventName("abp.multi_tenancy.create.tenant")]
public class CreateTenantEto : EtoBase
{
    public string Name { get; set; } = default!;

    public string AdminEmailAddress { get; set; } = default!;
}
