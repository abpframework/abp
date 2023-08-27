using System;

namespace Volo.Abp.Domain.Repositories;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class EntityChangeTrackingAttribute : Attribute
{
    public bool Enabled { get; set; }

    public EntityChangeTrackingAttribute(bool enabled)
    {
        Enabled = enabled;
    }
}
