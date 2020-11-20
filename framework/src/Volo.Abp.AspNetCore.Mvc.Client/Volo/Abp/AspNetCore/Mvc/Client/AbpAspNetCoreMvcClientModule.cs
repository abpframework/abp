using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcClientCommonModule)
        )]
    public class AbpAspNetCoreMvcClientModule : AbpModule
    {
    }
}
