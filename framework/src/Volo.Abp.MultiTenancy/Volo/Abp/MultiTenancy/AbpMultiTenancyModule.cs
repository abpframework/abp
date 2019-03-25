using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.Security;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpSecurityModule)
        )]
    public class AbpMultiTenancyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<DefaultTenantStoreOptions>(configuration);
        }
    }
}
