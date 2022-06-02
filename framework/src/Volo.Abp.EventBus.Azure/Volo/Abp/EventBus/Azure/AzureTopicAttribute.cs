using System;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.EventBus.Azure;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus;

[AttributeUsage(AttributeTargets.Class)]
public class AzureTopicAttribute : Attribute, IAzureTopicProvider
{
    public virtual string TopicName { get; }

    public AzureTopicAttribute([NotNull] string topicName)
    {
        TopicName = Check.NotNullOrWhiteSpace(topicName, nameof(topicName));
    }

    public static string GetNameOrDefault<TEvent>(AbpAzureEventBusOptions options)
    {
        return GetTopicNameOrDefault(typeof(TEvent), options);
    }

    public static string GetTopicNameOrDefault([NotNull] Type eventType, AbpAzureEventBusOptions options)
    {
        Check.NotNull(eventType, nameof(eventType));

        if(!options.TopicPerEvent) 
            return options.TopicName;

        return eventType
                   .GetCustomAttributes(true)
                   .OfType<IAzureTopicProvider>()
                   .FirstOrDefault()
                   ?.GetTopicName(eventType)
               ?? options.TopicName;
    }

    public string GetTopicName(Type eventType)
    {
        return TopicName;
    }
}
