using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DistributedEvents;

namespace DistDemoApp
{
    [ConnectionStringName("Default")]
    public class TodoMongoDbContext : AbpMongoDbContext, IHasEventOutbox, IHasEventInbox
    {
        public IMongoCollection<TodoItem> TodoItems => Collection<TodoItem>();
        public IMongoCollection<TodoSummary> TodoSummaries => Collection<TodoSummary>();

        public IMongoCollection<OutgoingEventRecord> OutgoingEvents
        {
            get => Collection<OutgoingEventRecord>();
            set {}
        }
        public IMongoCollection<IncomingEventRecord> IncomingEvents
        {
            get => Collection<IncomingEventRecord>();
            set {}
        }
    }

}
