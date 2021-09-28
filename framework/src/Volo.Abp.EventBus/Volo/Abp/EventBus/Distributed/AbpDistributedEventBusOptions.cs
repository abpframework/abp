using System;
using Volo.Abp.Collections;

namespace Volo.Abp.EventBus.Distributed
{
    public class AbpDistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public OutboxConfigDictionary Outboxes { get; }

        public InboxConfigDictionary Inboxes { get; }

        /// <summary>
        /// Default: -2 hours
        /// </summary>
        public TimeSpan InboxKeepEventTimeSpan { get; set; }

        public AbpDistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
            Outboxes = new OutboxConfigDictionary();
            Inboxes = new InboxConfigDictionary();
            InboxKeepEventTimeSpan = TimeSpan.FromHours(-2);
        }
    }
}
