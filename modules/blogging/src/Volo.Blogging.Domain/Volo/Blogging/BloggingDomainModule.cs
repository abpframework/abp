using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingDomainSharedModule),
        typeof(AbpDddDomainModule))]
    public class BloggingDomainModule : AbpModule
    {

    }
}
