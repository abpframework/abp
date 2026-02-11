using MongoDB.Bson.Serialization;
using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB;

public class MongoEntityModelBuilder<TEntity> :
    IMongoEntityModel,
    IHasBsonClassMap,
    IHasCreateCollectionOptions,
    IHasMongoIndexManagerAction,
    IMongoEntityModelBuilder,
    IMongoEntityModelBuilder<TEntity>
{
    public Type EntityType { get; }

    public string CollectionName { get; set; } = default!;
    
    public Action<IMongoIndexManager<BsonDocument>>? IndexesAction { get; set; }
    
    public CreateCollectionOptions<BsonDocument> CreateCollectionOptions { get; }

    BsonClassMap IMongoEntityModelBuilder.BsonMap => _bsonClassMap;

    BsonClassMap<TEntity> IMongoEntityModelBuilder<TEntity>.BsonMap => _bsonClassMap;

    private readonly BsonClassMap<TEntity> _bsonClassMap;

    public MongoEntityModelBuilder()
    {
        EntityType = typeof(TEntity);
        _bsonClassMap = new BsonClassMap<TEntity>();
        _bsonClassMap.ConfigureAbpConventions();
        CreateCollectionOptions = new CreateCollectionOptions<BsonDocument>();
    }

    public BsonClassMap GetMap()
    {
        return _bsonClassMap;
    }
    
    public void ConfigureIndexes(Action<IMongoIndexManager<BsonDocument>>? indexesAction)
    {
        IndexesAction = indexesAction;
    }
}
