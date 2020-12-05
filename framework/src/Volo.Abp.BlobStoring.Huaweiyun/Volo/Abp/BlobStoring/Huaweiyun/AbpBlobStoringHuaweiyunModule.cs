using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    [DependsOn(
        typeof(AbpBlobStoringModule)
        )]
    public class AbpBlobStoringHuaweiyunModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient();
        }
    }
}
