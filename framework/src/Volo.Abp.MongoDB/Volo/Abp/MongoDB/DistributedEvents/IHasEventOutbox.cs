using MongoDB.Driver;

namespace Volo.Abp.MongoDB.DistributedEvents;

public interface IHasEventOutbox : IAbpMongoDbContext
{
    IMongoCollection<OutgoingEventRecord> OutgoingEvents { get; }
}
