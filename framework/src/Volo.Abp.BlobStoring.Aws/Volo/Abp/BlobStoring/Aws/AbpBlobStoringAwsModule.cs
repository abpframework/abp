using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Aws
{
    [DependsOn(typeof(AbpBlobStoringModule),
        typeof(AbpCachingModule))]
    public class AbpBlobStoringAwsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
