using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationAbstractionsModule)
        )]
    public class BloggingApplicationContractsSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
