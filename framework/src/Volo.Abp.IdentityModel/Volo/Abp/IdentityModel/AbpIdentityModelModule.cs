using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.IdentityModel
{
    [DependsOn(
        typeof(AbpThreadingModule)
        )]
    public class AbpIdentityModelModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpIdentityClientOptions>(configuration);
        }
    }
}
