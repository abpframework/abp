using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Memory;

[DependsOn(typeof(AbpBlobStoringModule))]
public class AbpBlobStoringMemoryModule : AbpModule
{

}
