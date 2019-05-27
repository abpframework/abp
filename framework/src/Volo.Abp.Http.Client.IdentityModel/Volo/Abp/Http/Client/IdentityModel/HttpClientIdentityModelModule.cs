using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel
{
    [DependsOn(
        typeof(HttpClientModule),
        typeof(IdentityModelModule)
        )]
    public class HttpClientIdentityModelModule : AbpModule
    {

    }
}
