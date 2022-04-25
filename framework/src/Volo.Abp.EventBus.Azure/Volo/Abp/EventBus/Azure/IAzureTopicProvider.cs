using System;

namespace Volo.Abp.EventBus;

public interface IAzureTopicProvider
{
    string GetTopicName(Type eventType);
}
