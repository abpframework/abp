using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Data;

public class DataSeedContext
{
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Gets/sets a key-value on the <see cref="Properties"/>.
    /// </summary>
    /// <param name="name">Name of the property</param>
    /// <returns>
    /// Returns the value in the <see cref="Properties"/> dictionary by given <paramref name="name"/>.
    /// Returns null if given <paramref name="name"/> is not present in the <see cref="Properties"/> dictionary.
    /// </returns>
    [CanBeNull]
    public object this[string name]
    {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    /// <summary>
    /// Can be used to get/set custom properties.
    /// </summary>
    [NotNull]
    public Dictionary<string, object> Properties { get; }

    public DataSeedContext(Guid? tenantId = null)
    {
        TenantId = tenantId;
        Properties = new Dictionary<string, object>();
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual DataSeedContext WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }
}
