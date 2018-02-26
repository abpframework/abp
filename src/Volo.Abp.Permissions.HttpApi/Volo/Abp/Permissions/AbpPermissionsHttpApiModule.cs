using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions
{
    [DependsOn(typeof(AbpPermissionsApplicationModule), typeof(AbpAspNetCoreMvcModule))]
    public class AbpPermissionsHttpApiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionsHttpApiModule>();

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(
                    typeof(AbpPermissionsApplicationModule).Assembly,
                    opts =>
                    {
                        opts.RootPath = "permissions";
                    }
                );
            });
        }
    }
}