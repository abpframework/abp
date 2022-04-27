using System;
using System.Linq;

namespace Volo.Abp.EventBus.Distributed;

public static class AbpDistributedEventBusOptionsExtensions
{
    public static OutboxConfig GetOutbox(this AbpDistributedEventBusOptions options, Type eventType)
    {
        return options.Outboxes.Values.FirstOrDefault(outboxConfig =>
            outboxConfig.Selector == null || outboxConfig.Selector(eventType));
    }

    public static bool OutboxExist(this AbpDistributedEventBusOptions options, Type eventType)
    {
        return options.GetOutbox(eventType) is not null;
    }
}