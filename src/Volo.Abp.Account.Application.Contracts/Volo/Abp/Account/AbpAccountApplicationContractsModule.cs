using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account
{
    [DependsOn(typeof(AbpCommonModule))]
    public class AbpAccountApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAccountApplicationContractsModule>();
        }
    }
}
