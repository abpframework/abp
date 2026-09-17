using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Bunny;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpCachingModule))]
public class AbpBlobStoringBunnyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
    }
}
