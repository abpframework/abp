using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpDesk
{
    [DependsOn(typeof(AbpDeskDomainModule))]
    public class AbpDeskApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDeskApplicationModule>();
        }
    }
}
