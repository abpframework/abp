using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.Components.Server.Extensibility;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.EventBus;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(AbpAspNetCoreComponentsWebModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpEventBusModule),
    typeof(AbpAspNetCoreMvcContractsModule)
    )]
public class AbpAspNetCoreComponentsServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        StaticWebAssetsLoader.UseStaticWebAssets(context.Services.GetHostingEnvironment(), context.Services.GetConfiguration());
        context.Services.AddHttpClient(nameof(BlazorServerLookupApiRequestService))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All
            });
        var serverSideBlazorBuilder = context.Services.AddServerSideBlazor(options =>
        {
            if (context.Services.GetHostingEnvironment().IsDevelopment())
            {
                options.DetailedErrors = true;
            }
        });
        context.Services.ExecutePreConfiguredActions(serverSideBlazorBuilder);

        Configure<AbpAspNetCoreUnitOfWorkOptions>(options =>
        {
            options.IgnoredUrls.AddIfNotContains("/_blazor");
        });

        Configure<AbpAspNetCoreAuditingOptions>(options =>
        {
            options.IgnoredUrls.AddIfNotContains("/_blazor");
        });

        if (!context.Services.ExecutePreConfiguredActions<AbpAspNetCoreComponentsWebOptions>().IsBlazorWebApp)
        {
            var preConfigureActions = context.Services.GetPreConfigureActions<HttpConnectionDispatcherOptions>();
            Configure<AbpEndpointRouterOptions>(options =>
            {
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    endpointContext.Endpoints.MapBlazorHub(httpConnectionDispatcherOptions =>
                    {
                        preConfigureActions.Configure(httpConnectionDispatcherOptions);
                    });
                    endpointContext.Endpoints.MapFallbackToPage("/_Host");
                });
            });
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context.GetEnvironment().WebRootFileProvider =
            new CompositeFileProvider(
                new ManifestEmbeddedFileProvider(typeof(IServerSideBlazorBuilder).Assembly),
                new ManifestEmbeddedFileProvider(typeof(RazorComponentsEndpointRouteBuilderExtensions).Assembly),
                context.GetEnvironment().WebRootFileProvider
            );
    }
}
