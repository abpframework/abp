using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    [DependsOn(
        typeof(AbpPermissionManagementDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class AbpPermissionManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            AbpPermissionManagementBsonClassMap.Configure();

            services.AddMongoDbContext<PermissionManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<IPermissionManagementMongoDbContext>();

                options.AddRepository<PermissionGrant, MongoPermissionGrantRepository>();
            });

            services.AddAssemblyOf<AbpPermissionManagementMongoDbModule>();
        }
    }
}
