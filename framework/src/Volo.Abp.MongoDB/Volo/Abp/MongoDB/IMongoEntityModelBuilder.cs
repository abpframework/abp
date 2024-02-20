using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB;

public interface IMongoEntityModelBuilder<TEntity>
{
    Type EntityType { get; }

    string CollectionName { get; set; }

    BsonClassMap<TEntity> BsonMap { get; }
    
    CreateCollectionOptions<BsonDocument> CreateCollectionOptions { get; }
    
    void ConfigureIndexes(Action<IMongoIndexManager<BsonDocument>> action);
}

public interface IMongoEntityModelBuilder
{
    Type EntityType { get; }

    string CollectionName { get; set; }

    BsonClassMap BsonMap { get; }
    
    CreateCollectionOptions<BsonDocument> CreateCollectionOptions { get; }
    
    void ConfigureIndexes(Action<IMongoIndexManager<BsonDocument>> action);
}
