using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDb
{
    [DependsOn(
        typeof(AbpTenantManagementDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class AbpTenantManagementMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AbpTenantManagementBsonClassMap.Configure();

            context.Services.AddMongoDbContext<TenantManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<ITenantManagementMongoDbContext>();

                options.AddRepository<Tenant, MongoTenantRepository>();
            });

            context.Services.AddAssemblyOf<AbpTenantManagementMongoDbModule>();
        }
    }
}
