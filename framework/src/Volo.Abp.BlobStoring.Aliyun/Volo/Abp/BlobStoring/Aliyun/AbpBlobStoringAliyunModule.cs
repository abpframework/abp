using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Aliyun;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpCachingModule)
    )]
public class AbpBlobStoringAliyunModule : AbpModule
{

}
