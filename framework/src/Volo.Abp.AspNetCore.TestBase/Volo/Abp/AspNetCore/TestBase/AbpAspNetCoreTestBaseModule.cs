using Volo.Abp.Autofac;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.TestBase
{
    [DependsOn(typeof(AbpHttpClientModule))]
    [DependsOn(typeof(AbpAspNetCoreModule))]
    [DependsOn(typeof(AbpTestBaseModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    public class AbpAspNetCoreTestBaseModule : AbpModule
    {

    }
}