using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityApplicationModule), typeof(AbpAspNetCoreMvcModule))]
    public class AbpIdentityHttpApiModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpIdentityHttpApiModule>();

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.AppServiceControllers.Create(typeof(AbpIdentityApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "identity";
                    opts.UrlControllerNameNormalizer = context => context.ControllerName.RemovePreFix("Identity");
                    opts.ApiVersion = new ApiVersion(2, 0, "beta");
                });
            });
        }
    }
}