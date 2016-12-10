using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpDesk.EntityFrameworkCore
{
    [DependsOn(typeof(AbpDeskDomainModule))]
    public class AbpDeskEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpDeskEntityFrameworkCoreModule>();
        }
    }
}
