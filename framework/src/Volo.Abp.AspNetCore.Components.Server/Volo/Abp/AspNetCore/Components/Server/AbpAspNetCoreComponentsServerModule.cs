using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server
{
    [DependsOn(
        typeof(AbpAspNetCoreModule)
        )]
    public class AbpAspNetCoreComponentsServerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddServerSideBlazor();
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