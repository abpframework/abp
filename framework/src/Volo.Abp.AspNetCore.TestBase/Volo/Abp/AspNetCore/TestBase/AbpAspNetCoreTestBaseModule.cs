using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.TestBase
{
    [DependsOn(typeof(AbpHttpClientModule))]
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AbpAspNetCoreTestBaseModule : AbpModule
    {

    }
}