using System;

namespace Volo.Abp.Domain.ChangeTracking;

/// <summary>
/// Ensures that the change tracking in enabled for the given method or class.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class DisableEntityChangeTrackingAttribute : EntityChangeTrackingAttribute
{
    public DisableEntityChangeTrackingAttribute()
        : base(false)
    {
    }
}
