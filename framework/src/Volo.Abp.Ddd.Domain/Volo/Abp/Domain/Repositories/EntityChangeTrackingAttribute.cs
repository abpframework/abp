using System;

namespace Volo.Abp.Domain.Repositories;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public abstract class EntityChangeTrackingAttribute : Attribute
{
    public virtual bool IsEnabled { get; set; }

    public EntityChangeTrackingAttribute(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
}
