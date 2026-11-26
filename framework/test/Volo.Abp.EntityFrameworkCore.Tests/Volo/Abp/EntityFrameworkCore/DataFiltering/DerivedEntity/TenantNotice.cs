using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

// A derived entity type that adds ISoftDelete and IMultiTenant to an unfiltered root (mapped as TPH).
public class TenantNotice : Notice, ISoftDelete, IMultiTenant
{
    public bool IsDeleted { get; set; }

    public Guid? TenantId { get; set; }
}
