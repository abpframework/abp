using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace Volo.Abp.Session
{
    [DependsOn(typeof(AbpSecurityModule))]
    public class AbpSessionModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpSessionModule>();
        }
    }
}
