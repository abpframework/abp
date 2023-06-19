using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

[Serializable]
[GenericEventName(Postfix = ".BulkCreated")]
public class BulkEntityCreatedEto<TEntityEto>
{
    public List<TEntityEto> Entities { get; set; }

    public BulkEntityCreatedEto(List<TEntityEto> entities)
    {
        Entities = entities;
    }
}