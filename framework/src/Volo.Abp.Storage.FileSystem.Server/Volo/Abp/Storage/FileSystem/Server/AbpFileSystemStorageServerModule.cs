using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.Modularity;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.FileSystem.Server
{
    [DependsOn(typeof(AbpFileSystemStorageModule))]
    public class AbpFileSystemStorageServerModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var options = app.ApplicationServices.GetRequiredService<IOptions<FileSystemStorageServerOptions>>();

            app.Map(
                options.Value.EndpointPath,storePipeline => {
                    storePipeline.UseMiddleware<FileSystemStorageServerMiddleware>();
                });
        }
    }
}
