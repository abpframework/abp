using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Events;

[Serializable]
[Obsolete("This class isn't used anywhere.")]
public class EntityChangeEntry
{
    public object Entity { get; set; }

    public EntityChangeType ChangeType { get; set; }

    public EntityChangeEntry(object entity, EntityChangeType changeType)
    {
        Entity = entity;
        ChangeType = changeType;
    }
}
