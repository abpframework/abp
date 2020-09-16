using System;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly
{
    [DependsOn(
        typeof(AbpHttpClientIdentityModelModule)
    )]
    public class AbpHttpClientIdentityModelWebAssemblyModule : AbpModule
    {

    }
}
