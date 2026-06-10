using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB.DependencyInjection;
using Volo.Abp.Uow.MongoDB;
using Volo.Abp.MongoDB.DistributedEvents;

namespace Volo.Abp.MongoDB;

[DependsOn(typeof(AbpDddDomainModule))]
public class AbpMongoDbModule : AbpModule
{
    static AbpMongoDbModule()
    {
        AbpBsonSerializer.RemoveSerializer<Guid>();
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonTypeMapper.RegisterCustomTypeMapper(typeof(Guid), new AbpGuidCustomBsonTypeMapper());
    }

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddConventionalRegistrar(new AbpMongoDbConventionalRegistrar());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(
            typeof(IMongoDbContextProvider<>),
            typeof(UnitOfWorkMongoDbContextProvider<>)
        );

        context.Services.TryAddEnumerable(
            ServiceDescriptor.Transient(
                typeof(IMongoDbRepositoryFilterer<>),
                typeof(MongoDbRepositoryFilterer<>)
            ));

        context.Services.TryAddEnumerable(
            ServiceDescriptor.Transient(
                typeof(IMongoDbRepositoryFilterer<,>),
                typeof(MongoDbRepositoryFilterer<,>)
            ));

        context.Services.AddTransient(
            typeof(IMongoDbContextEventOutbox<>),
            typeof(MongoDbContextEventOutbox<>)
        );

        context.Services.AddTransient(
            typeof(IMongoDbContextEventInbox<>),
            typeof(MongoDbContextEventInbox<>)
        );

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.IgnoredEventSelectors.Add<OutgoingEventRecord>();
            options.IgnoredEventSelectors.Add<IncomingEventRecord>();
        });
    }
}
