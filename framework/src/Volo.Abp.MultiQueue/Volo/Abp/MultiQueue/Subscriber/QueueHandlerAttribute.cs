using System;

namespace Volo.Abp.MultiQueue.Subscriber;

public class QueueHandlerAttribute : Attribute
{
    public string Key { get; }

    public string Topic { get; }

    public Type EventType { get; }

    public QueueHandlerAttribute(string key, string topic, Type eventType)
    {
        Key = key;
        Topic = topic;
        EventType = eventType;
    }
}