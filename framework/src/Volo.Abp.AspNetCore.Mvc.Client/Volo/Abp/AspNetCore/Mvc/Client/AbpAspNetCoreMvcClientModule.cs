using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(AbpAspNetCoreMvcContractsModule)
        )]
    public class AbpAspNetCoreMvcClientModule : AbpModule
    {

    }
}
