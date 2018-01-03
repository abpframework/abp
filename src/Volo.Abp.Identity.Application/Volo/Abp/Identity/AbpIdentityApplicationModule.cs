using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.ObjectMappings;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityDomainModule), 
        typeof(AbpIdentityApplicationContractsModule), 
        typeof(AbpAutoMapperModule)
        )]
    public class AbpIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityApplicationModule>();

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityApplicationModuleAutoMapperProfile>();
            });
        }
    }
}