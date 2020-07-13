using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Aliyun
{
    /// <summary>
    /// https://help.aliyun.com/document_detail/31817.html
    /// </summary>
    [DependsOn(
        typeof(AbpBlobStoringModule),
        typeof(AbpCachingModule)
        )]
    public class AbpBlobStoringAliyunModule: AbpModule
    {

    }
}
