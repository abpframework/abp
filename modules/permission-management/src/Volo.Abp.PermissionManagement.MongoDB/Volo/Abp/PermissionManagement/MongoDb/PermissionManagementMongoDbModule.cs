using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    [DependsOn(
        typeof(PermissionManagementDomainModule),
        typeof(MongoDbModule)
        )]
    public class PermissionManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<PermissionManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<IPermissionManagementMongoDbContext>();

                options.AddRepository<PermissionGrant, MongoPermissionGrantRepository>();
            });
        }
    }
}
