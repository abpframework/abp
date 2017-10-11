using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    public class AbpIdentityDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //TODO: How to configure options of AddIdentity (and return value)
            services.AddAbpIdentity();
            services.AddAssemblyOf<AbpIdentityDomainModule>();
        }
    }
}