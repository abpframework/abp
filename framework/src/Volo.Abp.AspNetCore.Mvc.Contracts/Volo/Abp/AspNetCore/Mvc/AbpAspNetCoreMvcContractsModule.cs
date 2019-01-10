using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(
        typeof(AbpDddApplicationModule)
        )]
    public class AbpAspNetCoreMvcContractsModule : AbpModule
    {

    }
}
