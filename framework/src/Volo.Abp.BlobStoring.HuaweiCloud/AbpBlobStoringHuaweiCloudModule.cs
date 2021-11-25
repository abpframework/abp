using System;
using Volo.Abp.Modularity;
using Volo.Abp.BlobStoring;
namespace Volo.Abp.BlobStoring.HuaweiCloud
{
    [DependsOn(typeof(AbpBlobStoringModule))]
    public class AbpBlobStoringHuaweiCloudModule : AbpModule
    {

    }
}
