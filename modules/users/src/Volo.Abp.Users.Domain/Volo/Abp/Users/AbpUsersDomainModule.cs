using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Settings;

namespace Volo.Abp.Users
{
    [DependsOn(
        typeof(AbpUsersDomainSharedModule),
        typeof(AbpUsersAbstractionModule),
        typeof(AbpSecurityModule),
        typeof(AbpSettingsModule)
        )]
    public class AbpUsersDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpUsersDomainModule>();
        }
    }
}
