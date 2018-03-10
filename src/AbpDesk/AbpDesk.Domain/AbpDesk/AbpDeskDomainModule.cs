using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpDeskDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDeskDomainModule>();
        }
    }
}
