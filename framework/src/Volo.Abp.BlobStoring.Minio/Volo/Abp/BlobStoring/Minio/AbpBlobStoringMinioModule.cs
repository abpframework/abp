using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Minio;

[DependsOn(typeof(AbpBlobStoringModule))]
public class AbpBlobStoringMinioModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMinioHttpClient();
    }
}
