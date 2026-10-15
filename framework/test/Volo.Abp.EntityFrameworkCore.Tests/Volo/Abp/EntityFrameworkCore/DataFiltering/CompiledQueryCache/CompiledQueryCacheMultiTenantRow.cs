using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCacheMultiTenantRow : Entity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
}
