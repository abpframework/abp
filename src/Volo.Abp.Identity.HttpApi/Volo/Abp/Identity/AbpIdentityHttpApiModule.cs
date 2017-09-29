using System;
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
                });
            });

            //TODO: Allow to use Api Versioning's API to explicitly configure versioning for app services and other controllers,
            //TODO: rather than implicitly doing it via AppServiceControllers.Create call above!
        }
    }
}