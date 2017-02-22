using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    //TODO: Consider to remove this module and merge to Volo.Abp.AspNetCore. The only problem is this module depends on static files (can we remove dependency with a custom middleware?)
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AbpAspNetCoreEmbeddedFilesModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreEmbeddedFilesModule>();
        }
    }
}
