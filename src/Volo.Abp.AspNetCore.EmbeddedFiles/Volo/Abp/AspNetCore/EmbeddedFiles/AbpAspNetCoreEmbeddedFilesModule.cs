using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AbpAspNetCoreEmbeddedFilesModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreEmbeddedFilesModule>();
        }
    }
}
