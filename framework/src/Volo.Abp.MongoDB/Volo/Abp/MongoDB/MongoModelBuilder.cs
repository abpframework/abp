using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.MongoDB;

public class MongoModelBuilder : IMongoModelBuilder
{
    private readonly Dictionary<Type, object> _entityModelBuilders;

    private static readonly object SyncObj = new object();

    public MongoModelBuilder()
    {
        _entityModelBuilders = new Dictionary<Type, object>();
    }

    public virtual MongoDbContextModel Build(AbpMongoDbContext dbContext)
    {
        lock (SyncObj)
        {
            var useAbpClockHandleDateTime = dbContext.LazyServiceProvider.LazyGetRequiredService<IOptions<AbpMongoDbOptions>>().Value.UseAbpClockHandleDateTime;

            var entityModels = _entityModelBuilders
                .Select(x => x.Value)
                .Cast<IMongoEntityModel>()
                .ToImmutableDictionary(x => x.EntityType, x => x);

            var baseClasses = new List<Type>();

            foreach (var entityModel in entityModels.Values)
            {
                var map = entityModel.As<IHasBsonClassMap>().GetMap();

                if (useAbpClockHandleDateTime)
                {
                    var dateTimePropertyInfos = entityModel.EntityType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(property =>
                            (property.PropertyType == typeof(DateTime) ||
                             property.PropertyType == typeof(DateTime?)) &&
                            property.CanWrite
                        ).ToList();

                    dateTimePropertyInfos.ForEach(property =>
                    {
                        var disableDateTimeNormalization =
                            entityModel.EntityType.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true) ||
                            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableDateTimeNormalizationAttribute>(property) != null;

                        if (property.PropertyType == typeof(DateTime?))
                        {
                            map.MapProperty(property.Name).SetSerializer(new NullableSerializer<DateTime>().WithSerializer(new AbpMongoDbDateTimeSerializer(GetDateTimeKind(dbContext), disableDateTimeNormalization)));
                        }
                        else
                        {
                            map.MapProperty(property.Name).SetSerializer(new AbpMongoDbDateTimeSerializer(GetDateTimeKind(dbContext), disableDateTimeNormalization));
                        }
                    });
                }

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

                    if (useAbpClockHandleDateTime)
                    {
                        var dateTimePropertyInfos = baseClass.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                            .Where(property =>
                                (property.PropertyType == typeof(DateTime) ||
                                 property.PropertyType == typeof(DateTime?)) &&
                                property.CanWrite
                            ).ToList();

                        dateTimePropertyInfos.ForEach(property =>
                        {
                            if (property.PropertyType == typeof(DateTime?))
                            {
                                map.MapProperty(property.Name).SetSerializer(new NullableSerializer<DateTime>().WithSerializer(new AbpMongoDbDateTimeSerializer(GetDateTimeKind(dbContext), false)));
                            }
                            else
                            {
                                map.MapProperty(property.Name).SetSerializer(new AbpMongoDbDateTimeSerializer(GetDateTimeKind(dbContext), false));
                            }
                        });
                    }

                    BsonClassMap.RegisterClassMap(map);
                }
            }

            return new MongoDbContextModel(entityModels);
        }
    }

    private DateTimeKind GetDateTimeKind(AbpMongoDbContext dbContext)
    {
        return dbContext.LazyServiceProvider.LazyGetRequiredService<IOptions<AbpClockOptions>>().Value.Kind;
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
