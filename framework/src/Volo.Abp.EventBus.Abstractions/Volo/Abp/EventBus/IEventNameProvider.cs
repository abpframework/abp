using System;

namespace Volo.Abp.EventBus;

public interface IEventNameProvider
{
    string GetName(Type eventType);
}
