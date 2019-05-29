using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.Security;

namespace Volo.Abp.MultiTenancy
{
    //TODO: Create a Volo.Abp.MultiTenancy.Abstractions package?

    [DependsOn(
        typeof(DataModule),
        typeof(SecurityModule)
        )]
    public class MultiTenancyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<DefaultTenantStoreOptions>(configuration);
        }
    }
}
