using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Users.MongoDB
{
    [DependsOn(
        typeof(AbpUsersDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class AbpUsersMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpUsersMongoDbModule>();
        }
    }
}
