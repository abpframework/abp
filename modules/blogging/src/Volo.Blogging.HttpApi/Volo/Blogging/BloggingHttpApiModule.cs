using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class BloggingHttpApiModule : AbpModule
    {

    }
}
