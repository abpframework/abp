using System;
using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.EventBus
{
    public class EventExecutionErrorContext : ExtensibleObject
    {
        public IReadOnlyList<Exception> Exceptions { get; }

        public object EventData { get; set; }

        public Type EventType { get; }

        public IEventBus EventBus { get; }

        public EventExecutionErrorContext(List<Exception> exceptions, Type eventType, IEventBus eventBus)
        {
            Exceptions = exceptions;
            EventType = eventType;
            EventBus = eventBus;
        }

        public bool TryGetRetryAttempt(out int retryAttempt)
        {
            retryAttempt = 0;
            if (!this.HasProperty(EventErrorHandlerBase.RetryAttemptKey))
            {
                return false;
            }

            retryAttempt = this.GetProperty<int>(EventErrorHandlerBase.RetryAttemptKey);
            return true;

        }
    }
}
