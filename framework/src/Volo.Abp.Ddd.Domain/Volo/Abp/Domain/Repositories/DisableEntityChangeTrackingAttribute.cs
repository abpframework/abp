using System;

namespace Volo.Abp.Domain.Repositories;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class DisableEntityChangeTrackingAttribute : EntityChangeTrackingAttribute
{
    public DisableEntityChangeTrackingAttribute()
        : base(false)
    {
    }
}
