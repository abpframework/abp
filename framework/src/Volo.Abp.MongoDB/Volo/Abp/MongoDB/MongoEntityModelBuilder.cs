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

    BsonClassMap IMongoEntityModelBuilder.BsonMap => _bsonClassMap;

    BsonClassMap<TEntity> IMongoEntityModelBuilder<TEntity>.BsonMap => _bsonClassMap;

    private readonly BsonClassMap<TEntity> _bsonClassMap;
    
    CreateCollectionOptions<TEntity> IMongoEntityModelBuilder<TEntity>.CreateCollectionOptions => _createCollectionOptions;

    CreateCollectionOptions IMongoEntityModelBuilder.CreateCollectionOptions => _createCollectionOptions;

    private readonly CreateCollectionOptions<TEntity> _createCollectionOptions;

    public MongoEntityModelBuilder()
    {
        EntityType = typeof(TEntity);
        _bsonClassMap = new BsonClassMap<TEntity>();
        _createCollectionOptions = new CreateCollectionOptions<TEntity>();
        _bsonClassMap.ConfigureAbpConventions();
    }

    public BsonClassMap GetMap()
    {
        return _bsonClassMap;
    }

    public CreateCollectionOptions GetCreateCollectionOptions()
    {
        return _createCollectionOptions;
    }
}
