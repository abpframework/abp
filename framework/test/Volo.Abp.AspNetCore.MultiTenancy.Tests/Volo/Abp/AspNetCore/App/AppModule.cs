using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.App;

[DependsOn(
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreTestBaseModule)
    )]
public class AppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseMultiTenancy();

        app.Run(async (ctx) =>
        {
            var currentTenant = ctx.RequestServices.GetRequiredService<ICurrentTenant>();
            var jsonSerializer = ctx.RequestServices.GetRequiredService<IJsonSerializer>();

            var dictionary = new Dictionary<string, string>
            {
                ["TenantId"] = currentTenant.IsAvailable ? currentTenant.Id.ToString() : ""
            };

            var result = jsonSerializer.Serialize(dictionary, camelCase: false);
            await ctx.Response.WriteAsync(result);
        });
    }
}
