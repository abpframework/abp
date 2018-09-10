using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingApplicationContractsModule))]
    public class BloggingHttpApiModule : AbpModule
    {

    }
}
