using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Authorization
{
    public class AbpAuthorizationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                
            });

            services.AddAssemblyOf<AbpAuthorizationModule>();
        }
    }
}
