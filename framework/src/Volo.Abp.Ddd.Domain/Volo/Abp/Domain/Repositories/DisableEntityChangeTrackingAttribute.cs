using System;

namespace Volo.Abp.Domain.Repositories;

/// <summary>
/// Ensures that the change tracking in enabled for the given method or class.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class DisableEntityChangeTrackingAttribute : EntityChangeTrackingAttribute
{
    public DisableEntityChangeTrackingAttribute()
        : base(false)
    {
    }
}
