using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.MongoDB
{
    public class MongoModelBuilder : IMongoModelBuilder
    {
        private readonly Dictionary<Type, object> _entityModelBuilders;

        private static readonly object SyncObj = new object();

        public MongoModelBuilder()
        {
            _entityModelBuilders = new Dictionary<Type, object>();
        }

        public MongoDbContextModel Build(AbpMongoDbContext dbContext)
        {
            lock (SyncObj)
            {
                var entityModels = _entityModelBuilders
                    .Select(x => x.Value)
                    .Cast<IMongoEntityModel>()
                    .ToImmutableDictionary(x => x.EntityType, x => x);

                var baseClasses = new List<Type>();

                foreach (var entityModel in entityModels.Values)
                {
                    var map = entityModel.As<IHasBsonClassMap>().GetMap();
                    if (!BsonClassMap.IsClassMapRegistered(map.ClassType))
                    {
                        BsonClassMap.RegisterClassMap(map);
                    }

                    baseClasses.AddRange(entityModel.EntityType.GetBaseClasses(includeObject: false));

                    CreateCollectionIfNotExists(dbContext, entityModel.CollectionName);
                }

                baseClasses = baseClasses.Distinct().ToList();

                foreach (var baseClass in baseClasses)
                {
                    if (!BsonClassMap.IsClassMapRegistered(baseClass))
                    {
                        var map = new BsonClassMap(baseClass);
                        map.ConfigureAbpConventions();
                        BsonClassMap.RegisterClassMap(map);
                    }
                }

                return new MongoDbContextModel(entityModels);
            }
        }

        public virtual void Entity<TEntity>(Action<IMongoEntityModelBuilder<TEntity>> buildAction = null)
        {
            var model = (IMongoEntityModelBuilder<TEntity>)_entityModelBuilders.GetOrAdd(
                typeof(TEntity),
                () => new MongoEntityModelBuilder<TEntity>()
            );

            buildAction?.Invoke(model);
        }

        public virtual void Entity(Type entityType, Action<IMongoEntityModelBuilder> buildAction = null)
        {
            Check.NotNull(entityType, nameof(entityType));

            var model = (IMongoEntityModelBuilder)_entityModelBuilders.GetOrAdd(
                entityType,
                () => (IMongoEntityModelBuilder)Activator.CreateInstance(
                    typeof(MongoEntityModelBuilder<>).MakeGenericType(entityType)
                )
            );

            buildAction?.Invoke(model);
        }

        public virtual IReadOnlyList<IMongoEntityModel> GetEntities()
        {
            return _entityModelBuilders.Values.Cast<IMongoEntityModel>().ToImmutableList();
        }

        protected virtual void CreateCollectionIfNotExists(AbpMongoDbContext dbContext, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            if (!dbContext.Database.ListCollectionNames(options).Any())
            {
                dbContext.Database.CreateCollection(collectionName);
            }
        }
    }
}
