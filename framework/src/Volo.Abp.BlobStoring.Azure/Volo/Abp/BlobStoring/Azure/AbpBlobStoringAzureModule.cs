using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Azure
{
    [DependsOn(typeof(AbpBlobStoringModule))]
    public class AbpBlobStoringAzureModule : AbpModule
    {

    }
}
