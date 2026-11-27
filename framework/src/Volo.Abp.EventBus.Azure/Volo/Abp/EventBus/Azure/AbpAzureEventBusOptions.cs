using System.Collections.Generic;

namespace Volo.Abp.EventBus.Azure;

public class AbpAzureEventBusOptions
{
    public string? ConnectionName { get; set; }

    public string SubscriberName { get; set; } = default!;

    public string TopicName { get; set; } = default!;

    public bool IsServiceBusDisabled { get; set; }

    /// <summary>
    /// Additional topics to subscribe to and publish to.
    /// When empty, only the single topic in <see cref="TopicName"/> is used (today's behaviour).
    /// </summary>
    public List<AzureEventBusTopicOptions> Topics { get; set; } = new();

    /// <summary>
    /// Maps an event name to the topic it should be published to.
    /// Events without an entry are published to <see cref="TopicName"/>.
    /// </summary>
    public Dictionary<string, string> EventTopicMap { get; set; } = new();
}

public class AzureEventBusTopicOptions
{
    public string TopicName { get; set; } = default!;

    public string SubscriberName { get; set; } = default!;

    public string? ConnectionName { get; set; }
}
