using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB.DependencyInjection;
using Volo.Abp.Uow.MongoDB;

namespace Volo.Abp.MongoDB
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpMongoDbModule : AbpModule
    {
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

            context.Services.TryAddTransient(
                typeof(IMongoDbRepositoryFilterer<>),
                typeof(MongoDbRepositoryFilterer<>)
            );

            context.Services.TryAddTransient(
                typeof(IMongoDbRepositoryFilterer<,>),
                typeof(MongoDbRepositoryFilterer<,>)
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            InitializeAllCollections(context.ServiceProvider);
        }

        private void InitializeAllCollections(IServiceProvider serviceProvider)
        {
            var dbContexts = serviceProvider.GetServices<IAbpMongoDbContext>();
            var connectionStringResolver = serviceProvider.GetService<IConnectionStringResolver>();

            foreach (var dbContext in dbContexts)
            {
                var connectionString = connectionStringResolver.Resolve(ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType()));
                var mongoUrl = new MongoUrl(connectionString);
                var databaseName = mongoUrl.DatabaseName;
                var client = new MongoClient(mongoUrl);

                if (databaseName.IsNullOrWhiteSpace())
                {
                    databaseName = ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType());
                }

                (dbContext as AbpMongoDbContext)?.InitializeCollections(client.GetDatabase(databaseName));
            }
        }
    }
}
