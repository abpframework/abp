using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

[Serializable]
[GenericEventName(Postfix = ".BulkDeleted")]
public class BulkEntityDeletedEto<TEntityEto>
{
    public List<TEntityEto> Entities { get; set; }

    public BulkEntityDeletedEto(List<TEntityEto> entities)
    {
        Entities = entities;
    }
}