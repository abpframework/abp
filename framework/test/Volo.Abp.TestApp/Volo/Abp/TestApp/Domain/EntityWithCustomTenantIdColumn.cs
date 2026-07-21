using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TestApp.Domain;

public class EntityWithCustomTenantIdColumn : AggregateRoot<Guid>, IMultiTenant
{
    public const string TenantIdColumnName = "custom_tenant_id_column";

    public string Name { get; set; }

    public Guid? TenantId { get; set; }
}
