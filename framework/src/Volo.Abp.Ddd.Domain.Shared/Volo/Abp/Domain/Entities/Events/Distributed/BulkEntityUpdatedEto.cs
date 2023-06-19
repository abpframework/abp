using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

[Serializable]
[GenericEventName(Postfix = ".BulkUpdated")]
public class BulkEntityUpdatedEto<TEntityEto>
{
    public List<TEntityEto> Entities { get; set; }

    public BulkEntityUpdatedEto(List<TEntityEto> entities)
    {
        Entities = entities;
    }
}