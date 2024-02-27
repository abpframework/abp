using System;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.SignalR;

[DependsOn(
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpAspNetCoreTestBaseModule),
    typeof(AbpAutofacModule)
    )]
public class AbpAspNetCoreSignalRTestModule : AbpModule
{
    public static Exception UseConfiguredEndpointsException { get; set; }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseRouting();

        UseConfiguredEndpointsException = null;
        try
        {
            app.UseConfiguredEndpoints();
        }
        catch (Exception e)
        {
            UseConfiguredEndpointsException = e;
        }
    }
}
