using Volo.Abp.Application;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(typeof(BloggingDomainSharedModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpBlobStoringModule))]
    public class BloggingApplicationContractsSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
