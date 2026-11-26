using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

// A derived entity type that adds IMultiTenant to a soft-delete filtered root (mapped as TPH).
public class TenantAnnouncement : Announcement, IMultiTenant
{
    public Guid? TenantId { get; set; }
}
