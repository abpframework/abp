using MongoDB.Bson.Serialization;
using System;

namespace Volo.Abp.MongoDB
{
    public class MongoEntityModelBuilder<TEntity> : 
        IMongoEntityModel, 
        IHasBsonClassMap,
        IMongoEntityModelBuilder,
        IMongoEntityModelBuilder<TEntity>
    {
        public Type EntityType { get; }

        public string CollectionName { get; set; }

        BsonClassMap IMongoEntityModelBuilder.BsonMap => _bsonClassMap;
        BsonClassMap<TEntity> IMongoEntityModelBuilder<TEntity>.BsonMap => _bsonClassMap;

        private readonly BsonClassMap<TEntity> _bsonClassMap;

        public MongoEntityModelBuilder()
        {
            EntityType = typeof(TEntity);
            _bsonClassMap = new BsonClassMap<TEntity>();
            _bsonClassMap.ConfigureAbpConventions();
        }

        public BsonClassMap GetMap()
        {
            return _bsonClassMap;
        }
    }
}