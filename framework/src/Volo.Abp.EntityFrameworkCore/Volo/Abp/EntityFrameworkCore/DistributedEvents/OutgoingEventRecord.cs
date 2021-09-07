using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class OutgoingEventRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
    {
        public static int MaxEventNameLength { get; set; } = 256;
        
        public ExtraPropertyDictionary ExtraProperties { get; protected set; }

        public string EventName { get; set; }
        public byte[] EventData { get; set; }

        protected OutgoingEventRecord()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }

        public OutgoingEventRecord(Guid id)
            : base(id)
        {
            ExtraProperties = new ExtraPropertyDictionary();
            this.SetDefaultsForExtraProperties();
        }
    }
}