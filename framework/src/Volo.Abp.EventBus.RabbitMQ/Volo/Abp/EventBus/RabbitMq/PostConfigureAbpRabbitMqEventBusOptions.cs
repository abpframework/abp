using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Volo.Abp.EventBus.RabbitMq;

public class PostConfigureAbpRabbitMqEventBusOptions : IPostConfigureOptions<AbpRabbitMqEventBusOptions>
{
    private readonly HashSet<string> _uint64QueueArguments =
    [
        "x-delivery-limit",
        "x-expires",
        "x-message-ttl",
        "x-max-length",
        "x-max-length-bytes",
        "x-quorum-initial-group-size",
        "x-quorum-target-group-size",
        "x-stream-filter-size-bytes",
        "x-stream-max-segment-size-bytes",
    ];

    private readonly HashSet<string> _boolQueueArguments =
    [
        "x-single-active-consumer"
    ];

    public virtual void PostConfigure(string? name, AbpRabbitMqEventBusOptions options)
    {
        ParseBoolQueueArguments(options);
        ParseIntegerQueueArguments(options);
    }

    protected virtual void ParseBoolQueueArguments(AbpRabbitMqEventBusOptions options)
    {
        foreach (var argument in _boolQueueArguments)
        {
            if (options.QueueArguments.TryGetValue(argument, out var value) && value is string stringValue && bool.TryParse(stringValue, out var boolValue))
            {
                options.QueueArguments[argument] = boolValue;
            }
        }
    }

    protected virtual void ParseIntegerQueueArguments(AbpRabbitMqEventBusOptions options)
    {
        foreach (var argument in _uint64QueueArguments)
        {
            if (options.QueueArguments.TryGetValue(argument, out var value) && value is string stringValue && int.TryParse(stringValue, out var intValue))
            {
                options.QueueArguments[argument] = intValue;
            }
        }
    }

}
