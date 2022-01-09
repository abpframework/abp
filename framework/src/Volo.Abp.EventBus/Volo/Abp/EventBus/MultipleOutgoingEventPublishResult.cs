using System.Collections.Generic;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus;

public class MultipleOutgoingEventPublishResult
{
    public IReadOnlyList<OutgoingEventInfo> PublishedOutgoingEvents { get; }

    public MultipleOutgoingEventPublishResult(IReadOnlyList<OutgoingEventInfo> outgoingEvents)
    {
        PublishedOutgoingEvents = outgoingEvents;
    }
}
