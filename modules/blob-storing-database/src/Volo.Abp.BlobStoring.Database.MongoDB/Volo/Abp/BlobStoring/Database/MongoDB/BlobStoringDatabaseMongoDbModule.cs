using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    [DependsOn(
        typeof(BlobStoringDatabaseDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class BlobStoringDatabaseMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<BlobStoringDatabaseMongoDbContext>(options =>
            {
                options.AddRepository<Container, MongoContainerRepository>();
                options.AddRepository<Blob, MongoBlobRepository>();
            });
        }
    }
}
