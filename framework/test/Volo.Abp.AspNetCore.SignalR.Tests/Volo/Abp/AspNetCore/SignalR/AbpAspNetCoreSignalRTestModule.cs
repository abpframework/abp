using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR.SampleHubs;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.SignalR;

[DependsOn(
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAutofacModule)
    )]
public class AbpAspNetCoreSignalRTestModule : AbpModule
{
    public const string UnitOfWorkReservedHeaderName = "X-Unit-Of-Work-Reserved";

    public static Exception UseConfiguredEndpointsException { get; set; }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseRouting();
        app.UseUnitOfWork();
        app.Use(async (httpContext, next) =>
        {
            httpContext.Response.Headers[UnitOfWorkReservedHeaderName] =
                (httpContext.RequestServices.GetRequiredService<IAmbientUnitOfWork>().UnitOfWork?.IsReserved == true).ToString();

            await next();
        });

        UseConfiguredEndpointsException = null;
        try
        {
            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapHub<DisableAutoHubMapHub>("/manually-mapped-hub");
            });
        }
        catch (Exception e)
        {
            UseConfiguredEndpointsException = e;
        }
    }
}
