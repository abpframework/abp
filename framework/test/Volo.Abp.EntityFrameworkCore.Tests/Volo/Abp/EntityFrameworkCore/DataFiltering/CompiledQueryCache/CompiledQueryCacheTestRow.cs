using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCacheTestRow : Entity<Guid>, IMultiTenant, ISoftDelete
{
    public Guid? TenantId { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; }
}
