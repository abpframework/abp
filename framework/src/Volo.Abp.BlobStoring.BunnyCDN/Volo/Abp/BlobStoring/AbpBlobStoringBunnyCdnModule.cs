using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.BunnyCDN.Volo.Abp.BlobStoring
{
    [DependsOn(typeof(AbpBlobStoringModule))]
    public class AbpBlobStoringBunnyCdnModule : AbpModule
    {
    }
}
