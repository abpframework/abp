using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Minio
{
    [DependsOn(typeof(AbpBlobStoringModule))]
    public class AbpBlobStoringMinioModule : AbpModule
    {

    }
}
