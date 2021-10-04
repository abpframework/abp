using MongoDB.Driver;

namespace Volo.Abp.MongoDB.DistributedEvents
{
    public interface IHasEventInbox : IAbpMongoDbContext
    {
        IMongoCollection<IncomingEventRecord> IncomingEvents { get; }
    }
}
