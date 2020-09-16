using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MyCompanyName.MyProjectName.Data;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public class MongoDbMyProjectNameDbSchemaMigrator : IMyProjectNameDbSchemaMigrator , ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public MongoDbMyProjectNameDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task MigrateAsync()
        {
            var dbContexts = _serviceProvider.GetServices<IAbpMongoDbContext>();
            var connectionStringResolver = _serviceProvider.GetService<IConnectionStringResolver>();

            foreach (var dbContext in dbContexts)
            {
                var connectionString =
                    connectionStringResolver.Resolve(
                        ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType()));
                var mongoUrl = new MongoUrl(connectionString);
                var databaseName = mongoUrl.DatabaseName;
                var client = new MongoClient(mongoUrl);

                if (databaseName.IsNullOrWhiteSpace())
                {
                    databaseName = ConnectionStringNameAttribute.GetConnStringName(dbContext.GetType());
                }

                (dbContext as AbpMongoDbContext)?.InitializeCollections(client.GetDatabase(databaseName));
            }

            return Task.CompletedTask;
        }
    }
}
