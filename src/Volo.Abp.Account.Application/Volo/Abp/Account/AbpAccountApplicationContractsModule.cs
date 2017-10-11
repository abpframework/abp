using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Account
{
    [DependsOn(typeof(AbpAccountApplicationContractsModule))]
    public class AbpAccountApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAccountApplicationModule>();
        }
    }
}
