using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingDomainSharedModule),
        typeof(DddDomainModule))]
    public class BloggingDomainModule : AbpModule
    {

    }
}
