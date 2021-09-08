using System;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class OutgoingEventRecord : BasicAggregateRoot<Guid>, IHasExtraProperties, IHasCreationTime
    {
        public static int MaxEventNameLength { get; set; } = 256;
        
        public ExtraPropertyDictionary ExtraProperties { get; protected set; }

        public string EventName { get; set; }
        
        public byte[] EventData { get; set; }
        
        public DateTime CreationTime { get; set; }

        protected OutgoingEventRecord()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public OutgoingEventRecord(Guid id, string eventName, byte[] eventData)
            : base(id)
        {
            EventName = eventName;
            EventData = eventData;
            
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
    }
}