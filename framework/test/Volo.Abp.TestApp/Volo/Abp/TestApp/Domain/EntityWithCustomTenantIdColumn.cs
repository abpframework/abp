using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.Domain;

public class EntityWithCustomTenantIdColumn : AggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; set; }

    public Guid? TenantId { get; set; }
}
