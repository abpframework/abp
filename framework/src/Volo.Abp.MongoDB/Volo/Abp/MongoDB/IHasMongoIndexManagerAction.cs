using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB;

public interface IHasMongoIndexManagerAction
{
    Action<IMongoIndexManager<BsonDocument>>? IndexesAction { get; set; }
}