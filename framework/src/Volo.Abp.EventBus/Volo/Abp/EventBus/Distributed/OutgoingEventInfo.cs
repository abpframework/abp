using System;
using Volo.Abp.Data;

namespace Volo.Abp.EventBus.Distributed
{
    public class OutgoingEventInfo : IHasExtraProperties 
    {
        public static int MaxEventNameLength { get; set; } = 256;

        public ExtraPropertyDictionary ExtraProperties { get; protected set; }

        public Guid Id { get; }
        
        public string EventName { get; }
        
        public byte[] EventData { get; }
        
        public DateTime CreationTime { get; }

        protected OutgoingEventInfo()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public OutgoingEventInfo(
            Guid id, 
            string eventName,
            byte[] eventData,
            DateTime creationTime)
        {
            Id = id;
            EventName = eventName;
            EventData = eventData;
            CreationTime = creationTime;
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
    }}