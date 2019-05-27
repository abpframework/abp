using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(
        typeof(DddApplicationModule)
        )]
    public class AspNetCoreMvcContractsModule : AbpModule
    {

    }
}
