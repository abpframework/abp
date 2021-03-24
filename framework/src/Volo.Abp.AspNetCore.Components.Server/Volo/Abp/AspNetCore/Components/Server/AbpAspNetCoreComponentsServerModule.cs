using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpAspNetCoreComponentsWebModule),
        typeof(AbpAspNetCoreSignalRModule)
        )]
    public class AbpAspNetCoreComponentsServerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddServerSideBlazor();
            
            Configure<AbpAspNetCoreUnitOfWorkOptions>(options =>
            {
                options.IgnoredUrls.AddIfNotContains("/_blazor");
            });
            
            Configure<AbpAspNetCoreAuditingOptions>(options =>
            {
                options.IgnoredUrls.AddIfNotContains("/_blazor");
            });
            
            Configure<AbpEndpointRouterOptions>(options =>
            {
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    endpointContext.Endpoints.MapBlazorHub();
                    endpointContext.Endpoints.MapFallbackToPage("/_Host");
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.GetEnvironment().WebRootFileProvider =
                new CompositeFileProvider(
                    new ManifestEmbeddedFileProvider(typeof(IServerSideBlazorBuilder).Assembly),
                    context.GetEnvironment().WebRootFileProvider
                );
        }
    }
}