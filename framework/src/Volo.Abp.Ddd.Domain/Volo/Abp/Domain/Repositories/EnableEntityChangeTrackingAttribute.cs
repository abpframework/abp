using System;

namespace Volo.Abp.Domain.Repositories;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class EnableEntityChangeTrackingAttribute : EntityChangeTrackingAttribute
{
    public EnableEntityChangeTrackingAttribute()
        : base(true)
    {
    }
}
