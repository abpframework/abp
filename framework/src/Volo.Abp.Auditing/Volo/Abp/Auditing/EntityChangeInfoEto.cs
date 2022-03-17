using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.Auditing;

[Serializable]
public class EntityChangeInfoEto : IHasExtraProperties
{
    public DateTime ChangeTime { get; set; }

    public EntityChangeType ChangeType { get; set; }
    public Guid? EntityTenantId { get; set; }

    public string EntityId { get; set; }

    public string EntityTypeFullName { get; set; }

    public List<EntityPropertyChangeInfoEto> PropertyChanges { get; set; }
    public ExtraPropertyDictionary ExtraProperties { get; set; }
}