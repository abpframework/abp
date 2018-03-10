using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions
{
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpPermissionsDomainSharedModule))]
    [DependsOn(typeof(AbpCachingModule))]
    [DependsOn(typeof(AbpJsonModule))]
    public class AbpPermissionsDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionsDomainModule>();
        }
    }
}
