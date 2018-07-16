using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Auditing
{
    public class EntityChangeInfo : IMultiTenant, IHasExtraProperties
    {
        public DateTime ChangeTime { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public string EntityId { get; set; }

        public string EntityTypeFullName { get; set; }

        public Guid? TenantId { get; set; }

        public List<EntityPropertyChangeInfo> PropertyChanges { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public virtual object EntityEntry { get; set; } //TODO: Try to remove since it breaks serializability

        public EntityChangeInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
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
}
