using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Data;

namespace Volo.Abp.Auditing;

[Serializable]
public class EntityChangeInfo : IHasExtraProperties
{
    public DateTime ChangeTime { get; set; }

    public EntityChangeType ChangeType { get; set; }

    /// <summary>
    /// TenantId of the related entity.
    /// This is not the TenantId of the audit log entry.
    /// There can be multiple tenant data changes in a single audit log entry.
    /// </summary>
    public Guid? EntityTenantId { get; set; }

    public string EntityId { get; set; }

    public string EntityTypeFullName { get; set; }

    public List<EntityPropertyChangeInfo> PropertyChanges { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public virtual object EntityEntry { get; set; } //TODO: Try to remove since it breaks serializability

    public EntityChangeInfo()
    {
        ExtraProperties = new ExtraPropertyDictionary();
    }

    public virtual void Merge(EntityChangeInfo changeInfo)
    {
        foreach (var propertyChange in changeInfo.PropertyChanges)
        {
            var existingChange = PropertyChanges.FirstOrDefault(p => p.PropertyName == propertyChange.PropertyName);
            if (existingChange == null)
            {
                PropertyChanges.Add(propertyChange);
            }
            else
            {
                existingChange.NewValue = propertyChange.NewValue;
            }
        }

        foreach (var extraProperty in changeInfo.ExtraProperties)
        {
            var key = extraProperty.Key;
            if (ExtraProperties.ContainsKey(key))
            {
                key = InternalUtils.AddCounter(key);
            }

            ExtraProperties[key] = extraProperty.Value;
        }
    }
}
