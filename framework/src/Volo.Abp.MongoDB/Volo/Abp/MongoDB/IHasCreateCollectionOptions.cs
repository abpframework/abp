using MongoDB.Bson;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB;

public interface IHasCreateCollectionOptions
{
    CreateCollectionOptions<BsonDocument> CreateCollectionOptions { get; }
}