using System;
using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.EventBus
{
    public class EventExecutionErrorContext : ExtensibleObject
    {
        public IReadOnlyList<Exception> Exceptions { get; }

        public object EventData { get; }

        public Type EventType { get; }

        public IEventBus EventBus { get; }

        public EventExecutionErrorContext(List<Exception> exceptions, object eventData, Type eventType, IEventBus eventBus)
        {
            Exceptions = exceptions;
            EventData = eventData;
            EventType = eventType;
            EventBus = eventBus;
        }
    }
}
