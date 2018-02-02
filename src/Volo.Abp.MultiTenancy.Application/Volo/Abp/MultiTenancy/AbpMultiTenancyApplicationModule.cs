using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyDomainModule))]
    [DependsOn(typeof(AbpMultiTenancyApplicationContractsModule))]
    public class AbpMultiTenancyApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpMultiTenancyApplicationModuleAutoMapperProfile>(validate: true);
            });

            services.AddAssemblyOf<AbpMultiTenancyApplicationModule>();
        }
    }
}