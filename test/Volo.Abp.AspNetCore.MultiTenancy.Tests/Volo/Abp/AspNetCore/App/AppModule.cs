using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.App
{
    [DependsOn(typeof(AbpAspNetMultiTenancyModule))]
    public class AppModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.Run(async (ctx) =>
            {
                var manager = ctx.RequestServices.GetRequiredService<IMultiTenancyManager>();

                var dictionary = new Dictionary<string, string>
                {
                    ["TenantName"] = manager.CurrentTenant?.Name ?? "<host>"
                };
                
                await ctx.Response.WriteAsync(dictionary.ToJsonString());
            });
        }
    }
}
