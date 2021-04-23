using System;

namespace Volo.Abp.EventBus.Local
{
    public class LocalEventMessage
    {
        public Guid MessageId { get; }

        public object EventData { get; }

        public Type EventType { get; }

        public LocalEventMessage(Guid messageId, object eventData, Type eventType)
        {
            MessageId = messageId;
            EventData = eventData;
            EventType = eventType;
        }
    }
}
