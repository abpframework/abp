using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring
{
    [DependsOn(
        typeof(AbpBlobStoringModule),
        typeof(AbpTestBaseModule)
        )]
    public class AbpBlobStoringTestModule : AbpModule
    {
        
    }
}