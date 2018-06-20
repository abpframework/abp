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
        public override void ConfigureServices(IServiceCollection services)
        {
            AbpTenantManagementBsonClassMap.Configure();

            services.AddMongoDbContext<TenantManagementMongoDbContext>(options =>
            {
                options.AddDefaultRepositories<ITenantManagementMongoDbContext>();

                options.AddRepository<Tenant, MongoTenantRepository>();
            });

            services.AddAssemblyOf<AbpTenantManagementMongoDbModule>();
        }
    }
}
