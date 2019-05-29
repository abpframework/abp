using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.TestBase
{
    [DependsOn(typeof(HttpClientModule))]
    [DependsOn(typeof(AspNetCoreModule))]
    public class AspNetCoreTestBaseModule : AbpModule
    {

    }
}