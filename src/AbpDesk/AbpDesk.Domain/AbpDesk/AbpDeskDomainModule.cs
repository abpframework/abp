using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddModule))]
    public class AbpDeskDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDeskDomainModule>();
        }
    }
}
