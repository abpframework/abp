using System;
using System.Collections.Generic;
using Volo.Abp.EventBus.Local;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.EventBus
{
    public class EventExecutionErrorContext : ExtensibleObject
    {
        public IReadOnlyList<Exception> Exceptions { get; }

        public object EventData { get; }

        public Type EventType { get; }

        public EventExecutionErrorContext(List<Exception> exceptions, object eventData, Type eventType)
        {
            Exceptions = exceptions;
            EventData = eventData;
            EventType = eventType;
        }
    }
}
