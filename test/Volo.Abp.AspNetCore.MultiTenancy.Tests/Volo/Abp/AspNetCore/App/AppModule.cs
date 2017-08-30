using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.App
{
    [DependsOn(
        typeof(AbpAspNetMultiTenancyModule),
        typeof(AbpAspNetCoreTestBaseModule)
        )]
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MultiTenancyOptions>(options =>
            {
                options.AddDomainTenantResolver("{0}.abp.io");
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.Run(async (ctx) =>
            {
                var manager = ctx.RequestServices.GetRequiredService<IMultiTenancyManager>();
                var jsonSerializer = ctx.RequestServices.GetRequiredService<IJsonSerializer>();

                var dictionary = new Dictionary<string, string>
                {
                    ["TenantId"] = manager.CurrentTenant == null ? "" : manager.CurrentTenant.Id.ToString()
                };

                var result = jsonSerializer.Serialize(dictionary);
                await ctx.Response.WriteAsync(result);
            });
        }
    }
}
