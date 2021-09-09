using System;
using Volo.Abp.Data;

namespace Volo.Abp.EventBus.Distributed
{
    public class IncomingEventInfo : IHasExtraProperties
    {
        public static int MaxEventNameLength { get; set; } = 256;

        public ExtraPropertyDictionary ExtraProperties { get; protected set; }

        public Guid Id { get; }
        
        public string EventName { get; }
        
        public byte[] EventData { get; }
        
        public DateTime CreationTime { get; }
        
        protected IncomingEventInfo()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public IncomingEventInfo(
            Guid id, 
            string eventName,
            byte[] eventData,
            DateTime creationTime)
        {
            Id = id;
            EventName = Check.NotNullOrWhiteSpace(eventName, nameof(eventName), MaxEventNameLength);
            EventData = eventData;
            CreationTime = creationTime;
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
    }
}