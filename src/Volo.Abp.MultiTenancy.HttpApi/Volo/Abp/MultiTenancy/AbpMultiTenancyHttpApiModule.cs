using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpMultiTenancyApplicationModule), typeof(AbpAspNetCoreMvcModule))]
    public class AbpMultiTenancyHttpApiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpMultiTenancyHttpApiModule>();

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(
                    typeof(AbpMultiTenancyApplicationModule).Assembly,
                    opts =>
                    {
                        opts.RootPath = "multi-tenancy";
                    }
                );
            });
        }
    }
}